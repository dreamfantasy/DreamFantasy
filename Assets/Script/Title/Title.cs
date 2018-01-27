using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Title : MonoBehaviour {
	//----inspecter-----//
	public double ALPHA_SPEED = 0.03;
	public int MOVIE_COUNT = 300;
	//------------------//
	GameObject _bgm;
	SpriteRenderer _touch;
	int _count = 0;

	void Awake( ) {
		_touch = GameObject.Find( "Touch" ).GetComponent< SpriteRenderer >( );
		if ( !_touch ) {
			print( "eroor:touch is null" );
			Application.Quit( );
		}
		_bgm = Instantiate( Resources.Load< GameObject >( "Title/Prefab/Sound/Bgm" ) );
	}

	void Start( ) {
	}

	void Update( ) {
		checkGoNextScene( );
		updateColor( );
		_count++;
	}

	void updateColor( ) {
		_touch.color = new Color( 1, 1, 1, ( float )Math.Abs( Math.Sin( _count * ALPHA_SPEED ) ) );
	}

	void checkGoNextScene( ) {
		//タッチで次のシーン
		if ( Device.Instanse.Phase == Device.PHASE.ENDED ) {
			_bgm.GetComponent< AudioSource >( ).Stop( );
			if ( Game.Instance.tutorial ) {
				Game.Instance.loadScene( Game.SCENE.SCENE_SCENARIO );
			} else {
				Game.Instance.loadScene( Game.SCENE.SCENE_STAGESELECT );
			}
		} 
	}
}
