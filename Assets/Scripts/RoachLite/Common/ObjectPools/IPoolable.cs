// File create date:2022/6/20
using System;
using System.Collections.Generic;
using UnityEngine;
// Created By Yu.Liu
namespace RoachLite.Common {
	/// <summary>
	/// 可入池对象接口
	/// </summary>
	public interface IPoolable {
		/// <summary>
		/// 对象是否已经回收
		/// </summary>
		bool IsRecycled { get; set; }

		/// <summary>
		/// 回收对象时的处理方法
		/// </summary>
		void Recycle();
	}
	
	/// <summary>
	/// 可入池标签对象接口
	/// </summary>
	public interface ITagPoolable : IPoolable {
		/// <summary>
		/// 获取对象Tag
		/// </summary>
		/// <returns>对象Tag</returns>
		string GetTag();
	}

	/// <summary>
	/// 对象池接口
	/// </summary>
	/// <typeparam name="T">对象类型</typeparam>
	public interface IPool<T> {
		/// <summary>
		/// 分配对象
		/// </summary>
		/// <returns></returns>
		T Allocate();

		/// <summary>
		/// 回收对象
		/// </summary>
		/// <param name="obj">待回收对象实例</param>
		/// <returns>回收结果</returns>
		bool Recycle(T obj);
	}
	
	/// <summary>
	/// 标签对象池接口
	/// </summary>
	/// <typeparam name="T">对象类型</typeparam>
	public interface ITagPool<T> {
		/// <summary>
		/// 根据标签分配对象
		/// </summary>
		/// <param name="t">对象标签</param>
		/// <returns>对象实例</returns>
		T Allocate(string t);

		/// <summary>
		/// 回收对象
		/// </summary>
		/// <param name="obj">待回收对象实例</param>
		/// <returns>回收结果</returns>
		bool Recycle(T obj);
	}
}
