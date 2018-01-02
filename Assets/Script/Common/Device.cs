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

	public static Device Instanse { get; private set; }
	public Vector2 Pos  { get; private set; }
	public PHASE Phase { get; private set; }

	System.Action checkPos;
	System.Action checkTouchPhase;

	GameObject _effect;
	int _stop_count;

	void Awake( ) {
		Instanse = this;
		StopLittle( 30 );
		if ( isTouchDevice( ) ) {
			checkPos = checkPosTouch;
			checkTouchPhase = checkTouchPhaseTouch;
		} else {
			checkPos = checkPosMouse;
			checkTouchPhase = checkTouchPhaseMouse;
		}

		//effect
		GameObject prefab = Resources.Load< GameObject >( "Common/Prefab/Effect/Touch" );
		_effect = Instantiate( prefab );
		DontDestroyOnLoad( _effect );
		_effect.transform.position = Vector3.left * 2000;
	}

	void Start( ) {
	}
	void Update( ) {
		if ( _stop_count > 0 ) {
			_stop_count--;
			return;
		}
		PHASE before = Phase;
		checkPos( );
		checkTouchPhase( );
		//エフェクト
		if ( before == PHASE.BEGAN ) {
			_effect.transform.position = Pos;
			_effect.GetComponent< EffectTouch >( ).touch( );
		}
	}


	public void StopLittle( int count ) {
		_stop_count = count;
		Phase = PHASE.NONE;
	}

	bool isTouchDevice( ) {
		return ( Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer );
	}

	void checkPosTouch( ) {
		Vector3 tmp = Input.GetTouch( 0 ).position;
		// Z軸修正
		tmp.z = 10f;
		// マウス位置座標をスクリーン座標からワールド座標に変換する
		tmp = Camera.main.ScreenToWorldPoint( tmp );
		Pos = tmp;
	}

	void checkPosMouse( ) {
		Vector3 tmp = Input.mousePosition;
		// Z軸修正
		tmp.z = 10f;
		// マウス位置座標をスクリーン座標からワールド座標に変換する
		Camera camera = GameObject.FindGameObjectWithTag( "MainCamera" ).GetComponent< Camera >( );
		tmp = camera.ScreenToWorldPoint( tmp );
		Pos = tmp;
	}

	void checkTouchPhaseTouch( ) {
		Phase = PHASE.NONE;
		switch ( Input.GetTouch( 0 ).phase ) {
		case TouchPhase.Began:
			Phase = PHASE.BEGAN;
			break;
		case TouchPhase.Moved:
			Phase = PHASE.MOVED;
			break;
		case TouchPhase.Stationary:
			Phase = PHASE.MOVED;
			break;
		case TouchPhase.Ended:
			Phase = PHASE.ENDED;
			break;
		case TouchPhase.Canceled:
			Phase = PHASE.CANCELED;
			break;
		}
	}

	void checkTouchPhaseMouse( ) {
		Phase = PHASE.NONE;
		if ( Input.GetMouseButton( 0 ) ) {
			Phase = PHASE.MOVED;
		}
		if ( Input.GetMouseButtonDown( 0 ) ) {
			Phase = PHASE.BEGAN;
		}
		if ( Input.GetMouseButtonUp( 0 ) ) {
			Phase = PHASE.ENDED;
		}
		   
	}
}

