// File create date:2022/7/5
using System;
using System.Collections.Generic;
using RoachLite.Basic;
using RoachLite.Common;
using RoachLite.Configs;
using RoachLite.Services;
using RoachLite.Services.Broadcast;
using RoachLite.Services.Message;
using RoachLite.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace RoachLite.UIComponent {
	/// <summary>
	/// 窗口管理器，全局单例，不可销毁，包含静态窗体所需的Canvas
	/// </summary>
	[RequireComponent(typeof(Canvas))]
	[RequireComponent(typeof(CanvasScaler))]
	[RequireComponent(typeof(GraphicRaycaster))]
	public sealed class WindowManager : MonoSingleton<WindowManager>, IMessageReceiver {

		private const string Oid = "WindowManager";
		private const string Domain = "Runtime";
		public string Identifier => ObjectManager.CompileIdentifier(Oid, Domain);

		private Canvas _managerCanvas;

		private readonly Dictionary<string, WindowInfo> _windowSet =
			new Dictionary<string, WindowInfo>();

		private readonly List<WindowInfo> _windowStack = new List<WindowInfo>();

		public MessageService MessageService { get; private set; }
		private ITagFactory<GameObject> _windowFactory;

		public event Action<bool> OverallWindowStateCallabck;
		
		public void ReceiveMessage(MessageInfo e) {
			EnqueueMessage(e);
		}

		public void SetupWindowFactory(ITagFactory<GameObject> factory) {
			_windowFactory = factory;
		}

		public static void RequestWindowShow(string wid, string domain, string src,
			Dictionary<string, object> extra) {
			if (Instance == null) throw new NullReferenceException("No Window Manager");
			var builder =
				MessageUtils.GetMessageBuilder(CommonConfigs.MSG_TYPE_COMMON_UI_FUNCTION);
			builder.SetupContent(CommonConfigs.MSG_CONTENT_WINDOW_OPEN)
				.SetupActivate(true).SetupFromTo(Instance.Identifier, src)
				.SetupConsumable(false).SetupPriority(0);
			builder.PutExtra(CommonConfigs.EXTRA_TAG_CALLBACK_TARGET, src);
			builder.PutExtra(CommonConfigs.EXTRA_TAG_WINDOW_IDENTIFIER, wid);
			builder.PutExtra(CommonConfigs.EXTRA_TAG_WINDOW_DOMAIN, domain);
			if (extra != null && extra.Count > 0) {
				foreach (var key in extra.Keys) {
					builder.PutExtra(key, extra[key]);
				}
			}

			Instance.MessageService.SubmitMessage(builder.Build());
		}

		protected override void OnAwake() {
			base.OnAwake();
			ObjectManager.Instance.RegisterObject(Identifier, gameObject);
		}

		protected override void OnStart() {
			base.OnStart();
			MessageService = ServiceProvider.Instance.ProvideService<MessageService>(
				MessageService.SERVICE_NAME);
			MessageService.RegisterReceiver(Identifier, this);
			var filter = new BroadcastFilter(
				CommonConfigs.BROADCAST_FILTER_WINDOW_OPEN,
				CommonConfigs.BROADCAST_FILTER_WINDOW_CLOSE);
			RegisterBroadcastReceiver(filter, ReceiveBroadcast);
		}

		protected override void HandleMessage(MessageInfo msg) {
			base.HandleMessage(msg);
			if (msg.Type == CommonConfigs.MSG_TYPE_COMMON_UI_FUNCTION) {
				if (msg.Content == CommonConfigs.MSG_CONTENT_WINDOW_OPEN) {
					var cData = new Dictionary<string, object>();
					foreach (var key in msg.ExtraKeys) {
						cData[key] = msg.GetExtra<object>(key);
					}

					var wid = cData.TakeAndRemove(CommonConfigs.EXTRA_TAG_WINDOW_IDENTIFIER)
						.ToString();
					var domain = cData.TakeAndRemove(CommonConfigs.EXTRA_TAG_WINDOW_DOMAIN)
						.ToString();
					var id = ObjectManager.CompileIdentifier(wid, domain);
					if (!string.IsNullOrEmpty(id) && _windowSet.ContainsKey(id)) {
						var info = _windowSet[id];
						if (cData.ContainsKey(CommonConfigs.EXTRA_TAG_WINDOW_POSITION)) {
							var pos = (Vector2) cData.TakeAndRemove(CommonConfigs
								.EXTRA_TAG_WINDOW_POSITION);
							info.windowObject.ShowWindowAtPosition(pos, cData);
						} else {
							info.windowObject.ShowWindow(cData);
						}

						UpdateWindowOrder(id);
					}
				}
			}
		}

		private void ReceiveBroadcast(BroadcastInfo msg) {
			if (msg.Action == CommonConfigs.BROADCAST_FILTER_WINDOW_OPEN) {
				var wid = msg.Content;
				if (_windowSet.ContainsKey(wid)) {
					_windowSet[wid].isActive = true;
				} else {
					LogUtils.LogWarning(
						$"Cannot Find Window with Identifier - [{wid}] when showing it.");
				}

				UpdateWindowState();
			} else if (msg.Action == CommonConfigs.BROADCAST_FILTER_WINDOW_CLOSE) {
				var wid = msg.Content;
				if (_windowSet.ContainsKey(wid)) {
					_windowSet[wid].isActive = false;
				} else {
					LogUtils.LogWarning(
						$"Cannot Find Window with Identifier - [{wid}] when hiding it.");
				}

				UpdateWindowState();
			}
		}

		public void RegisterWindow(BaseWindow window, bool isStatic = false) {
			var id = window.GetComponent<ObjectIdentifier>();
			if (!_windowSet.ContainsKey(id.Identifier)) {
				if (isStatic) {
					window.transform.SetParent(transform);
				}

				window.RegisterDestroyCallback(OnWindowDestroy);
				var info = new WindowInfo(window, isStatic);
				_windowSet[id.Identifier] = info;
				if (!window.startShow) return;
				info.windowOrder = _windowStack.Count;
				_windowStack.Add(info);
			} else {
				LogUtils.LogWarning(
					$"Cannot Register Window[{window.name}] because Identifier Duplication - [{id.Identifier}]");
			}
		}

		public void CreateWindow(string wTag, Transform canvasRoot,
			bool isStatic = false) {
			if (_windowFactory == null) {
				Debug.LogWarning($"Cannot create window: {wTag} because no Window Factory!");
				return;
			}

			var winObj = _windowFactory.Create(wTag);
			var window = winObj != null ? winObj.GetComponent<BaseWindow>() : null;
			if (window != null) {
				winObj.transform.SetParent(canvasRoot == null ? transform : canvasRoot);
			} else {
				LogUtils.LogError($"Cannot create window {wTag}, please check the Factory.");
			}
		}

		public void SetWindowStatic(string id, bool isStatic) {
			if (string.IsNullOrEmpty(id) || !_windowSet.ContainsKey(id)) return;
			if (!_windowSet[id].isStatic && isStatic) {
				_windowSet[id].windowObject.transform.SetParent(transform);
			}

			_windowSet[id].windowObject.isStatic = isStatic;
			_windowSet[id].isStatic = isStatic;
		}

		public void UnregisterWindow(string id) {
			if (string.IsNullOrEmpty(id) || !_windowSet.ContainsKey(id)) return;
			RemoveWindowFromStack(id);
			_windowSet.Remove(id);
		}

		private void OnWindowDestroy(GameObject winObj) {
			var id = winObj.GetComponent<ObjectIdentifier>();
			UnregisterWindow(id.Identifier);
		}

		private void UpdateWindowState() {
			var state = false;
			foreach (var id in _windowSet.Keys) {
				state |= _windowSet[id].isActive;
			}

			OverallWindowStateCallabck?.Invoke(state);
		}

		/// <summary>
		/// 将指定ID的窗口挪到栈顶
		/// </summary>
		/// <param name="id">窗口ID</param>
		private void UpdateWindowOrder(string id) {
			if (!string.IsNullOrEmpty(id) && _windowSet.ContainsKey(id)) {
				var info = RemoveWindowFromStack(id);
				info.windowOrder = _windowStack.Count;
				info.windowObject.SetupSortingOrder(info.windowOrder + 1);
				_windowStack.Add(info);
			}
		}

		private WindowInfo RemoveWindowFromStack(string id) {
			var info = _windowSet[id];
			var prevOrder = info.windowOrder;
			if (prevOrder >= 0 && prevOrder < _windowStack.Count) {
				_windowStack.RemoveAt(prevOrder);
				for (var i = prevOrder; i < _windowStack.Count; i++) {
					_windowStack[i].windowOrder--;
					_windowStack[i].windowObject
						.SetupSortingOrder(_windowStack[i].windowOrder + 1);
				}
			}

			info.windowOrder = -1;
			return info;
		}

		private void ClearWindows() {
			var wids = new HashSet<string>(_windowSet.Keys);
			foreach (var wid in wids) {
				UnregisterWindow(wid);
			}
		}

		protected override void Release() {
			base.Release();
			ClearWindows();
			MessageService?.UnregisterReceiver(Identifier);
			ObjectManager.Instance.UnregisterObject(Identifier);
		}

		private class WindowInfo {

			public string objectName;
			public string windowIdentifier;
			public string windowDomain;
			public BaseWindow windowObject;
			public int windowOrder;
			public bool isStatic;
			public bool isActive;

			public WindowInfo(BaseWindow window, bool isStatic) {
				objectName = window.name;
				var id = window.GetComponent<ObjectIdentifier>();
				windowIdentifier = id.identifier;
				windowDomain = id.domain;
				windowObject = window;
				windowOrder = -1;
				this.isStatic = isStatic;
				isActive = false;
			}
		}
	}
}