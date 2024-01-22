// File create date:2020/10/24

using RoachLite.UIComponent;
using UnityEditor;
using UnityEngine;
// Created By Yu.Liu
namespace RoachLite.EditorPlugin {
    /// <summary>
    /// 勾选框组编辑器插件
    /// </summary>
    [CustomEditor(typeof(CheckGroup), true)]
    public class CheckGroupEditor : ScriptEditor {

        protected SerializedProperty singleSelectProperty;
        protected SerializedProperty canSwitchOffProperty;
        protected SerializedProperty isExtraTogglesProperty;
        protected SerializedProperty toggleListProperty;

        protected override void OnInit() {
            singleSelectProperty = GetPropertyFromTarget("singleSelect");
            canSwitchOffProperty = GetPropertyFromTarget("canSwitchOff");
            isExtraTogglesProperty = GetPropertyFromTarget("isExtraToggles");
            toggleListProperty = GetPropertyFromTarget("toggleList");
        }

        protected override void OnCustomInspectorGUI() {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("--- 选项组配置 ---");
            DrawBasicInfo();
            DrawToggleList();
            EditorGUILayout.EndVertical();
        }

        protected void DrawBasicInfo() {
            EditorGUILayout.BeginHorizontal();
            singleSelectProperty.boolValue = EditorGUILayout.Toggle(singleSelectProperty.boolValue, GUILayout.Width(14f));
            EditorGUILayout.LabelField("单项选择", GUILayout.Width(56f));
            EditorGUILayout.Space();
            canSwitchOffProperty.boolValue = EditorGUILayout.Toggle(canSwitchOffProperty.boolValue, GUILayout.Width(14f));
            EditorGUILayout.LabelField("可取消选择", GUILayout.Width(70f));
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
        }

        protected void DrawToggleList() {
            EditorGUILayout.BeginHorizontal();
            isExtraTogglesProperty.boolValue = EditorGUILayout.Toggle(isExtraTogglesProperty.boolValue, GUILayout.Width(14f));
            EditorGUILayout.LabelField("外部配置选择框");
            EditorGUILayout.EndHorizontal();
            if (isExtraTogglesProperty.boolValue) {
                EditorGUILayout.PropertyField(toggleListProperty);
            }
        }
    }
}
