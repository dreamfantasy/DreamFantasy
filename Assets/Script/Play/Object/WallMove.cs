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
	
	int _count = 0;
	bool _col = false;
	bool reversed = false;
	Vector3 _start_pos;

	public override void Awake( ) {
		base.Awake( );
		Rigidbody2D rd = gameObject.AddComponent< Rigidbody2D >( );
		rd.freezeRotation = true;
		rd.gravityScale = 0.0f;
		gameObject.isStatic = false;
	}
	
	public override void Start( ) {
		base.Start( );
		_start_pos = transform.position;
	}

	public override void Update( ) {
		base.Update( );
		_count = ( _count + 1 ) % option.reverse_time;
		if ( _count == 0 ) {
			reverse( );
		}
		transform.position = transform.position + ( Vector3 )option.vec;
		_col = false;
	}

	void OnCollisionEnter2D( Collision2D collision ) {
		if ( collision.collider.gameObject.tag == "Player" ) {
			return;
		}
		if ( _col ) {
			return;
		}
		_col = true;
		reverse( );
	}

	void reverse( ) {
		_count = 0;
		option.vec *= -1;
		reversed = !reversed;
	}

	public override void reset( ) {
		base.reset( );
		transform.position = _start_pos;
		_count = 0;
		if ( reversed ) {
			option.vec *= -1;
		}
	}
}
   
