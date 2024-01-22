using System;
using System.Collections.Generic;
using RoachLite.Services.Message;
using UnityEngine;

namespace RoachLite.Utils {
	/// <summary>
	/// 消息辅助方法
	/// </summary>
	public static class MessageUtils {
		
		private static readonly Queue<MessageInfo> MessageQueue = new Queue<MessageInfo>();

		public static MessageInfo ObtainMessage(int type) {
			MessageInfo msg;
			if (MessageQueue.Count > 0) {
				msg = MessageQueue.Dequeue();
				msg.ResetMessage();
				msg.SetupType(type);
			} else {
				msg = new MessageInfo(type);
			}

			return msg;
		}

		public static void RecycleMessage(MessageInfo msg) {
			MessageQueue.Enqueue(msg);
		}

		public static MessageBuilder GetMessageBuilder(int type) {
			return new MessageBuilder(type);
		}
	}

	public class MessageBuilder {

		private MessageInfo _message;

		public MessageBuilder(int type) {
			_message = MessageUtils.ObtainMessage(type);
		}

		public MessageBuilder SetupContent(string content) {
			_message.SetupContent(content);
			return this;
		}

		public MessageBuilder SetupActivate(bool activate) {
			_message.SetupActivate(activate);
			return this;
		}

		public MessageBuilder SetupFromTo(string target, string source = "") {
			_message.SetupFromTo(source, target);
			return this;
		}

		public MessageBuilder SetupConsumable(bool consumable) {
			_message.SetupConsumable(consumable);
			return this;
		}

		public MessageBuilder SetupPriority(int priority) {
			_message.SetupPriority(priority);
			return this;
		}

		public MessageBuilder SetupCallback(Action<MessageInfo> callback) {
			_message.SetupCallback(callback);
			return this;
		}

		public MessageBuilder PutExtra(string key, object value) {
			_message.PutExtra(key, value);
			return this;
		}

		public MessageInfo Build() {
			return _message;
		}
	}
}
