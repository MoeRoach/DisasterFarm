// File create date:2022/7/5
using System;
using System.Collections.Generic;
using UnityEngine;
// Created By Yu.Liu
namespace RoachLite.InputManagement {
	/// <summary>
	/// 虚拟输入组件接口
	/// </summary>
	public interface IVirtualInput {
		void SetupActive(bool active);
		object ReadValue();
		void Reset();
	}

	/// <summary>
	/// 基础虚拟输入组件的抽象基类
	/// </summary>
	/// <typeparam name="TKey">输入索引类型</typeparam>
	/// <typeparam name="TValue">输入值类型</typeparam>
	public abstract class BaseInput<TKey, TValue> : IVirtualInput {

		public TKey Key { get; protected set; }
		public TValue Value { get; set; }
		public bool Active { get; protected set; }
		
		public BaseInput() {
			Active = true;
		 }

		public BaseInput(TKey key) {
			Key = key;
			Active = true;
		}

		public void SetupActive(bool active) {
			Active = active;
		}

		public object ReadValue() {
			return Value;
		}

		public virtual void Reset() {
			Value = default;
		}
	}

	public class AxisInput : BaseInput<string, float> {
		public AxisInput() : base() { }
		public AxisInput(string key) : base(key) { }
	}
	
	public class ButtonInput : BaseInput<string, bool> {
		public ButtonInput() : base() { }
		public ButtonInput(string key) : base(key) { }
	}

	public class Vector2Input : BaseInput<string, Vector2> {
		public Vector2Input() : base() { }
		public Vector2Input(string key) : base(key) { }
	}

	public class Vector3Input : BaseInput<string, Vector3> {
		public Vector3Input() : base() { }
		public Vector3Input(string key) : base(key) { }
	}
}
