using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossClock : Boss {
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

	GameObject[ ] _needles      = new GameObject[ ( int )NEEDLE.MAX ];
	GameObject[ ] _switchs      = new GameObject[ ( int )NEEDLE.MAX ];
    Quaternion[ ] _target_times = new Quaternion[ ( int )NEEDLE.MAX ];
    int _length = 400;
    int _wait_count = 0;
	bool _col = false;

	STATE _state;

	void Awake( ) {
		loadSwitchs( );
		loadNeedles( );
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

	public override void reset( ) {
		base.reset( );
		_col = false;
		_switchs[ 0 ].GetComponent< Switch >( ).reset( );
		_switchs[ 1 ].GetComponent< Switch >( ).reset( );
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
		if ( !_col ) {
			if ( _switchs[ 0 ].GetComponent< Switch >( ).enable ) {
				_col = true;
			}
			if ( _switchs[ 1 ].GetComponent< Switch >( ).enable ) {
				if ( _col ) {
					_switchs[ 1 ].GetComponent< Switch >( ).reset( );
				}
				_col = true;
			}
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

            if ( rot_flag2 ) {
                rot_flag = true;
                //回転
                Quaternion rot = _needles[ i ].transform.rotation * Quaternion.AngleAxis( angle * 360 / ( 2 * Mathf.PI ), Vector3.back );
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

	void loadNeedles( ) {
        string path0 = "Play/Prefab/Boss/ClockLong";
        string path1 = "Play/Prefab/Boss/ClockShort";
        GameObject prefab0 = Resources.Load< GameObject >( path0 );
        GameObject prefab1 = Resources.Load< GameObject >( path1 );
        _needles[ 0 ] = Instantiate( prefab0 );
        _needles[ 1 ] = Instantiate( prefab1 );
        _needles[ 0 ].transform.parent = gameObject.transform;
        _needles[ 1 ].transform.parent = gameObject.transform;
	}

    void setSwitchPos( ) {
        for ( int i = 0; i < ( int )NEEDLE.MAX; i++ ) {
            //針の角度
            _switchs[ i ].transform.position = _target_times[ i ] * Vector3.up * _length;
        }
    }

    public override void erase( ) {
        Destroy( _needles[ 0 ] );
        Destroy( _needles[ 1 ] );
        Destroy( _switchs[ 0 ] );
        Destroy( _switchs[ 1 ] );
    }
}
