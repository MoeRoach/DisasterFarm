// File create date:2022/7/5
using System;
using System.Collections.Generic;
using UnityEngine;
// Created By Yu.Liu
namespace RoachLite.Configs {
	/// <summary>
	/// 通用配置项
	/// </summary>
	public static class CommonConfigs {
		
		public const int MSG_TYPE_COMMON_UI_FUNCTION = 0x0000; // 通用界面功能事件
		public const int MSG_TYPE_COMMON_CALLBACK = 0x0001; // 通用回调事件
		
		public const string MSG_CONTENT_WINDOW_OPEN = "WindowOpen";
		public const string MSG_CONTENT_CALLBACK_WINDOW = "CallbackWindow";
		
		public const string BROADCAST_FILTER_VIEW_SHOW = "ViewShow";
		public const string BROADCAST_FILTER_VIEW_HIDE = "ViewHide";
		public const string BROADCAST_FILTER_WINDOW_OPEN = "WindowOpen";
		public const string BROADCAST_FILTER_WINDOW_CLOSE = "WindowClose";
		public const string BROADCAST_FILTER_SCENE_LOAD = "SceneLoad";

		public const string BROADCAST_CONTENT_VIEW_BEHAVIOR_START = "ViewBehaviorStart";
		public const string BROADCAST_CONTENT_VIEW_BEHAVIOR_FINISH = "ViewBehaviorFinish";
		public const string BROADCAST_CONTENT_SWITCH_SCENE = "SwitchScene";
		public const string BROADCAST_CONTENT_APPEND_SCENE = "AppendScene";
		public const string BROADCAST_CONTENT_REMOVE_SCENE = "RemoveScene";

		public const string EXTRA_TAG_WINDOW_POSITION = "WindowPosition";
		public const string EXTRA_TAG_WINDOW_IDENTIFIER = "WindowIdentifier";
		public const string EXTRA_TAG_WINDOW_DOMAIN = "WindowDomain";
		public const string EXTRA_TAG_CALLBACK_TARGET = "CallbackTarget";
		public const string EXTRA_TAG_OBJECT_IDENTIFIER = "ObjectIdentifier";
		public const string EXTRA_TAG_OBJECT_DOMAIN = "ObjectDomain";
		public const string EXTRA_TAG_SCENE_NAME = "SceneName";

		public const string TARGET_SCENE_QUIT = "EXIT_GAME";

		public const string TAG_NULL_OBJ = "NULL_OBJECT";
		public const string TAG_SYSTEM_OBJ = "GameSystem";
	}
}