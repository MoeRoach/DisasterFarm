// File create date:2024/1/26
using System;
using System.Collections.Generic;
using RoachLite.Data;
using UnityEngine;
// Created By Yu.Liu
public class FarmData {

	public Dictionary<string, FarmTile> tiles;

	public FarmData() {
		tiles = new Dictionary<string, FarmTile>();
	}
}

public class FarmTile {
	public Square coord;
	public string Id => coord.Sid;

	public int ground;
	public int field;
	public int structure;

	public FarmTile(int x, int y) {
		coord = new Square(x, y);
	}
}

public class PawnCommand {

	public const string CMD_STR_MOVE = "Move";
	public const string CMD_STR_PLANT = "Plant";
	public const string CMD_STR_HARVEST = "Harvest";
	public const string CMD_STR_ATTACK = "Attack";
	public const string CMD_STR_HIDE = "Hide";
	public const string CMD_STR_SHOW = "Show";

	public const string CMD_ARG_KEY_TARGET_POSITION = "TargetPosition";
	public const string CMD_ARG_KEY_MOVE_SPEED = "MoveSpeed";
	public const string CMD_ARG_KEY_PLANT_SERIAL = "PlantSerial";
	public const string CMD_ARG_KEY_PLANT_IDENTIFIER = "PlantIdentifier";
	public const string CMD_ARG_KEY_TIME_DURATION = "TimeDuration";
	
	public string cmd;
	public Dictionary<string, object> args;
	public bool interrupt;

	public PawnCommand(string c) {
		cmd = c;
		args = new Dictionary<string, object>();
	}

	public void PutString(string key, string val) {
		args[key] = val;
	}

	public string GetString(string key) {
		if (!args.ContainsKey(key)) return null;
		return (string) args[key];
	}

	public void PutInteger(string key, int val) {
		args[key] = val;
	}
	
	public int GetInteger(string key) {
		if (!args.ContainsKey(key)) return int.MinValue;
		return (int) args[key];
	}
	
	public void PutFloat(string key, float val) {
		args[key] = val;
	}

	public float GetFloat(string key) {
		if (!args.ContainsKey(key)) return float.NaN;
		return (float) args[key];
	}

	public void PutVector3(string key, Vector3 v) {
		args[key] = v;
	}

	public Vector3 GetVector3(string key) {
		if (!args.ContainsKey(key)) return Vector3.negativeInfinity;
		return (Vector3) args[key];
	}
}

public static class PlantConfigs {
	
	public const int PLANT_SERIAL_BERRY = 0;
	public const int PLANT_SERIAL_BUSH = 1;
	public const int PLANT_SERIAL_CARROT = 2;
	public const int PLANT_SERIAL_FRUIT = 3;
	
	public static Dictionary<int, string> PlantNames = new Dictionary<int, string> {
		[PLANT_SERIAL_BERRY] = PrefabUtils.PREFAB_NAME_PLANT_BERRY,
		[PLANT_SERIAL_BUSH] = PrefabUtils.PREFAB_NAME_PLANT_BUSH,
		[PLANT_SERIAL_CARROT] = PrefabUtils.PREFAB_NAME_PLANT_CARROT,
		[PLANT_SERIAL_FRUIT] = PrefabUtils.PREFAB_NAME_PLANT_FRUIT
	};
	
	public static Dictionary<int, int> PlantFields = new Dictionary<int, int> {
		[PLANT_SERIAL_BERRY] = 2,
		[PLANT_SERIAL_BUSH] = 3,
		[PLANT_SERIAL_CARROT] = 1,
		[PLANT_SERIAL_FRUIT] = 0
	};
}