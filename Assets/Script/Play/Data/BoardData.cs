using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class BoardData : ScriptableObject {
	public Sprite _Bg;
	[SerializeField]
    public PlayerData _Player;
	[SerializeField]
    public GoalData   _Goal  ;
	[SerializeField]
    public List< WallData     > _Wall     = new List< WallData     >( );
	[SerializeField]
    public List< WallMoveData > _WallMove = new List< WallMoveData >( );
	[SerializeField]
    public List< SwitchData   > _Switch   = new List< SwitchData   >( );
	
	public void serchBoardObjects( ) {
		Debug.Log( "Assetを更新しました" );
		serchBg( );
		serchPlayer( );
		serchGoal( );
		serchWall( );
		serchWallMove( );
		serchSwitch( );
	}

	void serchBg( ) {
		string tag = Play.getTag( Play.BOARDOBJECT.BG );
		GameObject obj =  GameObject.FindGameObjectWithTag( tag );
		if ( obj == null ) {
			_Bg = null;
			return;
		}
		_Bg = obj.GetComponent< SpriteRenderer >( ).sprite;
	}

	void serchPlayer( ) {
		string tag = Play.getTag( Play.BOARDOBJECT.PLAYER );
		GameObject obj = GameObject.FindGameObjectWithTag( tag );
		if ( obj == null ) {
			_Player = null;
			return;
		}
		if ( _Player == null ) {
			_Player = new PlayerData( );
		}
		copyTransform( obj.GetComponent< Transform >( ), _Player );
	}
	
	void serchGoal( ) {
		string tag = Play.getTag( Play.BOARDOBJECT.GOAL );
		GameObject obj = GameObject.FindGameObjectWithTag( tag );
		if ( obj == null ) {
			_Goal = null;
			return;
		}
		if ( _Goal == null ) {
			_Goal = new GoalData( );
		}
		copyTransform( obj.GetComponent< Transform >( ), _Goal );
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
		SpriteRenderer compornent0 = obj.GetComponent< SpriteRenderer >( );
		add.size = compornent0.size;
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
		SpriteRenderer compornent0 = obj.GetComponent< SpriteRenderer >( );
		add.size = compornent0.size;

		WallMove compornent1 = obj.GetComponent< WallMove >( );
		add.option = compornent1.option;

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

	public GameObject createBg( ) {
		if ( _Bg == null ) {
			return null;
		}
		GameObject prefab = Resources.Load< GameObject >( Play.getPrefabPath( Play.BOARDOBJECT.BG ) );
		GameObject obj = Instantiate( prefab );
		obj.GetComponent< SpriteRenderer >( ).sprite = _Bg;
		return obj;
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
			copyTransform( _Wall[ i ], result[ i ].GetComponent< Transform >( ) );  //Transform
			result[ i ].GetComponent< SpriteRenderer >( ).size = _Wall[ i ].size;   //Size
		}
		return result;
	}

	public GameObject[ ] createWallMoves( ) {
		GameObject[ ] result = new GameObject[ _WallMove.Count ];
		GameObject prefab = Resources.Load< GameObject >( Play.getPrefabPath( Play.BOARDOBJECT.WALL_MOVE ) );
		for ( int i = 0; i < _WallMove.Count; i++ ) {
			result[ i ] = Instantiate( prefab );
			copyTransform( _WallMove[ i ], result[ i ].GetComponent< Transform >( ) );//Transform
			result[ i ].GetComponent< WallMove >( ).option = _WallMove[ i ].option;   //Option
			result[ i ].GetComponent< SpriteRenderer >( ).size = _WallMove[ i ].size; //Size
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

	[System.Serializable]
	public class Trans  {
		public Vector3 pos = new Vector3( );
		public Quaternion rot = new Quaternion( );
		public Vector3 scl = new Vector3( );
	}
	
	[System.Serializable]
	public class PlayerData   : Trans {
	}

	[System.Serializable]
	public class GoalData     : Trans {
	}

	[System.Serializable]
	public class WallData     : Trans {
		public Vector2 size;
	}

	[System.Serializable]
	public class WallMoveData : WallData {
		public WallMove.Option option;
	}

	[System.Serializable]
	public class SwitchData   : Trans {
	}
}