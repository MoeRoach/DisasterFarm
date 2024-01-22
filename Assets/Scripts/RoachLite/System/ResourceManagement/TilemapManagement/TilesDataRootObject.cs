// File create date:2021/6/25
using System;
using System.Collections.Generic;
using UnityEngine;
// Created By Yu.Liu
/// <summary>
/// 瓦片数据根对象
/// </summary>
[CreateAssetMenu(fileName = "NewRoot", menuName = "ResManage/Tiles/Root")]
public class TilesDataRootObject : ScriptableObject  {
	
	public List<string> paletteNames = new List<string>();
	public List<TilesPaletteObject> tilesPalettes = new List<TilesPaletteObject>();

	public bool AddTilesPalette(string plName, TilesPaletteObject database) {
		if (database != null) {
			if (!paletteNames.Contains(plName)) {
				tilesPalettes.Add(database);
				paletteNames.Add(plName);
				return true;
			} else {
				Debug.LogWarning($"TILES: Cannot Add Tiles Palette due to the same name [{plName}].");
			}
		}
		return false;
	}
	
	public bool CheckPalette(string plName) {
		return paletteNames.Contains(plName);
	}

	public void RemovePalette(string plName) {
		if (CheckPalette(plName)) {
			var index = paletteNames.IndexOf(plName);
			paletteNames.RemoveAt(index);
			tilesPalettes.RemoveAt(index);
		}
	}

	public TilesPaletteObject GetTilesPalette(string plName) {
		TilesPaletteObject result = null;
		if (CheckPalette(plName)) {
			var index = paletteNames.IndexOf(plName);
			result = tilesPalettes[index];
		}
		return result;
	}
}