using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class BoardData : ScriptableObject {
	public Sprite _bg;
	[SerializeField]
    public PlayerData _player;
	[SerializeField]
    public GoalData _goal;
	//壁
	[SerializeField ]
    public List< WallData > _walls = new List< WallData >( );
	[SerializeField ]
    public List< WallMoveData > _wall_moves = new List< WallMoveData >( );
	//スイッチ
	[SerializeField]
    public List< SwitchData > _switchs = new List< SwitchData >( );
	[SerializeField]
	public GameObject[ ] _boss = new GameObject[ 0 ];
	
	public void serchBoardObjects( ) {
		Debug.Log( "Assetを更新しました" );
		serchBg( );
		serchPlayer( );
		serchGoal( );
		serchWall( );
		serchSwitch( );
		serchBoss( );
	}

	//------------------Serch-------------------//
	void serchBg( ) {
		string tag = Play.getTag( Play.BOARDOBJECT.BG );
		GameObject obj =  GameObject.FindGameObjectWithTag( tag );
		if ( obj == null ) {
			_bg = null;
			return;
		}
		_bg = obj.GetComponent< SpriteRenderer >( ).sprite;
	}

	void serchPlayer( ) {
		string tag = Play.getTag( Play.BOARDOBJECT.PLAYER );
		GameObject obj = GameObject.FindGameObjectWithTag( tag );
		if ( obj == null ) {
			_player = null;
			return;
		}
		if ( _player == null ) {
			_player = new PlayerData( );
		}
		copy( obj, _player );
	}
	
	void serchGoal( ) {
		string tag = Play.getTag( Play.BOARDOBJECT.GOAL );
		GameObject obj = GameObject.FindGameObjectWithTag( tag );
		if ( obj == null ) {
			_goal = null;
			return;
		}
		if ( _goal == null ) {
			_goal = new GoalData( );
		}
		copy( obj, _goal );
	}

	void serchWall( ) {
		string tag = Play.getTag( Play.BOARDOBJECT.WALL );
		_walls.Clear( );
		_wall_moves.Clear( );
		GameObject[ ] objects = GameObject.FindGameObjectsWithTag( tag );
		foreach( GameObject obj in objects ) {
			addWall( obj );
		}
	}

	void serchSwitch( ) {
		_switchs.Clear( );
		string tag = Play.getTag( Play.BOARDOBJECT.SWITCH );
		GameObject[ ] objects = GameObject.FindGameObjectsWithTag( tag );
		foreach( GameObject obj in objects ) {
			addSwitch( obj );
		}
	}

	void serchBoss( ) {
		_switchs.Clear( );
		string tag = Play.getTag( Play.BOARDOBJECT.SWITCH );
		_boss = GameObject.FindGameObjectsWithTag( tag );
	}
	//-----------------/Serch-------------------//

	//------------------Add---------------------//
	void addWall( GameObject obj ) {
		//Null Check
		if ( obj == null ) {
			Debug.Log( "Wall : null" );
			return;
		}
		//継承確認
		if ( obj.GetComponent< WallMove >( ) != null ) {
			//WallMove
			WallMoveData add = new WallMoveData( );
			copy( obj, add );
			_wall_moves.Add( add );

		} else {
			//Wall
			WallData add = new WallData( );
			copy( obj, add );
			_walls.Add( add );
		}
	}

	void addSwitch( GameObject obj ) {
		if ( obj == null ) {
			Debug.Log( "Switch : null" );
			return;
		}

		SwitchData add = new SwitchData( );
		copy( obj, add );

		_switchs.Add( add );
	}
	//-----------------/Add---------------------//

	//------------------Copy---------------------//
	//丸ごと
	public void copy( BoardData from ) {
		_bg		    = from._bg;
		_player	    = from._player;
		_goal       = from._goal;
		_walls      = from._walls;
		_wall_moves = from._wall_moves;
		_switchs    = from._switchs;
		_boss       = from._boss;
	}
	//-----Common----//
	public static void copy( GameObject from, CommonData to ) {
		to.pos = from.GetComponent< Transform >( ).position; //Pos
	}
	public static void copy( CommonData from, GameObject to ) {
		to.GetComponent< Transform >( ).position = from.pos; //Pos
	}
	//----/Common----//

	//-----Player----//
	public static void copy( GameObject from, PlayerData to ) {
		copy( from, ( CommonData )to );
	}
	public static void copy( PlayerData from, GameObject to ) {
		copy( ( CommonData )from, to );
	}
	//----/Player----//

	//------Goal-----//
	public static void copy( GameObject from, GoalData to ) {
		copy( from, ( CommonData )to );
	}
	public static void copy( GoalData from, GameObject to ) {
		copy( ( CommonData )from, to );
	}
	//-----/Goal-----//

	//------Wall-----//
	public static void copy( GameObject from, WallData to ) {
		copy( from, ( CommonData )to );
		//rot
		to.rot = from.GetComponent< Transform >( ).rotation;
		//Size
		to.size = from.transform.localScale;  
		//Sprite
		to.texture = from.GetComponent< MeshRenderer >( ).materials[ 0 ].mainTexture;
	}
	public static void copy( WallData from, GameObject to ) {
		copy( ( CommonData )from, to );
		//Rot
		to.GetComponent< Transform >( ).rotation = from.rot;
		//Sprite
		Material mat = new Material( Shader.Find( "Unlit/Transparent" ) );
		mat.SetTexture( "_MainTex", from.texture );
		//Size
		int texture_size = 256;
		to.transform.localScale = from.size;
		mat.SetTextureScale( "_MainTex", from.size / texture_size );

		to.GetComponent< MeshRenderer >( ).material = mat;
	}
	//-----/Wall-----//

	//-----Switch----//
	public static void copy( GameObject from, SwitchData to ) {
		copy( from, ( CommonData )to );
	}
	public static void copy( SwitchData from, GameObject to ) {
		copy( ( CommonData )from, to );
	}
	//----/Switch----//

	//------------継承-----------//
	//------Wall-----//
	public static void copy( GameObject from, WallMoveData to ) {
		copy( from, ( WallData )to );
		to.option = from.GetComponent< WallMove >( ).option;
	}
	public static void copy( WallMoveData from, GameObject to ) {
		copy( ( WallData )from, to );
		to.GetComponent< WallMove >( ).option = from.option;
	}
	//-----/Wall-----//
	//-----------/継承-----------//
	//-----------------/Copy---------------------//

	public GameObject createBg( ) {
		if ( _bg == null ) {
			return null;
		}
		GameObject prefab = Resources.Load< GameObject >( Play.getPrefabPath( Play.BOARDOBJECT.BG ) );
		GameObject obj = Instantiate( prefab );
		obj.GetComponent< SpriteRenderer >( ).sprite = _bg;
		return obj;
	}

	public GameObject createPlayer( ) {
		if ( _player == null ) {
			return null;
		}
		GameObject prefab = Resources.Load< GameObject >( Play.getPrefabPath( Play.BOARDOBJECT.PLAYER ) );
		GameObject obj = Instantiate( prefab );
		copy( _player,  obj );
		return obj;
	}

	public GameObject createGoal( ) {
		if ( _goal == null ) {
			return null;
		}
		GameObject prefab = Resources.Load< GameObject >( Play.getPrefabPath( Play.BOARDOBJECT.GOAL ) );
		GameObject obj = Instantiate( prefab );
		copy( _goal,  obj );
		return obj;
	}

	public GameObject[ ] createWalls( ) {
		//Wall & WallMove
		int size = _walls.Count + _wall_moves.Count;
		GameObject[ ] result = new GameObject[ size];

		GameObject prefab = Resources.Load< GameObject >( Play.getPrefabPath( Play.BOARDOBJECT.WALL ) );
		
		//Wall
		int idx = 0;
		for ( int i = 0; i < _walls.Count; i++ ) {
			result[ idx ] = Instantiate( prefab );
			copy( _walls[ i ], result[ i ] );
			idx++;
		}

		//WallMove
		for ( int i = 0; i < _wall_moves.Count; i++ ) {
			result[ idx ] = Instantiate( prefab );
			if ( Application.isPlaying ) {
				Destroy( result[ idx ].GetComponent< Wall >( ) );
			} else {
				DestroyImmediate( result[ idx ].GetComponent< Wall >( ) );
			}
			result[ i ].AddComponent< WallMove >( );
			copy( _wall_moves[ i ], result[ idx ] );
			idx++;
		}

		return result;
	}

	public GameObject[ ] createSwitchs( ) {
		GameObject[ ] result = new GameObject[ _switchs.Count ];
		GameObject prefab = Resources.Load< GameObject >( Play.getPrefabPath( Play.BOARDOBJECT.SWITCH ) );
		for ( int i = 0; i < _switchs.Count; i++ ) {
			result[ i ] = Instantiate( prefab );
			copy( _switchs[ i ], result[ i ] );
		}
		return result;
	}

	public GameObject[ ] createBoss( ) {
		GameObject[ ] result = new GameObject[ _boss.Length ];
		for ( int i = 0; i < _boss.Length; i++ ) {
			result[ i ] = Instantiate( _boss[ i ] );
		}
		return result;
	}


	//-----------------オブジェクト-----------------//
	[System.Serializable]
	public class CommonData {
		public Vector3 pos;
	}

	[System.Serializable]
	public class PlayerData : CommonData {
	}

	[System.Serializable]
	public class GoalData : CommonData {
	}

	[System.Serializable]
	public class WallData : CommonData {
		public Quaternion rot;
		public Vector2 size;
		public Texture texture;
	}

	[System.Serializable]
	public class SwitchData : CommonData {
	}

	//-----------------継承-----------------//
	[System.Serializable]
	public class WallMoveData : WallData{
		[SerializeField]
		public WallMove.Option option;
	}

}