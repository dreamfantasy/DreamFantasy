using UnityEngine;
using System;


public class WallMove : Wall {
	[System.Serializable]
	public class Option {
		public Vector2 vec;
		public int move_range;
	};
	[SerializeField]
	public Option option;
	
	int _count = 0;
    int _reverse_time;
	bool reversed = false;
	Vector3 _start_pos;

	public override void Awake( ) {
		base.Awake( );
		//Rigidbody2D rd = gameObject.AddComponent< Rigidbody2D >( );
		//rd.freezeRotation = true;
		//rd.gravityScale = 0.0f;
		gameObject.isStatic = false;
	}
	
	public override void Start( ) {
        base.Start( );
        checkReverseTime( );
		_start_pos = transform.position;
	}

	public override void Update( ) {
		base.Update( );
        transform.position = transform.position + ( Vector3 )option.vec;
        if ( _reverse_time <= 0 ) {
            return;
        }
        _count = ( _count + 1 ) % _reverse_time;
		if ( _count == 0 ) {
			reverse( );
		}
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
        reversed = false;
	}


    void checkReverseTime( ) {
        if ( Mathf.Abs( option.vec.x ) > Mathf.Abs( option.vec.y ) ) {
            //横移動
            _reverse_time = ( int )( option.move_range / Mathf.Abs( option.vec.x ) );
        } else {
            _reverse_time = ( int )( option.move_range / Mathf.Abs( option.vec.y ) );
        }
        //Debug.Log( "reversetime" + _reverse_time );
    }
}
   
