using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Title : MonoBehaviour {
	public double ALPHA_SPEED = 0.01;
	public GameObject _movie;
	SpriteRenderer _touch;
	MovieTexture _movie_tex;
	int _count = 0;

	void Awake( ) {
		_touch = GameObject.Find( "Touch" ).GetComponent< SpriteRenderer >( );
		if ( !_touch ) {
			print( "eroor:touch is null" );
			Application.Quit( );
		}
		_movie_tex = _movie.GetComponent< MeshRenderer >( ).material.mainTexture as MovieTexture;
		if ( !_movie_tex ) {
			print( "eroor:movie_tex is null" );
			Application.Quit( );
		}
		_movie.SetActive( true );
		_movie_tex.Play( );
	}

	void Start( ) {
	}

	void Update( ) {
		_count++;
		float alpha = ( float )Math.Abs( Math.Sin( _count * ALPHA_SPEED ) );
		_touch.color = new Color( 1, 1, 1, alpha );
	}
}
