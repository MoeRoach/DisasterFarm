// File create date:2024/1/28
using System;
using System.Collections.Generic;
using UnityEngine;
// Created By Yu.Liu
public class PawnParrotKnight : BasePawnController  {
	// Script Code
	protected override void InitPawnAnimation() {
		var clip = AnimUtils.GetPawnAnim(AnimUtils.ANIM_NAME_PARROT_KNIGHT);
		var playable = PlayClipPlayable(clip);
	}
}