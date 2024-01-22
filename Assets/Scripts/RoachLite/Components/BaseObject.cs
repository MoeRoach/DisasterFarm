using System;
using System.Collections;
using System.Collections.Generic;
using RoachLite;
using RoachLite.Common;
using RoachLite.Services;
using RoachLite.Services.Broadcast;
using RoachLite.Services.Message;
using UniRx;
using UnityEngine;

namespace RoachLite.Basic {
    /// <summary>
    /// 游戏对象基类
    /// </summary>
    public abstract class BaseObject : MonoBehaviour, IMessageHandler {

        protected ObjectIdentifier identifier;
        protected bool exceptionFlag = false;
        protected SortedDictionary<int, List<Action>> updateFunctionSet;
        protected readonly List<Action<GameObject>> destroyFunctionSet = new List<Action<GameObject>>();
        protected MessageService messageService;
        protected Queue<MessageInfo> messageQueue;
        protected int messageProcessRate = 20;
        protected BroadcastService broadcastService;
        protected HashSet<Action<BroadcastInfo>> receiverSet;
        protected SimpleExecNext lazyLoadFunc;

        private void Awake() {
            UniverseController.Initialize();
            try {
                messageService = ServiceProvider.Instance.ProvideService<MessageService>(
                    MessageService.SERVICE_NAME);
                broadcastService = ServiceProvider.Instance.ProvideService<BroadcastService>(
                    BroadcastService.SERVICE_NAME);
                updateFunctionSet = new SortedDictionary<int, List<Action>>();
                messageQueue = new Queue<MessageInfo>();
                receiverSet = new HashSet<Action<BroadcastInfo>>();
                identifier = GetComponent<ObjectIdentifier>();
                lazyLoadFunc = new SimpleExecNext(OnLazyLoad);
                OnAwake();
            } catch (Exception e) {
                Debug.LogError($"Exception Occur In {name} Awake!");
                Debug.LogError($"{e.Message}\n{e.StackTrace}");
            }
        }

        protected virtual void OnAwake() { }

        private void Start() {
            UniverseController.Setup();
            try {
                OnStart();
                StartCoroutine(lazyLoadFunc);
            } catch (Exception e) {
                Debug.LogError($"Exception Occur In {name} Start!");
                Debug.LogError($"{e.Message}\n{e.StackTrace}");
            }
        }

        protected virtual void OnStart() { }
        
        protected virtual void OnLazyLoad() { }

        private void Update() {
            try {
                if (exceptionFlag) return;
                OnUpdate();
                ProcessMessage();
                CallFunctions();
            } catch (Exception e) {
                Debug.LogError($"Exception Occur In {name} Update!");
                Debug.LogError($"{e.Message}\n{e.StackTrace}");
                exceptionFlag = true;
            }
        }
        
        protected virtual void OnUpdate() { }
        
        /// <summary>
        /// 调用所有注册的更新方法
        /// </summary>
        private void CallFunctions() {
            if (updateFunctionSet.Count <= 0) return;
            foreach (var pair in updateFunctionSet) {
                if (pair.Value == null || pair.Value.Count <= 0) continue;
                for (var i = 0; i < pair.Value.Count; i++) {
                    pair.Value[i].Invoke();
                }
            }
        }

        private void ProcessMessage() {
            if (identifier == null || !identifier.receiveMessage) return;
            if (messageQueue.Count <= 0) return;
            for (var i = 0; i < messageProcessRate; i++) {
                if (messageQueue.Count <= 0) break;
                var msg = messageQueue.Dequeue();
                if (msg == null || msg.IsConsumed()) continue;
                HandleMessage(msg);
                msg.Consume();
            }
        }
        
        protected virtual void HandleMessage(MessageInfo msg) { }

        private void LateUpdate() {
            try {
                if (exceptionFlag) return;
                OnLateUpdate();
            } catch (Exception e) {
                Debug.LogError($"Exception Occur In {name} LateUpdate!");
                Debug.LogError($"{e.Message}\n{e.StackTrace}");
                exceptionFlag = true;
            }
        }
        
        protected virtual void OnLateUpdate() { }
        
