// File create date:2024/1/26
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using RoachLite.Basic;
using RoachLite.Data;
using UnityEngine;
// Created By Yu.Liu
public abstract class BasePlantController : BaseObject {
	
	public int Id { get; protected set; }
	public Square Coord { get; protected set; }

	protected SpriteRenderer avatarSprite;
	protected Animator avatarAnimator;
	
	private FarmDataService dataService;
	private FarmObjectManager objectManager;
	private FarmPawnManager pawnManager;

	protected override void OnAwake() {
		base.OnAwake();
		avatarSprite = FindComponent<SpriteRenderer>("Avatar");
		avatarAnimator = avatarSprite.GetComponent<Animator>();
	}

	protected override void OnStart() {
		base.OnStart();
		dataService = FarmDataService.Instance;
		objectManager = FarmObjectManager.Instance;
		pawnManager = FarmPawnManager.Instance;
		Coord = MapUtils.WorldToSquare(transform.position);
	}

	public void SetupIdentifier(int id) {
		Id = id;
	}

	protected override void OnLazyLoad() {
		base.OnLazyLoad();
		FarmObjectManager.Instance.RegisterPlant(this);
	}

	public async void SetupCoordinate(Square sq) {
		await UniTask.Yield();
		transform.position = MapUtils.SquareToWorld(sq);
		Coord = sq;
		FarmObjectManager.Instance.UpdatePlantMap(this);
	}
}