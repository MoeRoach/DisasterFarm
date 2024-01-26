// File create date:2024/1/26
using System;
using System.Collections.Generic;
using RoachLite.PrefabManagement;
using UnityEngine;
// Created By Yu.Liu
public static class PrefabUtils {

	public const string PDB_NAME_FARM_PAWNS = "FarmPawns";
	public const string PDB_NAME_FARM_PLANTS = "FarmPlants";
	
	public const string PREFAB_NAME_PLAYER_PIG_PAWN = "PlayerPigPawn";
	public const string PREFAB_NAME_ENEMY_ALIEN_PAWN = "EnemyAlienPawn";

	public static GameObject CreateObject(string db, string grp, int i = 0, Transform root = null) {
		return PrefabManager.Instance.CreateFromPrefab(db, grp, root, i);
	}

	public static GameObject CreatePawn(string pn, int index, Transform root) {
		return CreateObject(PDB_NAME_FARM_PAWNS, pn, index, root);
	}
	
	public static GameObject CreatePlant(string pn, int index, Transform root) {
		return CreateObject(PDB_NAME_FARM_PLANTS, pn, index, root);
	}
}