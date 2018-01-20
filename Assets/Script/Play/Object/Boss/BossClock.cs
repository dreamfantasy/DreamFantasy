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

	const float MAX_ROT_SPEED = 5 * Mathf.PI * 2 / 360;//度数法
    const int WAIT_COUNT = 200;

	GameObject[ ] _needles = new GameObject[ ( int )NEEDLE.MAX ];
	GameObject[ ] _switchs = new GameObject[ ( int )NEEDLE.MAX ];
    int[ ] _length;
    Quaternion[ ] _target_times = new Quaternion[ ( int )NEEDLE.MAX ];
    int _wait_count = 0;

	STATE _state;

	void Awake( ) {
        _length = new int[ ( int )NEEDLE.MAX ];
        _length[ ( int )NEEDLE.SHORT ] = 300;
        _length[ ( int )NEEDLE.LONG ] = 400;
		loadSwitchs( );
		serchNeedles( );
	}
	
	void Start( ) {
        setState( STATE.ROTATION );
	}
	
	void Update( ) {
        switch ( _state ) {
        case STATE.WAIT:
            wait( );
            break;
        case STATE.ROTATION:
            rotation( );
            break;
        }
	}

    void setState( STATE state ) {
        _state = state;
        switch ( state ) {
        case STATE.WAIT:
            _wait_count = 0;
            setSwitchPos( );
            foreach ( GameObject obj in _switchs ) {
                obj.SetActive( true );
            }
            break;
        case STATE.ROTATION:
            changeTargetTimes( );
            foreach ( GameObject obj in _switchs ) {
                obj.SetActive( false );
            }
            break;
        }
    }

    void wait( ) {
        if ( _wait_count > WAIT_COUNT ) {
            setState( STATE.ROTATION );
        }
        _wait_count++;
    }

	Quaternion getTimeQuaterion( int time ) {
        int angle = time * 360 / 12;
        if ( angle > 180 ) {
            angle = -360 + angle;
        }
        return Quaternion.AngleAxis( angle, Vector3.back );
	}

	void rotation( ) {
        bool rot_flag = false;
		for ( int i = 0; i < ( int )NEEDLE.MAX; i++ ) {
            //目的の位置までの角度
            Vector3 dir = _needles[ i ].transform.rotation.eulerAngles;
            Vector3 target_dir = _target_times[ i ].eulerAngles;
            float angle = Quaternion.Angle( _needles[ i ].transform.rotation, _target_times[ i ] );
			//angle = Mathf.PI * 2 * 360 / angle;
            //回転速度調整
            bool rot_flag2 = false;
			if ( angle > MAX_ROT_SPEED ) {
				angle = MAX_ROT_SPEED;
                rot_flag2 = true;
			}
			if ( angle < -MAX_ROT_SPEED ) {
				angle = -MAX_ROT_SPEED;
                rot_flag2 = true;
			}

            //if ( angle < 0 ) {
             //   angle = Mathf.PI * 2 + angle;
            //}

            if ( rot_flag2 ) {
                rot_flag = true;
                //回転ベクトル
                //dir.z += angle;
                //回転
                //Quaternion rot = _needles[ i ].transform.rotation;
                Quaternion rot = _needles[ i ].transform.rotation * Quaternion.AngleAxis( angle * 360 / ( 2 * Mathf.PI ), Vector3.back );
                //rot.eulerAngles = dir;
                _needles[ i ].transform.rotation = rot;
            } else {
                _needles[ i ].transform.rotation = _target_times[ i ];
            }

		}
        if ( !rot_flag ) {
            setState( STATE.WAIT );
        }
	}

	void changeTargetTimes( ) {
        _target_times[ 0 ] = getTimeQuaterion( Random.Range( 0, 11 ) );
        _target_times[ 1 ] = getTimeQuaterion( Random.Range( 0, 11 ) );
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

    void setSwitchPos( ) {
        for ( int i = 0; i < ( int )NEEDLE.MAX; i++ ) {
            //針の角度
            _switchs[ i ].transform.position = _target_times[ i ] * Vector3.up * _length[ i ];
        }
    }
}
