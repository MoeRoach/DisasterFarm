// File create date:2022/7/9
using System;
using System.Collections.Generic;
using RoachLite.Basic;
using UnityEditor;
using UnityEngine;
// Created By Yu.Liu
namespace RoachLite.EditorPlugin {
	/// <summary>
	/// 基础面板组件自定义编辑器
	/// </summary>
	[CustomEditor(typeof(BasePanel), true)]
	public class BasePanelEditor : ScriptEditor {

		protected SerializedProperty startShowProperty;
		protected SerializedProperty hideRootObjectProperty;
		protected SerializedProperty hideRaycastProperty;
		
		protected override void OnInit() {
			startShowProperty = GetPropertyFromTarget("startShow");
			hideRootObjectProperty = GetPropertyFromTarget("hideRootObject");
			hideRaycastProperty = GetPropertyFromTarget("hideRaycast");
		}

		protected override void OnCustomInspectorGUI() {
			EditorGUILayout.BeginHorizontal();
			startShowProperty.boolValue =
				EditorGUILayout.Toggle(startShowProperty.boolValue, GUILayout.Width(14f));
			EditorGUILayout.LabelField("默认显示", GUILayout.Width(54f));
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			hideRootObjectProperty.boolValue =
				EditorGUILayout.Toggle(hideRootObjectProperty.boolValue, GUILayout.Width(14f));
			EditorGUILayout.LabelField("物体激活状态跟随显示", GUILayout.Width(128f));
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			hideRaycastProperty.boolValue =
				EditorGUILayout.Toggle(hideRaycastProperty.boolValue, GUILayout.Width(14f));
			EditorGUILayout.LabelField("射线遮挡状态跟随显示", GUILayout.Width(128f));
			EditorGUILayout.EndHorizontal();
		}
	}
}