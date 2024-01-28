// File create date:2024/1/28
using System;
using System.Collections.Generic;
using RoachLite.AnimationManagement;
using UnityEngine;
// Created By Yu.Liu
public static class AnimUtils {

	public const string ADB_NAME_PAWN_ANIMS = "PawnAnims";

	public const string ANIM_NAME_BULL = "Bull";
	public const string ANIM_NAME_CAPYBARA = "Capybara";
	public const string ANIM_NAME_COW = "Cow";
	public const string ANIM_NAME_FROG = "Frog";
	public const string ANIM_NAME_GOAT = "Goat";
	public const string ANIM_NAME_HUMAN = "Human";
	public const string ANIM_NAME_MOUSE = "Mouse";
	public const string ANIM_NAME_OWL = "Owl";
	public const string ANIM_NAME_PARROT_KING = "ParrotKing";
	public const string ANIM_NAME_PARROT_KNIGHT = "ParrotKnight";
	public const string ANIM_NAME_PIG = "Pig";

	public static AnimationClip GetPawnAnim(string an, int i = 0) {
		return AnimaManager.Instance.GetAnima(ADB_NAME_PAWN_ANIMS, an, i);
	}
}