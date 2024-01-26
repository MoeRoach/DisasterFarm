// File create date:2024/1/26
using System;
using System.Collections.Generic;
using UnityEngine;
// Created By Yu.Liu
public class FarmData {
	// Script Code
}

public class PawnCommand {

	public const string CMD_STR_MOVE = "Move";
	public const string CMD_STR_PLANT = "Plant";
	public const string CMD_STR_HARVEST = "Harvest";
	public const string CMD_STR_ATTACK = "Attack";
	public const string CMD_STR_HIDE = "Hide";
	
	public string cmd;
	public string args;
	public bool interrupt;

	public PawnCommand(string c) {
		cmd = c;
	}
}