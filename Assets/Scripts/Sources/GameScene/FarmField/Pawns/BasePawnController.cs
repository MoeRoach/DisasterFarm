// File create date:2024/1/26
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using RoachLite.Basic;
using RoachLite.Data;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using Random = UnityEngine.Random;

// Created By Yu.Liu
public abstract class BasePawnController : BaseObject {

	public int Id { get; protected set; }
	public string Name { get; protected set; }
	public virtual Square Coord => MapUtils.WorldToSquare(transform.position);
	
	protected SpriteRenderer avatarSprite;
	protected Animator avatarAnimator;
	protected PlayableGraph avatarGraph;
	
	private FarmDataService dataService;
	private FarmObjectManager objectManager;
	private FarmPawnManager pawnManager;

	protected Queue<PawnCommand> commandQueue;
	protected List<AsyncCommandRunner> runnerList;

	protected override void OnAwake() {
		base.OnAwake();
		avatarSprite = FindComponent<SpriteRenderer>("Avatar");
		avatarAnimator = avatarSprite.GetComponent<Animator>();
		commandQueue = new Queue<PawnCommand>();
		runnerList = new List<AsyncCommandRunner>();
	}

	protected override void OnStart() {
		base.OnStart();
		dataService = FarmDataService.Instance;
		objectManager = FarmObjectManager.Instance;
		pawnManager = FarmPawnManager.Instance;
		RegisterUpdateFunction(1, UpdateCommandQueue);
		InitPawnAnimation();
	}

	protected abstract void InitPawnAnimation();

	protected AnimationClipPlayable PlayClipPlayable(AnimationClip clip) {
		return AnimationPlayableUtilities.PlayClip(avatarAnimator, clip, out avatarGraph);
	}

	public void SetupIdentifier(int id, string n) {
		Id = id;
		Name = n;
	}

	public void GetAttack(BasePawnController src) {
		var last = runnerList.Count > 0 ? runnerList[0] : null;
		if (last == null) return;
		if (last.cmd.HasArg(PawnCommand.CMD_ARG_KEY_RUN_AWAY)) return;
		last.isCanceled = true;
		while (commandQueue.Count > 0) {
			var ncmd = commandQueue.Peek();
			if (!ncmd.HasArg(PawnCommand.CMD_ARG_KEY_RUN_AWAY))
				commandQueue.Dequeue();
			else break;
		}
	}

	public void SendCommand(PawnCommand cmd) {
		commandQueue.Enqueue(cmd);
	}

	private void UpdateCommandQueue() {
		if (commandQueue.Count <= 0) return;
		var ncmd = commandQueue.Peek();
		var last = runnerList.Count > 0 ? runnerList[0] : null;
		if (last != null && last.isExecute) {
			if (!ncmd.interrupt) return;
			last.isCanceled = true;
		}
		
		var runner = CompileCommand(ncmd);
		ExecuteCommand(runner);
		commandQueue.Dequeue();
	}

	private AsyncCommandRunner CompileCommand(PawnCommand cmd) {
		return new AsyncCommandRunner {cmd = cmd};
	}

	protected virtual async UniTask OnCommandExecute(AsyncCommandRunner runner) {
		switch (runner.cmd.cmd) {
			case PawnCommand.CMD_STR_MOVE:
				await ExecuteMove(runner);
				break;
			case PawnCommand.CMD_STR_PLANT:
				await ExecutePlant(runner);
				break;
			case PawnCommand.CMD_STR_HARVEST:
				await ExecuteHarvest(runner);
				break;
			case PawnCommand.CMD_STR_FOLLOW:
				await ExecuteFollow(runner);
				break;
			case PawnCommand.CMD_STR_ATTACK:
				await ExecuteAttack(runner);
				break;
			case PawnCommand.CMD_STR_HIDE:
				await ExecuteHide(runner);
				break;
			case PawnCommand.CMD_STR_SHOW:
				await ExecuteShow(runner);
				break;
			case PawnCommand.CMD_STR_DONE:
				await ExecuteDone(runner);
				break;
		}
	}

	protected virtual async UniTask ExecuteMove(AsyncCommandRunner runner) {
		var target = runner.cmd.GetVector3(PawnCommand.CMD_ARG_KEY_TARGET_POSITION);
		var spd = runner.cmd.GetFloat(PawnCommand.CMD_ARG_KEY_MOVE_SPEED);
		var timer = 0f;
		var start = transform.position;
		var timeLength = (target - start).magnitude / spd;
		while (timer < 1f) {
			timer += Time.deltaTime / timeLength;
			transform.position = Vector3.Lerp(start, target, timer);
			await UniTask.Yield();
			if (runner.isCanceled) return;
		}
	}
	

