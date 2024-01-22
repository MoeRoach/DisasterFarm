using System;
using System.Collections;
using System.Collections.Generic;
using RoachLite;
using RoachLite.Common;
using RoachLite.Services;
using RoachLite.Services.Message;
using UnityEngine;

namespace RoachLite.Basic {
    /// <summary>
    /// 对象标识
    /// </summary>
    public sealed class ObjectIdentifier : MonoBehaviour, IMessageReceiver {

        [Tooltip("组件唯一标识，如果置空则与对象名称相同")] public string identifier = string.Empty;
        [Tooltip("组件作用域，可区分不同域下的同一ID")] public string domain = "Runtime";
        [Tooltip("是否接收消息")] public bool receiveMessage;
        public string Identifier { get; private set; }

        private MessageService _messageService;

        public void ReceiveMessage(MessageInfo msg) {
            gameObject.SetActive(gameObject.activeSelf || msg.Activate);
            var handlers = gameObject.GetComponents<IMessageHandler>();
            if (handlers == null || handlers.Length <= 0) return;
            if (msg.Consumable) msg.consumerCount = handlers.Length;
            foreach (var handler in handlers) {
                handler.EnqueueMessage(msg);
            }
        }

        private void Awake() {
            UniverseController.Initialize();
            InitIdentity();
        }

        private void Start() {
            _messageService = ServiceProvider.Instance.ProvideService<MessageService>(
                MessageService.SERVICE_NAME);
            if (!receiveMessage) return;
            _messageService.RegisterReceiver(Identifier, this);
        }

        private void InitIdentity() {
            if (string.IsNullOrEmpty(identifier)) identifier = name;
            Identifier = ObjectManager.CompileIdentifier(identifier, domain);
            ObjectManager.Instance.RegisterObject(Identifier, gameObject);
        }

        private void OnDestroy() {
            if (receiveMessage) _messageService?.UnregisterReceiver(Identifier);
            ObjectManager.Instance.UnregisterObject(Identifier);
        }
    }
}
