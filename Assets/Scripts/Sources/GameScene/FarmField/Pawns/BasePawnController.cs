// File create date:2024/1/26
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using RoachLite.Basic;
using UnityEngine;
// Created By Yu.Liu
public abstract class BasePawnController : BaseObject {

	public int Id { get; protected set; }
	
	protected SpriteRenderer avatarSprite;
	protected Animator avatarAnimator;

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
		RegisterUpdateFunction(1, UpdateCommandQueue);
	}

	public void SetupIdentifier(int id) {
		Id = id;
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
		await UniTask.CompletedTask;
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
	}

	protected virtual void OnCommandCancel(AsyncCommandRunner runner) {
		runnerList.Remove(runner);
	}

	protected class AsyncCommandRunner {

		public PawnCommand cmd;
		public bool isExecute;
		public bool isCanceled;

	}
}