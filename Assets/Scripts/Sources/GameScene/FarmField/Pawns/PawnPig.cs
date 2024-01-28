// File create date:2024/1/26
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// Created By Yu.Liu
public class PawnPig : BasePawnController  {
	// Script Code
	protected override void InitPawnAnimation() {
		var clip = AnimUtils.GetPawnAnim(AnimUtils.ANIM_NAME_PIG);
		var playable = PlayClipPlayable(clip);
	}
}