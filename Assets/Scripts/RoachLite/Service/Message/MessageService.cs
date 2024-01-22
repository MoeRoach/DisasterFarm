// File create date:2022/7/2
using System;
using System.Collections.Generic;
using RoachLite.Common;
using UnityEngine;

namespace RoachLite.Services.Message {
	/// <summary>
	/// 消息服务，提供基础的消息处理器注册和发送功能
	/// </summary>
	public class MessageService : IGameService {
		
		public const string SERVICE_NAME = "MessageService";
		private readonly Dictionary<string, IMessageReceiver> _receivers;

		public MessageService() {
			_receivers = new Dictionary<string, IMessageReceiver>();
		}

		public void InitService() {
			Debug.Log("Message Service Initiated!");
		}

		public void KillService() {
			Debug.Log("Message Service Destoryed!");
		}

		/// <summary>
		/// 发送消息
		/// </summary>
		/// <param name="msg">消息实例</param>
		/// <exception cref="NullReferenceException">发送消息为空时抛出异常</exception>
		public void SubmitMessage(MessageInfo msg) {
			if (msg == null) {
				throw new NullReferenceException("Cannot Send Null Message!");
			}
			if (_receivers.ContainsKey(msg.Target)) {
				_receivers[msg.Target].ReceiveMessage(msg);
				return;
			}
			Debug.LogWarning($"Cannot Find Message Target: {msg.Target}");
		}

		/// <summary>
		/// 注册消息接收器
		/// </summary>
		/// <param name="id">参照ID</param>
		/// <param name="receiver">接收器</param>
		public void RegisterReceiver(string id, IMessageReceiver receiver) {
			if (_receivers.ContainsKey(id)) {
				Debug.LogWarning($"Duplicate Receiver Found: {id}");
			}
			_receivers[id] = receiver;
		}

		/// <summary>
		/// 反注册接收器
		/// </summary>
		/// <param name="id">参照ID</param>
		public void UnregisterReceiver(string id) {
			_receivers.Remove(id);
		}
	}
}
