// File create date:2021/6/25
using System;
using System.Collections.Generic;
using RoachLite.TilemapManagement;
using UnityEngine;
// Created By Yu.Liu
/// <summary>
/// 瓦片调色盘对象
/// </summary>
[CreateAssetMenu(fileName = "NewPalette", menuName = "ResManage/Tiles/Palette")]
public class TilesPaletteObject : ScriptableObject  {
	
	public string paletteName;
	public List<string> groupNames = new List<string>();
	public List<TilesGroupData> tilesGroups = new List<TilesGroupData>();
	// Editor编辑界面所需数据
	public bool isExpanded = false;
	
	public bool AddTilesGroup(string grpName, TilesGroupData groupData) {
		if (groupData != null) {
			if (!groupNames.Contains(grpName)) {
				tilesGroups.Add(groupData);
				groupNames.Add(grpName);
				return true;
			} else {
				Debug.LogWarning($"TILES: Cannot Add Tiles Group Data due to the same name [{grpName}].");
			}
		}
		return false;
	}

	public bool CheckGroupData(string grpName) {
		return groupNames.Contains(grpName);
	}

	public void RemoveGroupData(string grpName) {
		if (CheckGroupData(grpName)) {
			var index = groupNames.IndexOf(grpName);
			groupNames.RemoveAt(index);
			tilesGroups.RemoveAt(index);
		}
	}

	public TilesGroupData GetTilesGroupData(string grpName) {
		TilesGroupData result = null;
		if (CheckGroupData(grpName)) {
			var index = groupNames.IndexOf(grpName);
			result = tilesGroups[index];
		}
		return result;
	}
}