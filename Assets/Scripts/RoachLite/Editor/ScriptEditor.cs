// File create date:12/2/2018
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
// Created By Yu.Liu
namespace RoachLite.EditorPlugin {
    /// <summary>
    /// 脚本Inspector界面插件基类，默认绘制了脚本对象框
    /// </summary>
    public abstract class ScriptEditor : Editor {

        protected SerializedObject serializedTarget;
        protected MonoScript monoScript;
        protected HashSet<string> propertyRecords;
        protected List<SerializedProperty> unhandledProperties;
        protected bool showUnhandledProps = false;

        private void OnEnable() {
            serializedTarget = new SerializedObject(target);
            propertyRecords = new HashSet<string>();
            unhandledProperties = new List<SerializedProperty>();
            monoScript = MonoScript.FromMonoBehaviour(target as MonoBehaviour);
            InitPropertyRecord();
            OnInit();
            ProcessUnhandledProps();
        }

        protected void InitPropertyRecord() {
            if (propertyRecords.Count == 0) {
                FieldInfo[] fields = target.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
                foreach (FieldInfo fi in fields) {
                    propertyRecords.Add(fi.Name);
                }
                fields = typeof(MonoBehaviour).GetFields(BindingFlags.Instance | BindingFlags.Public);
                foreach (FieldInfo fi in fields) {
                    propertyRecords.Remove(fi.Name);
                }
            }
        }

        /// <summary>
        /// 编辑器脚本初始化
        /// </summary>
        protected abstract void OnInit();

        protected void ProcessUnhandledProps() {
            if (unhandledProperties.Count == 0) {
                foreach (string name in propertyRecords) {
                    SerializedProperty prop = serializedTarget.FindProperty(name);
                    if (prop != null) {
                        unhandledProperties.Add(prop);
                    }
                }
            }
        }

        /// <summary>
        /// 寻找属性字段的封装方法，对已经自定义处理过的字段进行记录
        /// </summary>
        /// <param name="name">字段名</param>
        /// <returns>序列化属性</returns>
        protected SerializedProperty GetPropertyFromTarget(string name) {
            propertyRecords.Remove(name);
            return serializedTarget.FindProperty(name);
        }

        /// <summary>
        /// 绘制默认不可修改的脚本对象框
        /// </summary>
        protected void DrawMonoScript() {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Script", monoScript, typeof(MonoScript), false);
            EditorGUI.EndDisabledGroup();
        }

        public override void OnInspectorGUI() {
            serializedTarget.Update();
            DrawMonoScript();
            OnCustomInspectorGUI();
            DrawUnhandledInspectorGUI();
            serializedTarget.ApplyModifiedProperties();
        }

        /// <summary>
        /// 自定义Inspector界面绘制
        /// </summary>
        protected virtual void OnCustomInspectorGUI() { }

        protected void DrawUnhandledInspectorGUI() {
            if (unhandledProperties.Count > 0) {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("--- 其他配置 ---");
                for (int i = 0; i < unhandledProperties.Count; i++) {
                    EditorGUILayout.PropertyField(unhandledProperties[i], true);
                }
                EditorGUILayout.EndVertical();
            }
        }
    }
}
