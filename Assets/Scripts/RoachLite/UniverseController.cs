using RoachLite.Common;
using RoachLite.PrefabManagement;
using RoachLite.Services;
using RoachLite.Services.Broadcast;
using RoachLite.Services.Message;
using RoachLite.SpriteManagement;
using RoachLite.TilemapManagement;
using UnityEngine;

namespace RoachLite {
    /// <summary>
    /// 全局控制器，单例模式，静态初始化
    /// </summary>
    public class UniverseController {

        private static bool isInitialized;
        private static bool isSetup;

        public static UniverseController Instance { get; } = new UniverseController();

        private UniverseController() { }

        public static void Initialize() {
            if (isInitialized) return;
            // 生成所有的永久对象以及后台协程
            // 初始化所有游戏服务
            ServiceProvider.Instance.RegisterService(MessageService.SERVICE_NAME,
                new MessageService());
            ServiceProvider.Instance.RegisterService(BroadcastService.SERVICE_NAME,
                new BroadcastService());
            // 初始化所有单例对象
            ObjectManager.Instance.Initialize();
            isInitialized = true;
            // 任何涉及到BaseObject子类脚本的对象初始化都必须在更新标志后进行，避免无限递归
            PrefabManager.Instance.LoadPrefabData();
            SpriteManager.Instance.LoadSpriteData();
            TilesManager.Instance.LoadTilesData();
            FarmDataService.Instance.Initialize();
        }

        public static void Setup() {
            if (isSetup) return;
            // 进行所有全局设置，即全局生效且只进行一次的配置
            isSetup = true;
        }
    }
}
