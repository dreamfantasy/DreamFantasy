using UnityEngine;
using System.Collections.Generic;

public class BoardData : ScriptableObject { 
    public PlayerData _Player;
    public GoalData   _Goal  ;
    public List< WallData     > _Wall     = new List< WallData     >( );
    public List< WallMoveData > _WallMove = new List< WallMoveData >( );
    public List< SwitchData   > _Switch   = new List< SwitchData   >( );
	
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
		copyTransform( trans, _Player );
	}
	
	void serchGoal( ) {
		string tag = Play.getTag( Play.BOARDOBJECT.GOAL );
		Transform trans =  GameObject.FindGameObjectWithTag( tag ).GetComponent< Transform >( );
		if ( trans == null ) {
			_Goal = null;
			return;
		}
		if ( _Goal == null ) {
			_Goal = new GoalData( );
		}
		copyTransform( trans, _Goal );
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
		if ( trans == null ) {
			Debug.Log( "wall Trans : null" );
			return;
		}
		copyTransform( trans, add );
		//option(なし)
		//追加
		_Wall.Add( add );
	}
	
	void addWallMove( GameObject obj ) {
		WallMoveData add = new WallMoveData( );
		//trans
		Transform trans =  obj.GetComponent< Transform >( );
		if ( trans == null ) {
			Debug.Log( "wall move Trans : null" );
			return;
		}
		copyTransform( trans, add );

		//option
		WallMove script = obj.GetComponent< WallMove >( );
		add.option = script.option;
		//追加
		_WallMove.Add( add );
	}

	void addSwitch( GameObject obj ) {
		SwitchData add = new SwitchData( );
		//trans
		Transform trans =  obj.GetComponent< Transform >( );
		if ( trans == null ) {
			Debug.Log( "Switch Trans : null" );
			return;
		}

		copyTransform( trans, add );
		//option(なし)
		//追加
		_Switch.Add( add );
	}

	public static void copyTransform( Trans from, Transform to ) {
		to.transform.position   = from.pos;
		to.transform.rotation   = from.rot;
		to.transform.localScale = from.scl;
	}

	public static void copyTransform( Transform from, Trans to ) {
		to.pos = from.position;
		to.rot = from.rotation;
		to.scl = from.localScale;
	}

	public GameObject createPlayer( ) {
		if ( _Player == null ) {
			return null;
		}
		GameObject prefab = Resources.Load< GameObject >( Play.getPrefabPath( Play.BOARDOBJECT.PLAYER ) );
		GameObject obj = Instantiate( prefab );
		copyTransform( _Player,  obj.GetComponent< Transform >( ) );
		return obj;
	}

	public GameObject createGoal( ) {
		if ( _Goal == null ) {
			return null;
		}
		GameObject prefab = Resources.Load< GameObject >( Play.getPrefabPath( Play.BOARDOBJECT.GOAL ) );
		GameObject obj = Instantiate( prefab );
		copyTransform( _Goal,  obj.GetComponent< Transform >( ) );
		return obj;
	}

	public GameObject[ ] createWalls( ) {
		GameObject[ ] result = new GameObject[ _Wall.Count ];
		GameObject prefab = Resources.Load< GameObject >( Play.getPrefabPath( Play.BOARDOBJECT.WALL ) );
		for ( int i = 0; i < _Wall.Count; i++ ) {
			result[ i ] = Instantiate( prefab );
			copyTransform( _Wall[ i ], result[ i ].GetComponent< Transform >( ) );
		}
		return result;
	}

	public GameObject[ ] createWallMoves( ) {
		GameObject[ ] result = new GameObject[ _WallMove.Count ];
		GameObject prefab = Resources.Load< GameObject >( Play.getPrefabPath( Play.BOARDOBJECT.WALL_MOVE ) );
		for ( int i = 0; i < _WallMove.Count; i++ ) {
			result[ i ] = Instantiate( prefab );
			copyTransform( _WallMove[ i ], result[ i ].GetComponent< Transform >( ) );
			result[ i ].GetComponent< WallMove >( ).option = _WallMove[ i ].option;
		}
		return result;
	}

	public GameObject[ ] createSwitchs( ) {
		GameObject[ ] result = new GameObject[ _Switch.Count ];
		GameObject prefab = Resources.Load< GameObject >( Play.getPrefabPath( Play.BOARDOBJECT.SWITCH ) );
		for ( int i = 0; i < _Switch.Count; i++ ) {
			result[ i ] = Instantiate( prefab );
			copyTransform( _Switch[ i ], result[ i ].GetComponent< Transform >( ) );
		}
		return result;
	}

	public class Trans {
		public Vector3 pos = new Vector3( );
		public Quaternion rot = new Quaternion( );
		public Vector3 scl = new Vector3( );
	}
	
	public class PlayerData   : Trans {
	}

	public class GoalData     : Trans {
	}

	public class WallData     : Trans {
	}

	public class WallMoveData : Trans {
		public WallMove.Option option;
	}

	public class SwitchData   : Trans {
	}
}