        /// <summary>
        /// 接收消息并放入队列
        /// </summary>
        /// <param name="e">消息内容</param>
        public void EnqueueMessage(MessageInfo e) {
            messageQueue?.Enqueue(e);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg">消息内容</param>
        protected void SubmitMessage(MessageInfo msg) {
            messageService?.SubmitMessage(msg);
        }
        
        /// <summary>
        /// 下一帧执行方法
        /// </summary>
        /// <param name="action">方法委托</param>
        protected void ExecuteNext(Action action) {
            StartCoroutine(new SimpleExecNext(action));
        }
        
        /// <summary>
        /// 下一帧执行方法，UniRx全局代理，不受游戏对象激活状态影响
        /// </summary>
        /// <param name="action">方法委托</param>
        protected void ExecuteNextGlobal(Action action) {
            Observable.NextFrame().Subscribe(_ => {
                action.Invoke();
            });
        }
        
        /// <summary>
        /// 等待一定时间后执行方法
        /// </summary>
        /// <param name="time">等待时间，单位为秒</param>
        /// <param name="action">方法委托</param>
        protected Coroutine ExecuteDelay(float time, Action action) {
            return StartCoroutine(new SimpleExecDelay(action, time));
        }

        /// <summary>
        /// 等待一定时间后执行方法，UniRx全局代理，不受游戏对象激活状态影响
        /// </summary>
        /// <param name="time">等待时间，单位为秒</param>
        /// <param name="action">方法委托</param>
        protected IDisposable ExecuteDelayGlobal(float time, Action action) {
            return Observable.Return(true, Scheduler.MainThread)
                .Delay(TimeSpan.FromSeconds(time)).Subscribe(_ => {
                    action.Invoke();
                });
        }

        /// <summary>
        /// 注册帧更新方法到集合
        /// </summary>
        /// <param name="priority">方法优先级，数字越小越早运行</param>
        /// <param name="func">更新方法</param>
        protected void RegisterUpdateFunction(int priority, Action func) {
            List<Action> functions;
            if (updateFunctionSet.ContainsKey(priority)) {
                functions = updateFunctionSet[priority];
            } else {
                functions = new List<Action>();
            }
            functions.Add(func);
            updateFunctionSet[priority] = functions;
        }

        /// <summary>
        /// 反注册帧更新方法
        /// </summary>
        /// <param name="func">更新方法</param>
        protected void UnregisterUpdateFunction(Action func) {
            var priorityList = new List<int>(updateFunctionSet.Keys);
            foreach (var p in priorityList) {
                updateFunctionSet[p].Remove(func);
            }
        }
        
        /// <summary>
        /// 注册一个对象销毁回调方法
        /// </summary>
        /// <param name="callback">方法委托</param>
        public void RegisterDestroyCallback(Action<GameObject> callback) {
            destroyFunctionSet.Add(callback);
        }

        /// <summary>
        /// 根据筛选器注册一个广播接收器
        /// </summary>
        /// <param name="filter">筛选器</param>
        /// <param name="receiver">接收器</param>
        protected virtual void RegisterBroadcastReceiver(BroadcastFilter filter,
            Action<BroadcastInfo> receiver) {
            if (broadcastService == null) return;
            broadcastService.RegisterBroadcastReceiver(filter, receiver);
            receiverSet.Add(receiver);
        }

        /// <summary>
        /// 通过完整路径寻找子物体组件的快捷方法
        /// 注意路径即可以使用/分隔符，也可以使用.分隔符
        /// </summary>
        /// <typeparam name="T">目标组件类型</typeparam>
        /// <param name="path">完整路径</param>
        /// <returns>目标组件</returns>
        protected T FindComponent<T>(string path) where T : Component {
            path = path.Replace('.', '/');
            return gameObject.FindComponent<T>(path);
        }

        /// <summary>
        /// 通过完整路径寻找子物体的快捷方法
        /// 注意路径即可以使用/分隔符，也可以使用.分隔符
        /// </summary>
        /// <param name="path">完整路径</param>
        /// <returns>目标物体对象</returns>
        protected GameObject FindGameObject(string path) {
            path = path.Replace('.', '/');
            return gameObject.FindObject(path);
        }

        /// <summary>
        /// 设置目标对象的激活状态
        /// </summary>
        /// <param name="active">是否激活</param>
        public void SetActive(bool active) {
            gameObject.SetActive(active);
        }

        /// <summary>
        /// 摧毁对象时释放资源
        /// </summary>
        protected virtual void Release() { }

        private void OnDestroy() {
            Release();
            messageQueue.Clear();
            if (broadcastService != null) {
                foreach (var receiver in receiverSet) {
                    broadcastService.UnregisterBroadcastReceiver(receiver);
                }
            }
            foreach (var act in destroyFunctionSet) {
                act.Invoke(gameObject);
            }
        }
        
        
    }
}

