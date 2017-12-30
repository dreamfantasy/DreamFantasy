using UnityEngine;
public class Goal : MonoBehaviour {
	public bool EnterPlayer { get; private set; }
	bool enable = false;
	Switch[ ] switchs;


	void Awake( ) {
		GameObject[ ] objects = GameObject.FindGameObjectsWithTag( Play.getTag( Play.BOARDOBJECT.SWITCH ) );
		switchs = new Switch[ objects.Length ];
		for ( int i = 0; i < objects.Length; i++ ) {
			switchs[ i ] = objects[ i ].GetComponent< Switch >( );
		}
	}

	void Start( ) {
		EnterPlayer = false;
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
	
	void OnTriggerEnter2D( Collider2D other ) {
		if ( !enable ) {
			return;
		}
		if ( other.tag != Play.getTag( Play.BOARDOBJECT.PLAYER ) ) {
			return;
		}
		EnterPlayer = true;
		other.GetComponent< Rigidbody2D >( ).velocity = Vector3.zero;
	}

	public void reset( ) {
		EnterPlayer = false;
	}
}