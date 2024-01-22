// File create date:2022/7/5
// Created By Yu.Liu
namespace RoachLite.Basic {
	/// <summary>
	/// 通用脚本泛型单例基类
	/// </summary>
	/// <typeparam name="T">单例类型</typeparam>
	public abstract class MonoSingleton<T> : BaseObject where T : MonoSingleton<T> {
		public static T Instance { get; protected set; }
		public bool canBeDestroy;

		protected override void OnAwake() {
			if (Instance != null) {
				Destroy(gameObject);
				return;
			}
			Instance = this as T;
			if (!canBeDestroy) {
				DontDestroyOnLoad(gameObject);
			}
			base.OnAwake();
		}

		protected override void Release() {
			base.Release();
			if (Instance == this) {
				Instance = null;
			}
		}
	}
}