	protected virtual async UniTask ExecutePlant(AsyncCommandRunner runner) {
		var serial = runner.cmd.GetInteger(PawnCommand.CMD_ARG_KEY_PLANT_SERIAL);
		var duration = runner.cmd.GetFloat(PawnCommand.CMD_ARG_KEY_TIME_DURATION);
		var timer = 0f;
		DoPlant(serial);
		while (timer < duration) {
			timer += Time.deltaTime;
			await UniTask.Yield();
			if (runner.isCanceled) return;
		}
	}

	protected virtual void DoPlant(int ps) {
		var coord = MapUtils.WorldToSquare(transform.position);
		FarmFieldRootControl.Instance.PlantFarmPlant(ps, coord.x, coord.y);
	}
	
	protected virtual async UniTask ExecuteHarvest(AsyncCommandRunner runner) {
		var pid = runner.cmd.GetInteger(PawnCommand.CMD_ARG_KEY_PLANT_IDENTIFIER);
		var duration = runner.cmd.GetFloat(PawnCommand.CMD_ARG_KEY_TIME_DURATION);
		var timer = 0f;
		DoHarvest(pid);
		while (timer < duration) {
			timer += Time.deltaTime;
			await UniTask.Yield();
			if (runner.isCanceled) return;
		}
	}

	protected virtual void DoHarvest(int pid) {
		var po = FarmObjectManager.Instance.GetPlant(pid);
		if (po == null) return;
		Destroy(po.gameObject);
		var coord = MapUtils.WorldToSquare(transform.position);
		FarmFieldRootControl.Instance.HarvestFarmPlant(coord.x, coord.y);
	}

	protected virtual async UniTask ExecuteFollow(AsyncCommandRunner runner) {
		var pid = runner.cmd.GetInteger(PawnCommand.CMD_ARG_KEY_PAWN_IDENTIFIER);
		var po = FarmPawnManager.Instance.GetPawn(pid);
		var spd = runner.cmd.GetFloat(PawnCommand.CMD_ARG_KEY_MOVE_SPEED);
		Vector3 offset = Random.insideUnitCircle.normalized * 0.2f;
		var timer = 0f;
		var start = transform.position;
		while (timer < 1f) {
			timer += Time.deltaTime * spd;
			transform.position = Vector3.Lerp(start, po.transform.position + offset, timer);
			await UniTask.Yield();
			if (runner.isCanceled) return;
		}
	}

	protected virtual async UniTask ExecuteAttack(AsyncCommandRunner runner) {
		var tpid = runner.cmd.GetInteger(PawnCommand.CMD_ARG_KEY_PAWN_IDENTIFIER);
		var preDelay = 0.1f;
		var postDelay = 0.2f;
		var timer = 0f;
		while (timer < preDelay) {
			timer += Time.deltaTime;
			await UniTask.Yield();
			if (runner.isCanceled) return;
		}

		var pawn = FarmPawnManager.Instance.GetPawn(tpid);
		DoAttack(pawn);
		while (!CheckAttackDone()) {
			await UniTask.Yield();
			if (runner.isCanceled) return;
		}

		timer = 0f;
		while (timer < postDelay) {
			timer += Time.deltaTime;
			await UniTask.Yield();
			if (runner.isCanceled) return;
		}
	}

	protected virtual void DoAttack(BasePawnController target) {
		target.GetAttack(this);
	}

	protected virtual bool CheckAttackDone() {
		return true;
	}

	protected virtual async UniTask ExecuteHide(AsyncCommandRunner runner) {
		DoHide();
		await UniTask.Yield();
	}

	protected virtual void DoHide() {
		avatarSprite.color = Color.clear;
	}

	protected virtual async UniTask ExecuteShow(AsyncCommandRunner runner) {
		DoShow();
		await UniTask.Yield();
	}

	protected virtual void DoShow() {
		avatarSprite.color = Color.white;
	}

	protected virtual async UniTask ExecuteDone(AsyncCommandRunner runner) {
		await UniTask.Yield();
	}
	
	private async void ExecuteCommand(AsyncCommandRunner runner) {
		runner.isExecute = true;
		runnerList.Insert(0, runner);
		await UniTask.Yield();
		await OnCommandExecute(runner);
		runner.isExecute = false;
		await UniTask.Yield();
		if (runner.isCanceled) OnCommandCancel(runner);
		else OnCommandDone(runner);
	}

	protected virtual void OnCommandDone(AsyncCommandRunner runner) {
		runnerList.Remove(runner);
		if (runner.cmd.cmd.Equals(PawnCommand.CMD_STR_DONE)) {
			CaseViewController.Instance.AddToPool(Name);
			Destroy(gameObject);
		}
	}

	protected virtual void OnCommandCancel(AsyncCommandRunner runner) {
		runnerList.Remove(runner);
	}

	protected override void Release() {
		base.Release();
		if (avatarGraph.IsValid()) avatarGraph.Destroy();
	}

	protected class AsyncCommandRunner {

		public PawnCommand cmd;
		public bool isExecute;
		public bool isCanceled;

	}
}