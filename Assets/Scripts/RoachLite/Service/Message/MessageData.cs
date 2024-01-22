// File create date:2022/7/2
using System;
using System.Collections.Generic;
using RoachLite.Utils;
using UnityEngine;

namespace RoachLite.Services.Message {

	public class MessageInfo {
		public long Identifier { get; private set; }
		public int Type { get; private set; }
		public string Content { get; private set; }
		public bool Activate { get; private set; }
		public Action<MessageInfo> Callback { get; private set; }
		public int Priority { get; private set; }
		public string Source { get; private set; }
		public string Target { get; private set; }
		public bool Consumable { get; private set; }
		public int consumerCount;
		private Dictionary<string, object> _extras;
		public IEnumerable<string> ExtraKeys => _extras?.Keys;

		public MessageInfo(int type) {
			Identifier = DateTime.UtcNow.Ticks;
			Type = type;
			consumerCount = 0;
		}
		
		/// <summary>
		/// 放入额外信息
		/// </summary>
		/// <param name="key">额外信息关键字</param>
		/// <param name="value">额外信息内容</param>
		public void PutExtra(string key, object value) {
			_extras ??= new Dictionary<string, object>();
			_extras[key] = value;
		}
		
		/// <summary>
		/// 获取额外信息
		/// </summary>
		/// <param name="key">额外信息关键字</param>
		/// <returns>额外信息内容</returns>
		public T GetExtra<T>(string key) {
			T result = default;
			if (_extras != null && key != null) {
				result = (T)_extras.TryGetElement(key);
			}
			return result;
		}

		/// <summary>
		/// 重置消息ID
		/// </summary>
		public void ResetMessage() {
			Identifier = DateTime.UtcNow.Ticks;
			consumerCount = 0;
			Source = string.Empty;
			Target = string.Empty;
			Content = string.Empty;
			Activate = false;
			Priority = 0;
			Consumable = false;
			Callback = null;
			_extras.Clear();
		}

		/// <summary>
		/// 设置消息的类型
		/// </summary>
		/// <param name="type">类型标志</param>
		public void SetupType(int type) {
			Type = type;
		}
		
		/// <summary>
		/// 设置消息的来源和目的地
		/// </summary>
		/// <param name="source">来源</param>
		/// <param name="target">目的</param>
		public void SetupFromTo(string source, string target) {
			Source = source;
			Target = target;
		}

		/// <summary>
		/// 设置消息的内容标签
		/// </summary>
		/// <param name="content">标签</param>
		public void SetupContent(string content) {
			Content = content;
		}

		/// <summary>
		/// 设置消息是否主动唤醒休眠对象
		/// </summary>
		/// <param name="activate">是否主动唤醒</param>
		public void SetupActivate(bool activate) {
			Activate = activate;
		}

		/// <summary>
		/// 设置消息的优先级，处理时会照此排序
		/// </summary>
		/// <param name="priority">优先级</param>
		public void SetupPriority(int priority) {
			Priority = priority;
		}

		/// <summary>
		/// 设置消息是否可以被消费
		/// </summary>
		/// <param name="consumable">可否消费</param>
		public void SetupConsumable(bool consumable) {
			Consumable = consumable;
		}

		/// <summary>
		/// 设置消息的回调方法，在处理结束时回调
		/// </summary>
		/// <param name="callback">回调方法</param>
		public void SetupCallback(Action<MessageInfo> callback) {
			Callback = callback;
		}
		
		/// <summary>
		/// 消费当前事件，调用一次回调并将该事件放入回收队列
		/// </summary>
		/// <param name="intercept">是否拦截当前事件，如果拦截事件会导致后续处理被截断</param>
		public void Consume(bool intercept = false) {
			Callback?.Invoke(this);
			if (intercept) {
				consumerCount = 0;
			} else {
				consumerCount--;
			}

			if (!Consumable || consumerCount != 0) return;
			consumerCount = -1;
			MessageUtils.RecycleMessage(this);
		}

		/// <summary>
		/// 当前消息是否已经被消费
		/// </summary>
		/// <returns></returns>
		public bool IsConsumed() {
			return Consumable && consumerCount <= 0;
		}
	}
}