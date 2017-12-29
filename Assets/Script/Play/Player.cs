using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour {
	public Sprite[ ] _ball_sprites;
	private GameObject _allow;
	private int _hp;
	void Awake( ) {
		_allow = Resources.Load( "Play/Prefab/Allow" ) as GameObject;
		_allow.transform.position = new Vector3( );
	}
	void update( ) {
	}
}
