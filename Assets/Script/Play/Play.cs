﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
		GAME_CLEAR,
		GAME_OVER,
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
	PlayData _data;
	int _count;
	int _area;
	int _stock;
	


	//-----------関数------------//

	void Awake( ) {
		setRetireButton( );
		createLimitMoveWall( );
		loadAreaData( 0 );
		findStockUI( );
		findGameClearUI( );
		findAreaTxtUI( );
	}

	void Start( ) {
		state = STATE.WAIT;
		_area = 0;
		_count = 0;
		_stock = MAX_STOCK;
	}

	void Update( ) {
		_count++;
		switch ( state ) {
		case STATE.WAIT:
			if ( !_area_txt_ui.activeSelf ) {
				//エリア3/3を表示させる
				_area_txt_ui.SetActive( true );
				_area_txt_ui.GetComponent< Text >( ).text = getAreaString( );
			}
			if ( _count > START_WAIT_COUNT ) {
				if ( Device.Instanse.Phase == Device.PHASE.ENDED ) {
					//プレイヤーが操作できる状態に以降
					Vector2 tmp = Device.Instanse.Pos;
					_area_txt_ui.SetActive( false );
					state = STATE.PLAY;
					_count = 0;
				}
			}
			break;
		case STATE.PLAY:
			//プレイヤーが死亡
			if ( _data.player.GetComponent< Player >( ).isFinished( ) ) {
				reStart( );
			}
			//プレイヤーがゴールに行った
			if ( _data.goal.GetComponent< Goal >( ).EnterPlayer ) {
				state = STATE.STAGE_CLEAR;
				_count = 0;
			}
			break;
		case STATE.STAGE_CLEAR:
			//次のエリアを読み込み
			state = STATE.WAIT;
			_count = 0;
			_area++;
			loadAreaData( _area );
			break;
		case STATE.GAME_CLEAR:
			if ( !_clear_ui.activeSelf ) {
				_clear_ui.SetActive( true );
			}
			if ( Device.Instanse.Phase == Device.PHASE.ENDED ) {
				if ( !Game.Instance.tutorial ) {
					if ( Game.Instance.clear_stage < Game.Instance.stage ) {
						Game.Instance.clear_stage = Game.Instance.stage;
					}
				}
				Game.Instance.tutorial = false;
				Game.Instance.loadScene( Game.SCENE.SCENE_STAGESELECT );
			}
			break;
		case STATE.GAME_OVER:
			if ( Device.Instanse.Phase == Device.PHASE.ENDED ) {
				Game.Instance.loadScene( Game.SCENE.SCENE_STAGESELECT );
			}
			break;
		}
	}

	void reStart( ) {
		_stock--;
		if ( _stock < 0 ) {
			_stock = 0;
			state = STATE.GAME_OVER;
			return;
		}
		for ( int i = 0; i < MAX_STOCK; i++ ) {
			if ( _stock <= i ) {
				_stock_ui[ i ].SetActive( false );
			}
		}
		_data.reset( );
	}

	void retire( ) {
		Game.Instance.loadScene( Game.SCENE.SCENE_STAGESELECT );
	}

	string getAreaString( ) {
		return "Area " + ( _area + 1 ).ToString( ) + "/" + MAX_AREA.ToString( );
	}

	//----------------初期化系統----------------//

	void loadAreaData( int area ) {
		destroyArea( );
		if ( area >= MAX_AREA ) {
			state = STATE.GAME_CLEAR;
			return;
		}
		_data = new PlayData( );
		string path = getDataPath( Game.Instance.stage, area );
		if ( Game.Instance.tutorial ) {
			path = getDataTutorialPath( area );
		}
		BoardData data = ( BoardData )Resources.Load( path );
		if ( data == null ) {
			print( "エリアのAssetが存在しません。" );
			Application.Quit( );
			return;
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
		GameObject prefab = Resources.Load< GameObject >( getPrefabDir( ) + "/BoxCol" );
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
	//------------------------------------------------//
}
