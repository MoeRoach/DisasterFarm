// File create date:2024/1/27
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using RoachLite.Basic;
using RoachLite.Data;
using UnityEngine;
// Created By Yu.Liu
public abstract class BaseFarmObject : BaseObject  {
	
	public int Id { get; protected set; }
	public Square Coord { get; protected set; }
	public Square Area { get; protected set; }

	protected SpriteRenderer avatarSprite;

	protected override void OnAwake() {
		base.OnAwake();
		avatarSprite = FindComponent<SpriteRenderer>("Avatar");
	}

	protected override void OnStart() {
		base.OnStart();
		InitObjectArea();
		Coord = MapUtils.WorldToSquare(transform.position);
	}

	public void SetupIdentifier(int id) {
		Id = id;
	}

	protected override void OnLazyLoad() {
		base.OnLazyLoad();
		UpdateAvatarPosition();
		FarmObjectManager.Instance.RegisterObject(this);
	}

	protected virtual void InitObjectArea() {
		Area = Square.One;
	}

	protected virtual void UpdateAvatarPosition() {
		var offset = Vector3.zero;
		offset.x = (Area.x - 1) / 2f;
		offset.y = (Area.y - 1) / 2f;
		avatarSprite.transform.localPosition += offset;
	}

	public async void SetupCoordinate(Square coord) {
		await UniTask.Yield();
		transform.position = MapUtils.SquareToWorld(coord);
		Coord = coord;
	}

	protected override void Release() {
		base.Release();
		if (FarmObjectManager.Instance == null) return;
		FarmObjectManager.Instance.UnregisterObject(this);
	}
}