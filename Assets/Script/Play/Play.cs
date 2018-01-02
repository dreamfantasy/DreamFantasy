using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Play : MonoBehaviour {
	public enum BOARDOBJECT {
		BG,
		PLAYER,
		GOAL,
		SWITCH,
		WALL,
	};
	public enum STATE {
		WAIT,
		PLAY,
		STAGE_CLEAR,
		GAME_OVER,
		TUTORIAL
	};
	//--------Inspecter定数------//
	[ Range( 0, 300 ) ]
	public int START_WAIT_COUNT = 60;

	//-----------定数------------//
	public const int MAX_STAGE = 9;
	public const int MAX_AREA  = 3;
	public const int MAX_STOCK = 3;

	//-----------変数------------//
	public STATE state { get; private set; }
	GameObject[ ] _stock_ui;
	GameObject _clear_ui;
	GameObject _area_txt_ui;
	GameObject _col_effect;
	PlayData _data;
	Action act;
	int _count = 0;
	int _area = -1;
	int _stock = MAX_STOCK;
	//Tutorial
	//int _tutorial_phase = 0;
	//int _tutorial_line = 0;
	//List< string[ ] > _tutorial_txt = new List< string[ ] >( );
	


	//-----------関数------------//

	void Awake( ) {
		setRetireButton( );
		createLimitMoveWall( );
		loadAreaData( );
		loadColEffect( );
		findStockUI( );
		findGameClearUI( );
		findAreaTxtUI( );
	}

	void Start( ) {
		setState( STATE.WAIT );
	}

	void Update( ) {
		act( );
		_count++;
	}


	//-----------------------Act---------------------//
	void setState( STATE value ) {
		switch ( state ) {
		case STATE.WAIT:
			_area_txt_ui.SetActive( false );
			break;
		case STATE.PLAY:
			break;
		case STATE.STAGE_CLEAR:
			_clear_ui.SetActive( false );
			break;
		case STATE.GAME_OVER:
			break;
		case STATE.TUTORIAL:
			break;
		}

		state = value;
		_count = 0;

		switch ( state ) {
		case STATE.WAIT:
			act = actOnWait;
			_area_txt_ui.SetActive( true );
			_area_txt_ui.GetComponent< Text >( ).text = getAreaString( );
			break;
		case STATE.PLAY:
			act = actOnPlay;
			break;
		case STATE.STAGE_CLEAR:
			act = actONStageClear;
			_clear_ui.SetActive( true );
			break;
		case STATE.GAME_OVER:
			act = actOnGameOver;
			break;
		case STATE.TUTORIAL:
			act = actOnTutorial;
			break;
		}
	}
	
	void actOnWait( ) {
		if ( _count > START_WAIT_COUNT ) {
			if ( Device.Instanse.Phase == Device.PHASE.ENDED ) {
				//プレイヤーが操作できる状態に以降
				setState( STATE.PLAY );
			}
		}
	}

	void actOnPlay( ) {
		//プレイヤーが死亡
		if ( _data.player.GetComponent< Player >( ).isFinished( ) ) {
			if ( reStart( ) ) {
				setState( STATE.WAIT );
			} else {
				setState( STATE.GAME_OVER );
			}
		}
		//プレイヤーがゴールに行った
		if ( _data.goal.GetComponent< Goal >( ).EnterPlayer ) {
			setState( STATE.STAGE_CLEAR );
		}
	}

	void actONStageClear( ) {
		//次のエリアを読み込み
		if ( Device.Instanse.Phase == Device.PHASE.ENDED ) {
			if ( loadAreaData( ) ) {
				setState( STATE.WAIT );
			} else {
				//クリア
				if ( !Game.Instance.tutorial ) {
					if ( Game.Instance.clear_stage < Game.Instance.stage ) {
						Game.Instance.clear_stage = Game.Instance.stage;
					}
				}
				Game.Instance.tutorial = false;
				Game.Instance.loadScene( Game.SCENE.SCENE_STAGESELECT );
			}
		}
	}

	void actOnGameOver( ) {
		if ( Device.Instanse.Phase == Device.PHASE.ENDED ) {
			Game.Instance.loadScene( Game.SCENE.SCENE_STAGESELECT );
		}
	}

	void actOnTutorial( ) {
	}

	//----------------------/Act---------------------//
	//--------------------Tutorial-------------------//
	//-------------------/Tutorial-------------------//

	bool reStart( ) {
		_stock--;
		if ( _stock < 0 ) {
			return false;
		}
		for ( int i = 0; i < MAX_STOCK; i++ ) {
			if ( _stock <= i ) {
				_stock_ui[ i ].SetActive( false );
			}
		}
		_data.reset( );
		return true;
	}

	void retire( ) {
		Game.Instance.loadScene( Game.SCENE.SCENE_STAGESELECT );
	}

	string getAreaString( ) {
		return "Area " + ( _area + 1 ).ToString( ) + "/" + MAX_AREA.ToString( );
	}

	//----------------初期化系統----------------//

	bool loadAreaData( ) {
		_area++;
		destroyArea( );
		if ( _area >= MAX_AREA ) {
			return false;
		}
		_data = new PlayData( );
		string path = getDataPath( Game.Instance.stage, _area );
		if ( Game.Instance.tutorial ) {
			path = getDataTutorialPath( _area );
		}
		BoardData data = ( BoardData )Resources.Load( path );
		if ( data == null ) {
			print( "エリアのAssetが存在しません。" );
			Application.Quit( );
			return false;
		}
		//----描画順に読み込む----//
		//Bg( 代入しない )
		data.createBg( );
		//Goal
		_data.goal = data.createGoal( );
		//Wall & WallMove
		_data.walls = data.createWalls( );
		//Switch
		_data.switchs = data.createSwitchs( );
		//Player
		_data.player = data.createPlayer( );


		_data.setActives( true );
		return true;
	}

	void destroyArea( ) {
		if ( _data == null ) {
			return;
		}
		//Player
		if ( _data.player != null ) {
			Destroy( _data.player );
		}
		//Goal
		if ( _data.goal != null ) {
			Destroy( _data.goal );
		}
		//Wall / move
		foreach ( GameObject obj in _data.walls ) {
			Destroy( obj );
		}
		//Switch
		foreach ( GameObject obj in _data.switchs ) {
			Destroy( obj );
		}
	}

	void createLimitMoveWall( ) {
		GameObject prefab = Resources.Load< GameObject >( getPrefabDir( ) + "/Other/BoxCol" );
		{//上
			GameObject obj = Instantiate( prefab );
			Transform trans = obj.GetComponent< Transform >( );
			trans.position = Vector3.up * 880;
			trans.localScale = Vector3.up * 200 + Vector3.right * 1080 + Vector3.forward * 1;
		}
		{//下
			GameObject obj = Instantiate( prefab );
			Destroy( obj.GetComponent< MeshCollider >( ) );
			Transform trans = obj.GetComponent< Transform >( );
			trans.position = Vector3.down * 1060;
			trans.localScale = Vector3.up * 200 + Vector3.right * 1080 + Vector3.forward * 1;
		}
		{//左
			GameObject obj = Instantiate( prefab );
			Destroy( obj.GetComponent< MeshCollider >( ) );
			Transform trans = obj.GetComponent< Transform >( );
			trans.position = Vector3.left * 640;
			trans.localScale = Vector3.up * 1920 + Vector3.right * 200 + Vector3.forward * 1;
		}
		{//右
			GameObject obj = Instantiate( prefab );
			Destroy( obj.GetComponent< MeshCollider >( ) );
			Transform trans = obj.GetComponent< Transform >( );
			trans.position = Vector3.right * 640;
			trans.localScale = Vector3.up * 1920 + Vector3.right * 200 + Vector3.forward * 1;
		}
	}

	void loadColEffect( ) {
		_col_effect = Instantiate( Resources.Load< GameObject >( "Play/Prefab/Effect/Col" ) );
	}

	void findStockUI( ) {	
		_stock_ui = new GameObject[ MAX_STOCK ];
		for ( int i = 0; i < MAX_STOCK; i++ ) {
			string path = "UI/Stock" + i.ToString( );
			_stock_ui[ i ] = GameObject.Find( path );
		}
	}
	void findGameClearUI( ) {
		_clear_ui = GameObject.Find( "UI" ).transform.Find( "GameClear" ).gameObject;
		_clear_ui.SetActive( false );
	}

	void findAreaTxtUI( ) {
		_area_txt_ui = GameObject.Find( "UI" ).transform.Find( "AreaTxt" ).gameObject;
		_area_txt_ui.SetActive( false );
	}

	void setRetireButton( ) {
		Button button = GameObject.Find( "UI" ).transform.Find( "Retire" ).GetComponent< Button >( );
		button.onClick.AddListener( retire );
	}
	//------------------------------------------------//

	
	//----------------public static-------------------//

	public static string getTag( BOARDOBJECT type ) {
		string tag = "";
		switch ( type ) {
		case BOARDOBJECT.BG:
			tag = "Bg";
			break;
		case BOARDOBJECT.PLAYER:
			tag = "Player";
			break;
		case BOARDOBJECT.GOAL:
			tag = "Goal";
			break;
		case BOARDOBJECT.SWITCH:
			tag = "Switch";
			break;
		case BOARDOBJECT.WALL:
			tag = "Wall";
			break;
		}
		return tag;
	}

	public static string getPrefabDir( ) {
		return "Play/Prefab";
	}

	public static string getPrefabPath( BOARDOBJECT type ) {
		string path = getPrefabDir( ) + "/";
		switch ( type ) {
		case BOARDOBJECT.BG:
			path += "Bg";
			break;
		case BOARDOBJECT.PLAYER:
			path += "Player";
			break;
		case BOARDOBJECT.GOAL:
			path += "Goal";
			break;
		case BOARDOBJECT.SWITCH:
			path += "Switch";
			break;
		case BOARDOBJECT.WALL:
			path += "Wall";
			break;
		}
		return path;
	}

	public static string getDataDir( int stage ) {
		return "Play/Data/Board/0/" + stage.ToString( );
	}

	public static string getDataPath( int stage, int area ) {
		return  getDataDir( stage ) + "/" + area.ToString( );
	}

	public static string getDataTutorialDir( ) {
		return "Play/Data/Board/Tutorial/";
	}

	public static string getDataTutorialPath( int area ) {
		return  getDataTutorialDir( ) + area.ToString( );
	}
	//-------------/Public static-------------------------//
	//-----------------Effect-------------------------//

	public void startEffect( Vector3 pos ) {
		_col_effect.transform.position = pos;
		_col_effect.GetComponent< ParticleSystem >( ).Play( );
	}


	//------------------------------------------------//
}
