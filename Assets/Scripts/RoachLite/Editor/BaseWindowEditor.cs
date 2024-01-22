// File create date:2022/7/9
using System;
using System.Collections.Generic;
using RoachLite.Basic;
using UnityEditor;
using UnityEngine;
// Created By Yu.Liu
namespace RoachLite.EditorPlugin {
	/// <summary>
	/// 窗口基类的编辑器插件
	/// </summary>
	[CustomEditor(typeof(BaseWindow), true)]
	public class BaseWindowEditor : BasePanelEditor {

		protected SerializedProperty isStaticProperty;
		protected SerializedProperty isPopupProperty;
		protected SerializedProperty isDragableProperty;
		protected SerializedProperty dragableAreaProperty;

		protected override void OnInit() {
			base.OnInit();
			isStaticProperty = GetPropertyFromTarget("isStatic");
			isPopupProperty = GetPropertyFromTarget("isPopup");
			isDragableProperty = GetPropertyFromTarget("isDragable");
			dragableAreaProperty = GetPropertyFromTarget("dragableArea");
		}

		protected override void OnCustomInspectorGUI() {
			base.OnCustomInspectorGUI();
			EditorGUILayout.BeginHorizontal();
			isStaticProperty.boolValue =
				EditorGUILayout.Toggle(isStaticProperty.boolValue, GUILayout.Width(14f));
			EditorGUILayout.LabelField("静态窗口", GUILayout.Width(56f));
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			isPopupProperty.boolValue =
				EditorGUILayout.Toggle(isPopupProperty.boolValue, GUILayout.Width(14f));
			EditorGUILayout.LabelField("窗口动画", GUILayout.Width(56f));
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			isDragableProperty.boolValue = 
				EditorGUILayout.Toggle(isDragableProperty.boolValue, GUILayout.Width(14f));
			EditorGUILayout.LabelField("窗口拖拽");
			EditorGUILayout.EndHorizontal();
			if (isDragableProperty.boolValue) {
				EditorGUILayout.PropertyField(dragableAreaProperty);
			}
		}
	}
}