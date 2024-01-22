// File create date:1/27/2019
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

// Created By Yu.Liu
namespace RoachLite.Services.Broadcast {
    /// <summary>
    /// 广播服务，提供基础的广播注册和发送方法
    /// </summary>
    public class BroadcastService : IGameService {

        public const string SERVICE_NAME = "BroadcastService";

        private readonly Dictionary<string, HashSet<Action<BroadcastInfo>>> _receivers;

        public BroadcastService() {
            _receivers = new Dictionary<string, HashSet<Action<BroadcastInfo>>>();
        }

        public void InitService() {
            Debug.Log("Broadcast Service Initiated!");
            MessageBroker.Default.Receive<BroadcastInfo>().Subscribe(ReceiveInformation);
        }

        public void KillService() {
            Debug.Log("Broadcast Service Destoryed!");
        }

        public void BroadcastInformation(BroadcastInfo msg) {
            MessageBroker.Default.Publish(msg);
        }

        private void ReceiveInformation(BroadcastInfo msg) {
            if (!_receivers.ContainsKey(msg.Action)) return;
            var actionSet = _receivers[msg.Action];
            foreach (var action in actionSet) {
                action(msg);
            }
        }

        public void RegisterBroadcastReceiver(BroadcastFilter filter, Action<BroadcastInfo> action) {
            foreach (var filterStr in filter) {
                if (_receivers.ContainsKey(filterStr)) {
                    _receivers[filterStr].Add(action);
                } else {
                    var actionSet = new HashSet<Action<BroadcastInfo>> {action};
                    _receivers[filterStr] = actionSet;
                }
            }
        }

        public void UnregisterBroadcastReceiver(Action<BroadcastInfo> action) {
            var filters = new BroadcastFilter();
            foreach (var filter in _receivers.Keys) {
                var actionSet = _receivers[filter];
                if (actionSet.Contains(action)) {
                    filters.AddFilter(filter);
                }
            }
            foreach (var filter in filters) {
                _receivers[filter].Remove(action);
            }
        }
    }
}
