// File create date:2024/1/26
using System;
using System.Collections.Generic;
using RoachLite.Data;
using RoachLite.TilemapManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

// Created By Yu.Liu
public static class MapUtils {

	public static Square WorldToSquare(Vector3 pos) {
		var x = Mathf.FloorToInt(pos.x);
		var y = Mathf.FloorToInt(pos.y);
		return new Square(x, y);
	}

	public static Vector3 SquareToWorld(Square sq) {
		return new Vector3(sq.x + 0.5f, sq.y + 0.5f, 0f);
	}

	public static Vector3Int SquareToCoordinate(Square sq) {
		return SquareToCoordinate(sq.x, sq.y);
	}
	
	public static Vector3Int SquareToCoordinate(int x, int y) {
		return new Vector3Int(x, y, 0);
	}

	public const int FENCE_INDEX_LEFT_TOP = 0;
	public const int FENCE_INDEX_TOP = 1;
	public const int FENCE_INDEX_RIGHT_TOP = 2;
	public const int FENCE_INDEX_LEFT = 3;
	public const int FENCE_INDEX_RIGHT = 4;
	public const int FENCE_INDEX_LEFT_BOT = 5;
	public const int FENCE_INDEX_BOT = 6;
	public const int FENCE_INDEX_RIGHT_BOT = 7;

	public const int GROUND_INDEX_LEFT_TOP = 0;
	public const int GROUND_INDEX_TOP = 1;
	public const int GROUND_INDEX_RIGHT_TOP = 2;
	public const int GROUND_INDEX_LEFT = 3;
	public const int GROUND_INDEX_CENTER = 4;
	public const int GROUND_INDEX_RIGHT = 5;
	public const int GROUND_INDEX_LEFT_BOT = 6;
	public const int GROUND_INDEX_BOT = 7;
	public const int GROUND_INDEX_RIGHT_BOT = 8;

	#region Tilemap Funcs

	public const string TDB_FARM_TILES = "FarmTiles";

	public const string TILE_GROUP_GRASS = "Grass";
	public const string TILE_GROUP_FENCE = "Fence";
	public const string TILE_GROUP_GROUND = "Ground";

	public static TileBase GetGrassTile() {
		return TilesManager.Instance.GetTile(TDB_FARM_TILES, TILE_GROUP_GRASS);
	}
	
	public static TileBase GetFenceTile(int i) {
		return TilesManager.Instance.GetTile(TDB_FARM_TILES, TILE_GROUP_FENCE, i);
	}
	
	public static TileBase GetGroundTile(int i) {
		return TilesManager.Instance.GetTile(TDB_FARM_TILES, TILE_GROUP_GROUND, i);
	}

	#endregion
}