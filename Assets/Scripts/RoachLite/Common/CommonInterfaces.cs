// File create date:2022/7/2
using System;
using System.Collections.Generic;
using RoachLite.Services.Message;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RoachLite.Common {
	/// <summary>
	/// 消息处理接口，实现该接口即拥有处理事件的权限
	/// </summary>
	public interface IMessageHandler {
		void EnqueueMessage(MessageInfo e);
	}

	/// <summary>
	/// 事件接收器接口，实现该接口表示可接受事件
	/// </summary>
	public interface IMessageReceiver {
		void ReceiveMessage(MessageInfo e);
	}
	
	/// <summary>
	/// 可拖拽组件接口，提供拖拽回调方法
	/// </summary>
	public interface IDragable {
		void OnDragBegin(PointerEventData pointer);
		void OnDragUpdate(PointerEventData pointer);
		void OnDragFinish(PointerEventData pointer);
	}
	
	/// <summary>
	/// 界面刷新接口，提供同步和隔帧刷新方法
	/// </summary>
	public interface IViewUpdater {
		void UpdateViews();
		void NotifyUpdate(bool activate = false);
	}
	
	/// <summary>
	/// 列表数据适配器接口，提供适配器所需的基础方法集
	/// </summary>
	public interface IAdapter {
		void SetupUpdateReference(IViewUpdater v);
		int GetCount(); // 获取列表项数目
		int GetItemId(int index); // 获取项目ID
		object GetItem(int index); // 获取项目数据
		GameObject GetObject(GameObject prev, int index); // 生成项目对象
	}
	
	/// <summary>
	/// 工厂类接口
	/// </summary>
	/// <typeparam name="TObject">对象类型</typeparam>
	public interface IFactory<out TObject> {
		/// <summary>
		/// 创建对象
		/// </summary>
		/// <returns>对象实体</returns>
		TObject Create();
	}

	/// <summary>
	/// 带标签的工厂类接口
	/// </summary>
	/// <typeparam name="TObject">对象类型</typeparam>
	public interface ITagFactory<out TObject> {
		/// <summary>
		/// 根据标签创建对象
		/// </summary>
		/// <param name="t">标签</param>
		/// <returns>对象实体</returns>
		TObject Create(string t);
	}
}