// File create date:2022/7/16
using System;
using System.Collections.Generic;
using UnityEngine;
// Created By Yu.Liu
namespace RoachLite.Common {

	public interface IPhysicCaster2D {
		void Cast();
	}
	
	public interface IPhysicCaster3D {
		void Cast();
	}
	
	public abstract class BasePhysicCaster2D : IPhysicCaster2D {

		protected Vector2 point;

		public void SetupPoint(Vector2 p) {
			point = p;
		}
		
		public abstract void Cast();
	}
	
	public abstract class BasePhysicCaster3D : IPhysicCaster3D {
		
		protected Vector3 point;

		public void SetupPoint(Vector3 p) {
			point = p;
		}
		
		public abstract void Cast();
	}
}
