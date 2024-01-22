// File create date:2020/10/24
using RoachLite.UIComponent;
using UnityEditor;
using UnityEngine;
// Created By Yu.Liu
namespace RoachLite.EditorPlugin {
    /// <summary>
    /// 带前后缀文字控件编辑器插件
    /// </summary>
    [CustomEditor(typeof(AffixTextView), true)]
    public class AffixTextViewEditor : ScriptEditor {

        protected SerializedProperty hasPrefixProperty;
        protected SerializedProperty prefixTextProperty;
        protected SerializedProperty hasSuffixProperty;
        protected SerializedProperty suffixTextProperty;

        protected override void OnInit() {
            hasPrefixProperty = GetPropertyFromTarget("hasPrefix");
            prefixTextProperty = GetPropertyFromTarget("prefixText");
            hasSuffixProperty = GetPropertyFromTarget("hasSuffix");
            suffixTextProperty = GetPropertyFromTarget("suffixText");
        }

        protected override void OnCustomInspectorGUI() {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("--- 组件配置 ---");
            DrawPrefixInfo();
            DrawSuffixInfo();
            EditorGUILayout.EndVertical();
        }

        protected void DrawPrefixInfo() {
            EditorGUILayout.BeginHorizontal();
            hasPrefixProperty.boolValue = EditorGUILayout.Toggle(hasPrefixProperty.boolValue, GUILayout.Width(14f));
            EditorGUILayout.LabelField("包含前缀");
            EditorGUILayout.EndHorizontal();
            if (hasPrefixProperty.boolValue) {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("前缀内容：", GUILayout.Width(70f));
                prefixTextProperty.stringValue = EditorGUILayout.TextField(prefixTextProperty.stringValue);
                EditorGUILayout.EndHorizontal();
            }
        }

        protected void DrawSuffixInfo() {
            EditorGUILayout.BeginHorizontal();
            hasSuffixProperty.boolValue = EditorGUILayout.Toggle(hasSuffixProperty.boolValue, GUILayout.Width(14f));
            EditorGUILayout.LabelField("包含后缀");
            EditorGUILayout.EndHorizontal();
            if (hasSuffixProperty.boolValue) {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("后缀内容：", GUILayout.Width(70f));
                suffixTextProperty.stringValue = EditorGUILayout.TextField(suffixTextProperty.stringValue);
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
