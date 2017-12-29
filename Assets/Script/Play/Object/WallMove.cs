using UnityEngine;
using System;


public class WallMove : Wall {
	[System.Serializable]
	public class Option {
		public Vector2 vec;
		public int reverse_time;
	};
	[SerializeField]
	public Option option;
}
   
