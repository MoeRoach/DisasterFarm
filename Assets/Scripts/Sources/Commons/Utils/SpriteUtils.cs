// File create date:2024/1/27
using System;
using System.Collections.Generic;
using RoachLite.SpriteManagement;
using UnityEngine;
// Created By Yu.Liu
public static class SpriteUtils {

	public const string SDB_FARM_SPRITES = "FarmSprites";
	public const string SDB_CASE_IMAGES = "CaseImages";
	public const string SDB_CASE_SIGNALS = "CaseSignals";

	public const string SPRITE_NAME_PLANTS = "Plants";
	public const string SPRITE_NAME_HOUSE = "House";
	public const string SPRITE_NAME_POOL = "Pool";

	public const string SPRITE_NAME_PIG = "Pig";
	public const string SPRITE_NAME_BIRD = "Bird";
	public const string SPRITE_NAME_COW = "Cow";
	public const string SPRITE_NAME_GRASS = "Grass";
	public const string SPRITE_NAME_HAT = "Hat";
	public const string SPRITE_NAME_MOUSE = "Mouse";
	public const string SPRITE_NAME_WITCH = "Witch";

	public const string SPRITE_NAME_HIRE = "Hire";
	public const string SPRITE_NAME_FARM = "Farm";
	public const string SPRITE_NAME_FIGHT = "Fight";
	public const string SPRITE_NAME_OTHER = "Other";
	
	public static Sprite GetFarmSprite(string grp, int i = -1) {
		return SpriteManager.Instance.GetSprite(SDB_FARM_SPRITES, grp, i);
	}

	public static Sprite GetCaseImageSprite(string grp, int i = -1) {
		return SpriteManager.Instance.GetSprite(SDB_CASE_IMAGES, grp, i);
	}

	public static Sprite GetCaseSignalSprite(string grp, int i = -1) {
		return SpriteManager.Instance.GetSprite(SDB_CASE_SIGNALS, grp, i);
	}
}