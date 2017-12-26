using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Device : MonoBehaviour {
	public enum PHASE {
		BEGAN,		//タッチした
		MOVED,		//タッチしている
		ENDED,		//指を離した
		CANCELED,	//タッチの追跡をやめた
		NONE		//なし
	};

	System.Action checkPos;
	System.Action checkTouchPhase;
	static Vector2 _pos;
	static PHASE _phase;
	static int _stop_count;

	void Awake( ) {
		StopLittle( 30 );
		if ( isTouchDevice( ) ) {
			checkPos = checkPosTouch;
			checkTouchPhase = checkTouchPhaseTouch;
		} else {
			checkPos = checkPosMouse;
			checkTouchPhase = checkTouchPhaseMouse;
		}
	}

	void Update( ) {
		if ( _stop_count > 0 ) {
			_stop_count--;
			return;
		}
		checkPos( );
		checkTouchPhase( );
	}

	public static void StopLittle( int count ) {
		_stop_count = count;
		_phase = PHASE.NONE;
	}

	public static Vector2 getPos( ) {
		return _pos;
	}

	public static PHASE getTouchPhase( ) {
		return _phase;
	}

	bool isTouchDevice( ) {
		return ( Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer );
	}

	void checkPosTouch( ) {
		_pos = Input.GetTouch( 0 ).position;
	}

	void checkPosMouse( ) {
		_pos = Input.mousePosition;
	}

	void checkTouchPhaseTouch( ) {
		switch ( Input.GetTouch( 0 ).phase ) {
		case TouchPhase.Began:
			_phase = PHASE.BEGAN;
			break;
		case TouchPhase.Moved:
			_phase = PHASE.MOVED;
			break;
		case TouchPhase.Stationary:
			_phase = PHASE.MOVED;
			break;
		case TouchPhase.Ended:
			_phase = PHASE.ENDED;
			break;
		case TouchPhase.Canceled:
			_phase = PHASE.CANCELED;
			break;
		}
	}

	void checkTouchPhaseMouse( ) {
		if ( Input.GetMouseButton( 0 ) ) {
			_phase = PHASE.MOVED;
		}
		if ( Input.GetMouseButtonDown( 0 ) ) {
			_phase = PHASE.BEGAN;
		}
		if ( Input.GetMouseButtonUp( 0 ) ) {
			_phase = PHASE.ENDED;
		}
		   
	}
}

