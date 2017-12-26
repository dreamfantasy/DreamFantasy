using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Title : MonoBehaviour {
	//----inspecter-----//
	public double ALPHA_SPEED = 0.03;
	public int MOVIE_COUNT = 300;
	//------------------//
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
	}

	void Start( ) {
	}

	void Update( ) {
		float alpha = ( float )Math.Abs( Math.Sin( _count * ALPHA_SPEED ) );
		_touch.color = new Color( 1, 1, 1, alpha );

		if ( _movie_tex.isPlaying ) {
			//タッチで動画を止める
			if ( Device.getTouchPhase( ) == Device.PHASE.ENDED ) {
				stopMovie( );
			}
		} else {
			//一定時間放置でムービー再生
			if ( _count > MOVIE_COUNT ) {
				playMovie( );
			}
			//タッチで次のシーン
			if ( Device.getTouchPhase( ) == Device.PHASE.ENDED ) {
				Game.loadScene( Game.SCENE.SCENE_STAGESELECT );
			}
		}
		_count++;
	}

	void playMovie( ) {
		_movie.SetActive( true );
		_movie_tex.Play( );
	}

	void stopMovie( ) {
		_count = 0;
		_movie_tex.Stop( );
		_movie.SetActive( false );
	}
}
