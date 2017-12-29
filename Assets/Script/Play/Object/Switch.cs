using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {
	public bool enable { get; private set; }

	void Awake( ) {
		enable = true;
		CircleCollider2D col = gameObject.AddComponent< CircleCollider2D >( );
		col.isTrigger = true;
	}

	public void reset( ) {
		enable = false;
		GetComponent< SpriteRenderer >( ).color = new Color( 1, 1, 1 );
	}

	void OnTriggerEnter2D( Collider2D other ) {
		if ( other.tag != "Player" ) {
			return;
		}
		enable = false;
		GetComponent< SpriteRenderer >( ).color = new Color( 1, 0.4f, 0.4f );
	}
}
