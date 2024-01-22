using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoachLite.Common {
	/// <summary>
	/// 简单的下一帧执行协程
	/// </summary>
	public class SimpleExecNext : IEnumerator {

		public object Current => null;

		private Action _action;
		private bool _done;

		public SimpleExecNext(Action act) {
			_action = act;
			_done = false;
		}

		public void SetupAction(Action act) {
			_action = act;
		}

		public bool MoveNext() {
			if (!_done) {
				_done = true;
				return true;
			}

			_action?.Invoke();
			return false;
		}

		public void Reset() {
			_done = false;
		}
	}

	/// <summary>
	/// 简单的延时执行协程
	/// </summary>
	public class SimpleExecDelay : IEnumerator {

		public object Current => null;

		private Action _action;
		private float _delay;
		private bool _unscaleTime;
		private bool _done;
		private float _timer;

		private float DeltaTime => _unscaleTime
			? Time.unscaledDeltaTime
			: Time.deltaTime;

		public SimpleExecDelay(Action act, float delay, bool isUnscale = false) {
			_action = act;
			_delay = delay;
			_unscaleTime = isUnscale;
			_done = false;
			_timer = 0f;
		}

		public void SetupAction(Action act) {
			_action = act;
		}

		public void SetupDelay(float delay) {
			_delay = delay;
		}

		public void SetupUnscaleTime(bool isOn) {
			_unscaleTime = isOn;
		}

		public bool MoveNext() {
			if (!_done) {
				_timer += DeltaTime;
				_done = _timer >= _delay;
				return true;
			} else {
				_action?.Invoke();
				return false;
			}
		}

		public void Reset() {
			_done = false;
			_timer = 0f;
		}
	}

	/// <summary>
	/// 增强型协程封装，可以等待和中断以及替换协程方法体
	/// </summary>
	public class EnhanceCoroutine {

		private State _state;
		private IEnumerator _runner;
		public bool IsExec => _state == State.Exec;
		public bool IsHold => _state == State.Hold;
		public bool IsStop => _state == State.Stop;

		public EnhanceCoroutine(IEnumerator runner) {
			_state = State.Hold;
			_runner = runner;
		}

		public void SetupRunner(IEnumerator runner) {
			_runner = runner;
		}

		public IEnumerator StartExec() {
			_state = State.Exec;
			return ExecHelper();
		}

		public IEnumerator StartHold() {
			_state = State.Hold;
			return ExecHelper();
		}

		public void Hold() {
			_state = State.Hold;
		}

		public void Exec() {
			_state = State.Exec;
		}

		public void Stop() {
			_state = State.Stop;
		}

		private IEnumerator ExecHelper() {
			if (_runner == null) yield break;
			while (_state == State.Hold) {
				yield return null;
			}

			while (_state == State.Exec) {
				while (_state == State.Hold) {
					yield return null;
				}

				if (_runner.MoveNext()) {
					yield return _runner.Current;
				} else {
					_state = State.Stop;
				}

				while (_state == State.Hold) {
					yield return null;
				}
			}
		}

		private enum State {
			Hold,
			Exec,
			Stop
		}
	}
}