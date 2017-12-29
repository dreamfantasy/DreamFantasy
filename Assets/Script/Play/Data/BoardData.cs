using UnityEngine;
using System.Collections.Generic;

public class BoardData : ScriptableObject { 
    public PlayerData _Player;
    public GoalData   _Goal  ;
    public List< WallData     > _Wall     = new List< WallData     >( );
    public List< WallMoveData > _WallMove = new List< WallMoveData >( );
    public List< SwitchData   > _Switch   = new List< SwitchData   >( );

	void OnEnable( ) {
		serchBoardObjects( );
	}
	public void serchBoardObjects( ) {
		Debug.Log( "serch" );
		serchPlayer( );
		serchGoal( );
		serchWall( );
		serchWallMove( );
		serchSwitch( );
	}

	void serchPlayer( ) {
		string tag = Play.getTag( Play.BOARDOBJECT.PLAYER );
		Transform trans =  GameObject.FindGameObjectWithTag( tag ).GetComponent< Transform >( );
		if ( trans == null ) {
			_Player = null;
			return;
		}
		if ( _Player == null ) {
			_Player = new PlayerData( );
		}
		copyTransform( trans, _Player.trans );
	}
	
	void serchGoal( ) {
		string tag = Play.getTag( Play.BOARDOBJECT.GOAL );
		Transform trans =  GameObject.FindGameObjectWithTag( tag ).GetComponent< Transform >( );
		if ( !trans ) {
			_Goal = null;
			return;
		}
		if ( _Goal == null ) {
			_Goal = new GoalData( );
		}
		copyTransform( trans, _Goal.trans );
	}

	void serchWall( ) {
		string tag = Play.getTag( Play.BOARDOBJECT.WALL );
		_Wall.Clear( );
		GameObject[ ] objects = GameObject.FindGameObjectsWithTag( tag );
		foreach( GameObject obj in objects ) {
			addWall( obj );
		}
	}

	void serchWallMove( ) {
		_WallMove.Clear( );
		string tag = Play.getTag( Play.BOARDOBJECT.WALL_MOVE );
		GameObject[ ] objects = GameObject.FindGameObjectsWithTag( tag );
		foreach( GameObject obj in objects ) {
			addWallMove( obj );
		}
	}

	void serchSwitch( ) {
		_Switch.Clear( );
		string tag = Play.getTag( Play.BOARDOBJECT.SWITCH );
		GameObject[ ] objects = GameObject.FindGameObjectsWithTag( tag );
		foreach( GameObject obj in objects ) {
			addSwitch( obj );
		}
	}

	void addWall( GameObject obj ) {
		WallData add = new WallData( );
		//trans
		Transform trans =  obj.GetComponent< Transform >( );
		copyTransform( trans, add.trans );
		//option(なし)
		//追加
		_Wall.Add( add );
	}
	
	void addWallMove( GameObject obj ) {
		WallMoveData add = new WallMoveData( );
		//trans
		Transform trans =  obj.GetComponent< Transform >( );
		copyTransform( trans, add.trans );

		//option
		WallMove script = obj.GetComponent< WallMove >( );
		add.option = script.option;
		//追加
		_WallMove.Add( add );
	}

	void addSwitch( GameObject obj ) {
		//trans
		Transform trans =  obj.GetComponent< Transform >( );
		SwitchData tmp = new SwitchData( );
		copyTransform( trans, tmp.trans );
		//option(なし)
		//追加
		_Switch.Add( tmp );
	}

	public static void copyTransform( Transform from, Transform to ) {
		to.transform.position   = from.position;
		to.transform.rotation   = from.rotation;
		to.transform.localScale = from.localScale;
	}

	public GameObject createPlayer( ) {
		if ( _Player == null ) {
			return null;
		}
		GameObject prefab = Resources.Load< GameObject >( Play.getPrefabPath( Play.BOARDOBJECT.PLAYER ) );
		GameObject obj = Instantiate( prefab );
		copyTransform( _Player.trans,  obj.GetComponent< Transform >( ) );
		return obj;
	}

	public GameObject createGoal( ) {
		if ( _Goal == null ) {
			return null;
		}
		GameObject prefab = Resources.Load< GameObject >( Play.getPrefabPath( Play.BOARDOBJECT.GOAL ) );
		GameObject obj = Instantiate( prefab );
		copyTransform( _Goal.trans,  obj.GetComponent< Transform >( ) );
		return obj;
	}

	public GameObject[ ] createWalls( ) {
		GameObject[ ] result = new GameObject[ _Wall.Count ];
		GameObject prefab = Resources.Load< GameObject >( Play.getPrefabPath( Play.BOARDOBJECT.WALL ) );
		for ( int i = 0; i < _Wall.Count; i++ ) {
			result[ i ] = Instantiate( prefab );
			copyTransform( _Wall[ i ].trans, result[ i ].GetComponent< Transform >( ) );
		}
		return result;
	}

	public GameObject[ ] createWallMoves( ) {
		GameObject[ ] result = new GameObject[ _WallMove.Count ];
		GameObject prefab = Resources.Load< GameObject >( Play.getPrefabPath( Play.BOARDOBJECT.WALL_MOVE ) );
		for ( int i = 0; i < _WallMove.Count; i++ ) {
			result[ i ] = Instantiate( prefab );
			copyTransform( _WallMove[ i ].trans, result[ i ].GetComponent< Transform >( ) );
			result[ i ].GetComponent< WallMove >( ).option = _WallMove[ i ].option;
		}
		return result;
	}

	public GameObject[ ] createSwitchs( ) {
		GameObject[ ] result = new GameObject[ _Switch.Count ];
		GameObject prefab = Resources.Load< GameObject >( Play.getPrefabPath( Play.BOARDOBJECT.SWITCH ) );
		for ( int i = 0; i < _Switch.Count; i++ ) {
			result[ i ] = Instantiate( prefab );
			copyTransform( _Switch[ i ].trans, result[ i ].GetComponent< Transform >( ) );
		}
		return result;
	}

	public class BoardObject {
		public Transform trans;
	}
	
	public class PlayerData   : BoardObject {
	}

	public class GoalData     : BoardObject {
	}

	public class WallData     : BoardObject {
	}

	public class WallMoveData : BoardObject {
		public WallMove.Option option;
	}

	public class SwitchData   : BoardObject {
	}
}