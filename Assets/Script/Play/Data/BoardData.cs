using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class BoardData : ScriptableObject {
	public Sprite _Bg;
	[SerializeField]
    public PlayerData _Player;
	[SerializeField]
    public GoalData   _Goal  ;
	[SerializeField ]
    public List< WallData     > _Wall     = new List< WallData     >( );
	[SerializeField]
    public List< SwitchData   > _Switch   = new List< SwitchData   >( );
	
	public void serchBoardObjects( ) {
		Debug.Log( "Assetを更新しました" );
		serchBg( );
		serchPlayer( );
		serchGoal( );
		serchWall( );
		serchSwitch( );
	}

	//------------------Serch-------------------//
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
		copy( obj, _Player );
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
		copy( obj, _Goal );
	}

	void serchWall( ) {
		string tag = Play.getTag( Play.BOARDOBJECT.WALL );
		_Wall.Clear( );
		GameObject[ ] objects = GameObject.FindGameObjectsWithTag( tag );
		foreach( GameObject obj in objects ) {
			addWall( obj );
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
	//-----------------/Serch-------------------//

	//------------------Add---------------------//
	void addWall( GameObject obj ) {
		//Null Check
		if ( obj == null ) {
			Debug.Log( "wall Trans : null" );
			return;
		}
		//継承確認
		if ( obj.GetComponent< WallMove >( ) != null ) {
			WallMoveData add = new WallMoveData( );
			copy( obj, add );
			_Wall.Add( add );

		} else {
			WallData add = new WallData( );
			copy( obj, add );
			_Wall.Add( add );
		}
	}
	
	void addWallMove( GameObject obj ) {
		WallMoveData add = new WallMoveData( );
		//trans
		Transform trans =  obj.GetComponent< Transform >( );
		if ( trans == null ) {
			Debug.Log( "wall move Trans : null" );
			return;
		}
		copy( obj, add );


		//option
		SpriteRenderer compornent0 = obj.GetComponent< SpriteRenderer >( );
		add.size = compornent0.size;

		WallMove compornent1 = obj.GetComponent< WallMove >( );
		add.option = compornent1.option;

		//追加
		_Wall.Add( add );
	}

	void addSwitch( GameObject obj ) {
		SwitchData add = new SwitchData( );
		if ( obj == null ) {
			Debug.Log( "Switch : null" );
			return;
		}

		copy( obj, add );
		//option(なし)
		//追加
		_Switch.Add( add );
	}
	//-----------------/Add---------------------//

	//------------------Copy---------------------//
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
		to.rot = from.GetComponent< Transform >( ).rotation;  //rot
		SpriteRenderer sr = from.GetComponent< SpriteRenderer >( );
		to.size   = sr.size;  //Size
		to.sprite = sr.sprite;//Sprite
	}
	public static void copy( WallData from, GameObject to ) {
		copy( ( CommonData )from, to );
		to.GetComponent< Transform >( ).rotation = from.rot;  //rot
		SpriteRenderer sr = to.GetComponent< SpriteRenderer >( );
		sr.size   = from.size;   //Size
		sr.sprite = from.sprite; //Sprite
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
		copy( _Player,  obj );
		return obj;
	}

	public GameObject createGoal( ) {
		if ( _Goal == null ) {
			return null;
		}
		GameObject prefab = Resources.Load< GameObject >( Play.getPrefabPath( Play.BOARDOBJECT.GOAL ) );
		GameObject obj = Instantiate( prefab );
		copy( _Goal,  obj );
		return obj;
	}

	public GameObject[ ] createWalls( ) {
		GameObject[ ] result = new GameObject[ _Wall.Count ];
		GameObject prefab = Resources.Load< GameObject >( Play.getPrefabPath( Play.BOARDOBJECT.WALL ) );
		for ( int i = 0; i < _Wall.Count; i++ ) {
			result[ i ] = Instantiate( prefab );
			//継承チェック
			if ( _Wall[ i ] is WallMoveData ) {
				if ( Application.isPlaying ) {
					Destroy( result[ i ].GetComponent< Wall >( ) );
				} else {
					DestroyImmediate( result[ i ].GetComponent< Wall >( ) );
				}
				result[ i ].AddComponent< WallMove >( );
				copy( _Wall[ i ] as WallMoveData, result[ i ] );
			} else {
				copy( _Wall[ i ], result[ i ] );
			}
		}
		return result;
	}

	public GameObject[ ] createSwitchs( ) {
		GameObject[ ] result = new GameObject[ _Switch.Count ];
		GameObject prefab = Resources.Load< GameObject >( Play.getPrefabPath( Play.BOARDOBJECT.SWITCH ) );
		for ( int i = 0; i < _Switch.Count; i++ ) {
			result[ i ] = Instantiate( prefab );
			copy( _Switch[ i ], result[ i ] );
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
		public Sprite sprite;
	}

	[System.Serializable]
	public class SwitchData : CommonData {
	}

	//-----------------継承-----------------//
	[System.Serializable]
	public class WallMoveData : WallData {
		[SerializeField]
		public WallMove.Option option;
	}

}