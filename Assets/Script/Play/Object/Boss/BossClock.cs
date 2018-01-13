using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossClock : MonoBehaviour {
	enum STATE {
		WAIT,
		ROTATION,
	};
	enum NEEDLE {
		LONG,
		SHORT,
		MAX
	};

	const float MAX_ROT_SPEED = 1;//度数法

	GameObject[ ] _needles = new GameObject[ ( int )NEEDLE.MAX ];
	GameObject[ ] _switchs = new GameObject[ ( int )NEEDLE.MAX ];
	int[ ] _target_times = new int[ ( int )NEEDLE.MAX ];

	STATE _state;

	void Awake( ) {
		loadSwitchs( );
		serchNeedles( );
	}
	
	void Start( ) {
		_state = STATE.ROTATION;
	}
	
	void Update( ) {
		rotation( );
	}

	Quaternion getTimeQuaterion( int time ) {
		return Quaternion.AngleAxis( Mathf.PI * 2 * time / 12, Vector3.back );
	}

	void rotation( ) {
		if ( _state != STATE.ROTATION ) {
			return;
		}
		for ( int i = 0; i < ( int )NEEDLE.MAX; i++ ) {
			float angle = Quaternion.Angle( _needles[ i ].transform.rotation, getTimeQuaterion( _target_times[ i ] ) );
			angle = Mathf.PI * 2 * 360 / angle;
			if ( angle > MAX_ROT_SPEED ) {
				angle = MAX_ROT_SPEED;
			}
			if ( angle < -MAX_ROT_SPEED ) {
				angle = -MAX_ROT_SPEED;
			}
			Vector3 rot = Vector3.forward * angle;

			_needles[ i ].transform.Rotate( rot );
		}
	}

	void changeTargetTimes( ) {
		_target_times[ 0 ] = Random.Range( 0, 11 );
		_target_times[ 1 ] = Random.Range( 0, 11 );
	}

	void loadSwitchs( ) {
		GameObject prefab = Resources.Load< GameObject >( Play.getPrefabPath( Play.BOARDOBJECT.SWITCH ) );
		_switchs[ 0 ] = Instantiate( prefab );
		_switchs[ 1 ] = Instantiate( prefab );
	}

	void serchNeedles( ) {
		_needles[ 0 ] = transform.Find( "Long" ).gameObject;
		_needles[ 1 ] = transform.Find( "Short" ).gameObject;
	}
}
