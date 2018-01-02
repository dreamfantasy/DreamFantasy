using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Events;
using System;

[ExecuteInEditMode( )]
public class EffectTouch : MonoBehaviour {
	[Range( 1, 500 )]
	public int LIFE_TIME = 1;
	[Range( 0.1f, 10f )]
	public float MAX_SIZE = 2f;

	int count = 0;
	SpriteRenderer sp;
	Vector3 add_scale;
	float add_alpha = 0;
	bool _active = false;

	void Awake( ) {
		sp = gameObject.GetComponent< SpriteRenderer >( );
	}

	void Start( ) {
		_active = true;
		count = 0;
		//Scale
		add_scale = ( Vector2.right + Vector2.up ) * ( MAX_SIZE / LIFE_TIME );
		gameObject.transform.localScale = Vector3.zero;
		//Color
		add_alpha = 1.0f / LIFE_TIME;
		sp.color = new Color( 1, 1, 1, 1 );
		if ( !Application.isPlaying ) {
			EditorApplication.update += Update;
		}
	}

	void Update( ) {
		if ( !_active ) {
			return;
		}
		if ( isFinished( ) ) {
			_active = false;
			return;
		}
		//Scale
		gameObject.transform.localScale = gameObject.transform.localScale + add_scale;
		//Color
		Color color = sp.color;
		color.a = Math.Max( color.a - add_alpha, 0 );
		sp.color = color;
		count++;
	}

	bool isFinished( ) {
		return count >= LIFE_TIME;
	}

	public void touch( ) {
		Start( );
	}

	public void stop( ) {
		_active = false;
	}
}
