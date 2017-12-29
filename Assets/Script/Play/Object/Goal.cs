using UnityEngine;
class Goal : MonoBehaviour {
	bool enable = false;
	Switch[ ] switchs;


	void Awake( ) {
		GameObject[ ] objects = GameObject.FindGameObjectsWithTag( Play.getTag( Play.BOARDOBJECT.SWITCH ) );
		switchs = new Switch[ objects.Length ];
		for ( int i = 0; i < objects.Length; i++ ) {
			switchs[ i ] = objects[ i ].GetComponent< Switch >( );
		}
	}

	void Update( ) {
		if ( isExistanceActiveSwitch( ) ) {
			if ( enable ) {
				setEnable( false );
			}
		} else {
			if ( !enable ) {
				setEnable( true );
			}
		}
	}

	bool isExistanceActiveSwitch( ) {
		if ( switchs.Length == 0 ) {
			return false;
		}
		bool result = false;
		foreach ( Switch sw in switchs ) {
			if ( sw.enable ) {
				result = true;
				break;
			}
		}
		return result;
	}

	void setEnable( bool value ) {
		enable = value;
	}
}