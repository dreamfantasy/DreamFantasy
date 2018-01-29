using UnityEngine;
public class Goal : MonoBehaviour {
	public bool EnterPlayer { get; private set; }
	bool enable = true;
	Switch[ ] switchs;
	Color color_on  = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
	Color color_off = new Color( 1.0f, 1.0f, 1.0f, 0.3f );

	void Awake( ) {
		setEnable( false );
		addCollider( );
	}

	void Start( ) {
		serchSwitch( );
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
		if ( value ) {
			gameObject.GetComponent< SpriteRenderer >( ).color = color_on;
		} else {
			gameObject.GetComponent< SpriteRenderer >( ).color = color_off;
		}
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

	void addCollider( ) {
		CircleCollider2D col = gameObject.AddComponent< CircleCollider2D >( );
		col.offset = Vector2.down * 20;
		col.radius = 50;
		col.isTrigger = true;
	}

	public void reset( ) {
		EnterPlayer = false;
		setEnable( false );
	}

	void serchSwitch( ) {
		GameObject[ ] objects = GameObject.FindGameObjectsWithTag( Play.getTag( Play.BOARDOBJECT.SWITCH ) );
		switchs = new Switch[ objects.Length ];
		for ( int i = 0; i < objects.Length; i++ ) {
			switchs[ i ] = objects[ i ].GetComponent< Switch >( );
		}
	}
}