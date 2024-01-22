// File create date:2022/7/5
using System;
using System.Collections.Generic;
using RoachLite.Common;
using RoachLite.Configs;
using RoachLite.Services.Broadcast;
using RoachLite.UIComponent;
using RoachLite.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

// Created By Yu.Liu
namespace RoachLite.Basic {
	/// <summary>
	/// 窗口组件抽象基类
	/// </summary>
	[RequireComponent(typeof(ObjectIdentifier))]
	public abstract class BaseWindow : BasePanel, IDragable {
		
		public bool isStatic; // 是否静态对象，即是否不会随场景加载而销毁
		public bool isPopup; // 是否支持弹出
		public bool isDragable; // 是否支持拖拽
		public DragableArea dragableArea;

		protected RectTransform rootTransform;
		protected Vector2 currentPointerPos; // 拖拽时的指针位置缓存
		protected bool isDragging; // 是否正在拖动

		protected string callbackTarget;
		protected Dictionary<string, object> callbackData;

		protected abstract Camera UiCamera { get; }

		protected override void PreLoad() {
			base.PreLoad();
			hideRaycast = false;
			hideRootObject = true;
		}

		protected override void LoadViews() {
			base.LoadViews();
			if (isDragable && dragableArea != null) {
				dragableArea.SetupDragable(this);
			}
		}

		protected override void PostLoad() {
			base.PostLoad();
			WindowManager.Instance.RegisterWindow(this);
		}

		protected virtual void BeforeWindowShow() { }

		/// <summary>
		/// 展示窗体
		/// </summary>
		/// <param name="cData">附带数据</param>
		public virtual void ShowWindow(Dictionary<string, object> cData = null) {
			callbackData = cData ?? new Dictionary<string, object>();
			callbackTarget =
				ExcludeCallbackData<string>(CommonConfigs.EXTRA_TAG_CALLBACK_TARGET);
			BeforeWindowShow();
			Show(!isPopup);
		}

		public virtual void ShowWindowAtPosition(Vector2 pos,
			Dictionary<string, object> cData = null) {
			ViewRect.anchoredPosition = pos;
			ShowWindow(cData);
		}

		/// <summary>
		/// 关闭窗体
		/// </summary>
		public virtual void DismissWindow() {
			if (!string.IsNullOrEmpty(callbackTarget)) {
				var builder = MessageUtils.GetMessageBuilder(
					CommonConfigs.MSG_TYPE_COMMON_CALLBACK);
				SetupCallbackData(builder);
				var e = builder
					.SetupFromTo(callbackTarget, identifier.Identifier)
					.SetupContent(CommonConfigs.MSG_CONTENT_CALLBACK_WINDOW)
					.PutExtra(CommonConfigs.EXTRA_TAG_WINDOW_IDENTIFIER, identifier.identifier)
					.PutExtra(CommonConfigs.EXTRA_TAG_WINDOW_DOMAIN, identifier.domain)
					.Build();
				messageService.SubmitMessage(e);
			}
			Hide(!isPopup);
		}

		protected T ExcludeCallbackData<T>(string key) {
			var result = (T)callbackData.TryGetElement(key);
			callbackData.Remove(key);
			return result;
		}

		protected void IncludeCallbackData(string key, object value) {
			callbackData[key] = value;
		}

		private void SetupCallbackData(MessageBuilder builder) {
			foreach (var key in callbackData.Keys) {
				builder.PutExtra(key, callbackData[key]);
			}
		}

		protected override void OnAfterShow() {
			var msg = BroadcastInfo.Create(
				CommonConfigs.BROADCAST_FILTER_WINDOW_OPEN, identifier.Identifier);
			msg.PutStringExtra(CommonConfigs.EXTRA_TAG_WINDOW_IDENTIFIER,
				identifier.identifier);
			msg.PutStringExtra(CommonConfigs.EXTRA_TAG_WINDOW_DOMAIN, identifier.domain);
			broadcastService.BroadcastInformation(msg);
			base.OnAfterShow();
		}

		protected override void OnAfterHide() {
			var msg = BroadcastInfo.Create(
				CommonConfigs.BROADCAST_FILTER_WINDOW_CLOSE, identifier.Identifier);
			msg.PutStringExtra(CommonConfigs.EXTRA_TAG_WINDOW_IDENTIFIER,
				identifier.identifier);
			msg.PutStringExtra(CommonConfigs.EXTRA_TAG_WINDOW_DOMAIN, identifier.domain);
			broadcastService.BroadcastInformation(msg);
			base.OnAfterHide();
		}

		public void OnDragBegin(PointerEventData pointer) {
			isDragging = RectTransformUtility.ScreenPointToLocalPointInRectangle(
				rootTransform, pointer.position, UiCamera, out currentPointerPos);
		}

		public void OnDragUpdate(PointerEventData pointer) {
			if (!isDragging) return;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rootTransform,
				pointer.position, UiCamera, out var pos)) return;
			var delta = pos - currentPointerPos;
			var nextPos = ViewRect.anchoredPosition + delta;
			ViewRect.anchoredPosition = nextPos;
			currentPointerPos = pos;
		}

		public void OnDragFinish(PointerEventData pointer) {
			isDragging = false;
		}

		protected override void Release() {
			base.Release();
			if (WindowManager.Instance == null) return;
			WindowManager.Instance.UnregisterWindow(identifier.Identifier);
		}
	}
}