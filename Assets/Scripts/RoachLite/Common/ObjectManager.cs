// File create date:2022/7/2
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RoachLite.Common {
	/// <summary>
	/// 对象管理器
	/// </summary>
	public class ObjectManager : Singleton<ObjectManager> {

		private Dictionary<string, GameObject> _objects;
		
		public override void Initialize() {
			if (isInitialized) return;
			_objects = new Dictionary<string, GameObject>();
			isInitialized = true;
		}

		public void RegisterObject(string id, GameObject obj) {
			if (_objects.ContainsKey(id)) {
				Debug.LogWarning($"Duplicate Identifier Detected: {id}");
				return;
			}

			_objects[id] = obj;
		}

		public void UnregisterObject(string id) {
			_objects.Remove(id);
		}

		public GameObject FindGameObject(string id) {
			return _objects.TryGetElement(id);
		}

		public GameObject FindGameObject(string oid, string domain) {
			return FindGameObject(CompileIdentifier(oid, domain));
		}

		public TComponent FindComponent<TComponent>(string id) 
			where TComponent : Component {
			var obj = _objects.TryGetElement(id);
			return obj != null ? obj.GetComponent<TComponent>() : null;
		}

		public TComponent FindComponent<TComponent>(string oid, string domain)
			where TComponent : Component {
			return FindComponent<TComponent>(CompileIdentifier(oid, domain));
		}

		public static string CompileIdentifier(string oid, string domain) {
			return $"{oid}@{domain}";
		}
	}
}
