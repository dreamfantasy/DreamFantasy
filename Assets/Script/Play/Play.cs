using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play : MonoBehaviour {
	public enum BOARDOBJECT {
		PLAYER,
		GOAL,
		SWITCH,
		WALL,
		WALL_MOVE,
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
	PlayData _data;
	int _count;
	int _area;
	int _stock;
	


	//-----------関数------------//

	void Awake( ) {
		if ( !Main.instance ) {
			return;
		}
		createLimitMoveWall( );
		//loadStageData( );
		loadAreaData( 0 );
		findStockUI( );
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
			if ( Device.getTouchPhase( ) == Device.PHASE.ENDED ) {
				state = STATE.PLAY;
			}
			break;
		case STATE.PLAY:
			if ( _data._player.GetComponent< Player >( ).isFinished( ) ) {
				reStart( );
			}
			if ( _data._goal.GetComponent< Goal >( ).EnterPlayer ) {
				state = STATE.GAME_CLEAR;
			}
			break;
		case STATE.STAGE_CLEAR:
			break;
		case STATE.GAME_CLEAR:
			break;
		case STATE.GAME_OVER:
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
		_data.reset( );
	}



	//----------------初期化系統----------------//
	//void loadStageData( ) {
	//	_data = new PlayData[ MAX_AREA ];
	//	for ( int i = 0; i < MAX_AREA; i++ ) {
	//		loadAreaData( i );
	//	}
	//}

	void loadAreaData( int area ) {
		_data = new PlayData( );
		BoardData data = ( BoardData )Resources.Load( getDataPath( Game.stage, area ) );
		if ( data == null ) {
			print( "エリアのAssetが存在しません。" );
			Application.Quit( );
			return;
		}
		//Player
		_data._player = data.createPlayer( );
		//Goal
		_data._goal = data.createGoal( );
		//Wall
		foreach ( GameObject obj in data.createWalls( ) ) {
			_data._wall.Add( obj );
		}
		//WallMove
		foreach ( GameObject obj in data.createWallMoves( ) ) {
			_data._wall.Add( obj );
		}
		//Switch
		foreach ( GameObject obj in data.createSwitchs( ) ) {
			_data._switch.Add( obj );
		}

		if ( area == 0 ) {
			_data.setActives( true );
		} else {
			_data.setActives( false );
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
	//------------------------------------------------//

	
	//----------------public static-------------------//

	public static string getTag( BOARDOBJECT type ) {
		string tag = "";
		switch ( type ) {
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
		case BOARDOBJECT.WALL_MOVE:
			tag = "WallMove";
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
		case BOARDOBJECT.WALL_MOVE:
			path += "WallMove";
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

	//------------------------------------------------//
}
