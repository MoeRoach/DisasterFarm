// File create date:2024/1/27
using System;
using System.Collections.Generic;
using RoachLite.Common;
using UnityEngine;
// Created By Yu.Liu
public class FarmDataService : Singleton<FarmDataService> {

	public FarmData farmData;
	
	public override void Initialize() {
		farmData = new FarmData();
	}
}