// File create date:2022/7/2
using System;

namespace RoachLite.Common {
	/// <summary>
	/// 通用泛型单例抽象基类
	/// </summary>
	/// <typeparam name="T">单例类型</typeparam>
	public abstract class Singleton<T> where T : Singleton<T> {

		private static T instance;

		public static T Instance =>
			instance ??= (T) Activator.CreateInstance(typeof(T), true);

		protected bool isInitialized = false;

		public abstract void Initialize();

	}
}
