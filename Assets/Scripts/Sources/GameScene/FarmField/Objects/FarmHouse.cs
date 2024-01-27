// File create date:2024/1/27
using System;
using System.Collections.Generic;
using RoachLite.Data;
using UnityEngine;
// Created By Yu.Liu
public class FarmHouse : BaseFarmObject  {
	protected override void InitObjectArea() {
		Area = new Square(4, 3);
	}
}