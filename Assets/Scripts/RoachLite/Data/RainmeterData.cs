// File create date:2022/7/22
using UnityEngine;
// Created By Yu.Liu
namespace RoachLite.Data {
	/// <summary>
	/// 整型进度数据
	/// </summary>
	public class IntegerMeter {
		
		public int cusorValue;
		public int targetValue;

		public IntegerMeter(int val, bool startFull = false) {
			targetValue = val;
			cusorValue = startFull ? targetValue : 0;
		}

		public void ChangeCusor(int delta) {
			cusorValue += delta;
			if (cusorValue < 0) {
				cusorValue = 0;
			} else if (cusorValue > targetValue) {
				cusorValue = targetValue;
			}
		}

		public void SetupCursor(int value) {
			cusorValue = value;
			if (cusorValue < 0) {
				cusorValue = 0;
			} else if (cusorValue > targetValue) {
				cusorValue = targetValue;
			}
		}
		
		public void SetupTarget(int value) {
			targetValue = value;
			if (cusorValue < targetValue) {
				cusorValue = targetValue;
			}
		}

		public bool IsMax() {
			return cusorValue >= targetValue;
		}

		public float GetPercentage() {
			if (targetValue <= 0) return 1f;
			return cusorValue * 1f / targetValue;
		}
	}

	/// <summary>
	/// 浮点型进度数据
	/// </summary>
	public class FloatMeter {

		public float cusorValue;
		public float targetValue;

		public FloatMeter(float val, bool startFull = false) {
			targetValue = val;
			cusorValue = startFull ? targetValue : 0f;
		}
		
		public void ChangeCusor(float delta) {
			cusorValue += delta;
			if (cusorValue < 0f) {
				cusorValue = 0f;
			} else if (cusorValue > targetValue) {
				cusorValue = targetValue;
			}
		}

		public void SetupCusor(float value) {
			cusorValue = value;
			if (cusorValue < 0f) {
				cusorValue = 0f;
			} else if (cusorValue > targetValue) {
				cusorValue = targetValue;
			}
		}

		public void SetupTarget(float value) {
			targetValue = value;
			if (cusorValue < targetValue) {
				cusorValue = targetValue;
			}
		}

		public bool IsMax() {
			return cusorValue >= targetValue;
		}
		
		public float GetPercentage() {
			if (targetValue <= 0f) return 1f;
			return cusorValue * 1f / targetValue;
		}
	}
}