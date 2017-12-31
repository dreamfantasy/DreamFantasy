using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {
	public bool enable { get; private set; }
	private Color color_on  = new Color( 0.5f, 1, 1 );
	private Color color_off = new Color( 1, 0.4f, 0.4f );

	void Awake( ) {
		addCollider( );
	}

	void Start( ) {
		setEnable( true );	 
	}

	public void reset( ) {
		setEnable( true );
	}

	void OnTriggerEnter2D( Collider2D other ) {
		if ( other.tag != "Player" ) {
			return;
		}
		setEnable( false );
	}

	void setEnable( bool value ) {
		enable = value;
		if ( enable ) {
			GetComponent< SpriteRenderer >( ).color = color_on;
		} else {
			GetComponent< SpriteRenderer >( ).color = color_off;
		}
	}

	void addCollider( ) {
		CircleCollider2D col = gameObject.AddComponent< CircleCollider2D >( );
		col.isTrigger = true;
	}
}
