// File create date:2024/1/26
using System;
using System.Collections.Generic;
using RoachLite.Basic;
using UnityEngine;
// Created By Yu.Liu
public abstract class BasePlantController : BaseObject {
	
	public int Id { get; protected set; }

	protected SpriteRenderer avatarSprite;
	protected Animator avatarAnimator;

	protected override void OnAwake() {
		base.OnAwake();
		avatarSprite = FindComponent<SpriteRenderer>("Avatar");
		avatarAnimator = avatarSprite.GetComponent<Animator>();
	}
	
	public void SetupIdentifier(int id) {
		Id = id;
	}
}