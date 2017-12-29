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
	public const int MAX_STAGE = 9;
	public const int MAX_AREA  = 3;
	PlayData[ ] _data;

	void Awake( ) {
		loadStageData( );
	}

	void Start( ) {
	}

	void Update( ) {
		
	}

	void loadStageData( ) {
		_data = new PlayData[ MAX_AREA ];
		for ( int i = 0; i < MAX_AREA; i++ ) {
			loadAreaData( i );
		}
	}

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

	public static string getPrefabPath( BOARDOBJECT type ) {
		string path = "Play/Prefab/";
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

	void loadAreaData( int area ) {
		BoardData data = Resources.Load< BoardData >( getDataPath( Game.stage, area ) );
		if ( data == null ) {
			print( "エリアのAssetが存在しません。" );
			Application.Quit( );
			return;
		}
		//Player
		_data[ area ].Player = data.createPlayer( );
		//Goal
		_data[ area ].Goal   = data.createGoal( );
		//Wall
		foreach ( GameObject obj in data.createWalls( ) ) {
			_data[ area ].Wall.Add( obj );
		}
		//WallMove
		foreach ( GameObject obj in data.createWallMoves( ) ) {
			_data[ area ].Wall.Add( obj );
		}
		//Switch
		foreach ( GameObject obj in data.createSwitchs( ) ) {
			_data[ area ].Switch.Add( obj );
		}
		if ( area == 0 ) {
			_data[ area ].setActives( false );
		} else {
			_data[ area ].setActives( true );
		}
	}
}
