using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {
	public enum SCENE {
		SCENE_TITLE,
		SCENE_STAGESELECT,
		SCENE_SCENARIO,
		SCENE_PLAY,
		MAX_SCENE
	};
	public int chapter     { get; set; }
	public int stage       { get; set; }
	public int stock       { get; set; }
	public int clear_stage { get; set; }
	public bool tutorial   { get; set; }
	public static Game Instance { get; private set; }
	const int MAX_LOAD_STAGE = 5;
	string[ ] KEY = new string[ MAX_LOAD_STAGE ] {
		"0",
		"1",
		"2",
		"3",
		"4"
	};
	//----------関数------------//
	void Awake( ) {	
		reset( );
		Instance = this;
	}
	void Start ( ) { }

	void Update( ) {
		//debugキー
		for ( int i = 0; i < MAX_LOAD_STAGE; i++ ) {
			if ( Input.GetKeyDown( KEY[ i ] ) ) {
				tutorial = false;
				stage = i;
				loadScene( SCENE.SCENE_PLAY );
			}
		}
	}

	public void loadScene( SCENE scene ) {
		Device.Instanse.StopLittle( 30 );
		switch ( scene ) {
		case SCENE.SCENE_TITLE:
			SceneManager.LoadScene( "Title" );
			break;
		case SCENE.SCENE_STAGESELECT:
			SceneManager.LoadScene( "StageSelect" );
			break;
		case SCENE.SCENE_SCENARIO:
			SceneManager.LoadScene( "Scenario" );
			break;
		case SCENE.SCENE_PLAY:
			SceneManager.LoadScene( "Play" );
			break;
		}
	}

	public void reset( ) {
		stock = 0;
		stage = 0;
		chapter = 0;
		clear_stage = -1;
		tutorial = true; 
	}
}
