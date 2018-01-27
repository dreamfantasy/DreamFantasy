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
		BOSS,
	};
	public enum STATE {
		WAIT,
		PLAY,
		STAGE_CLEAR,
		GAME_OVER,
		TUTORIAL
	};
	public enum EFFECT {
		COL,
		MAX
	};

	public enum SOUND {
		BGM,
		REF,
		GOAL,
		MAX
	}
	//--------Inspecter定数------//
	[ Range( 0, 300 ) ]
	public int START_WAIT_COUNT = 10;

	//-----------定数------------//
	public const int MAX_STAGE = 9;
	public const int MAX_AREA  = 3;
	public const int MAX_STOCK = 3;

	//-----------変数------------//
	public STATE state { get; private set; }
	GameObject[ ] _stock_ui;
	GameObject _clear_ui;
	GameObject _faild_ui;
	GameObject _area_txt_ui;
	GameObject[ ] _effect;
	GameObject[ ] _sound;
	BoardData data;
	PlayData _data;
	Action act;
	int _count = 0;
	public int area = -1;
	int _stock = MAX_STOCK;
	


	//-----------関数------------//

	void Awake( ) {
		if ( Game.Instance.tutorial ) {
			gameObject.AddComponent< PlayTutorial >( );
		}
		setRetireButton( );
		createLimitMoveWall( );
		loadAreaData( );
		loadEffect( );
		loadSound( );
		findStockUI( );
		findGameClearUI( );
		findGameOverUI( );
		findAreaTxtUI( );
	}

	void Start( ) {
        state = STATE.WAIT;
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
			_faild_ui.SetActive( false );
			break;
		case STATE.TUTORIAL:
			break;
		}

        state = value;
        Device.Instanse.StopLittle( 1 );
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
			addSound( SOUND.GOAL );
			_clear_ui.SetActive( true );
			break;
		case STATE.GAME_OVER:
			act = actOnGameOver;
			_faild_ui.SetActive( true );
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
			if ( Game.Instance.tutorial ) {
					Game.Instance.reset( );
					Game.Instance.loadScene( Game.SCENE.SCENE_TITLE );
			} else {
				Game.Instance.loadScene( Game.SCENE.SCENE_STAGESELECT );
			}
		}
	}

	void actOnTutorial( ) {
	}

	//----------------------/Act---------------------//

	public Player getPlayer( ) {
		return _data.player.GetComponent< Player >( );
	}
		
		
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
		return "Area " + ( area + 1 ).ToString( ) + "/" + MAX_AREA.ToString( );
	}

	//----------------初期化系統----------------//

	bool loadAreaData( ) {
		area++;
		destroyArea( );
		if ( area >= MAX_AREA ) {
			return false;
		}
		_data = new PlayData( );
		string path = getAssetPath( Game.Instance.stage, area );
		if ( Game.Instance.tutorial ) {
			path = getAssetTutorialPath( area );
		}
		{//Asset
			data = ScriptableObject.CreateInstance< BoardData >( );
			//BoardData asset = AssetDatabase.LoadAssetAtPath< BoardData >( path ); //Resources.Load< BoardData >( path );
			string[ ] div = { "Resources/", ".asset" };
			string[ ] tmp = path.Split( div, StringSplitOptions.None );
			path = tmp[ 1 ];
			BoardData asset = Resources.Load< BoardData >( path );

			if ( asset == null ) {
				print( "エリアのAssetが存在しません。" );
				Application.Quit( );
				return false;
			}
			data.copy( asset );
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
		//Boss
		_data.boss = data.createBoss( );
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
		//Boss
		foreach ( GameObject obj in _data.boss ) {
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

	void findGameOverUI( ) {
		_faild_ui = GameObject.Find( "UI" ).transform.Find( "GameOver" ).gameObject;
		_faild_ui.SetActive( false );
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
		case BOARDOBJECT.BOSS:
			tag = "Boss";
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

	public static string getAssetDir( int stage ) {
		return "Assets/Resources/Play/Data/Board/0/" + stage.ToString( );
	}

	public static string getAssetPath( int stage, int area ) {
		return  getAssetDir( stage ) + "/" + area.ToString( ) + ".asset";
	}

	public static string getAssetTutorialDir( ) {
		return "Assets/Resources/Play/Data/Board/Tutorial";
	}

	public static string getAssetTutorialPath( int area ) {
		return  getAssetTutorialDir( ) + "/" + area.ToString( ) + ".asset";
	}
	//-------------/Public static-------------------------//


	//-----------------Effect&Sound-------------------------//
	void loadEffect( ) {
		_effect = new GameObject[ ( int )EFFECT.MAX ]; 
		_effect[ ( int )EFFECT.COL ] = Instantiate( Resources.Load< GameObject >( "Play/Prefab/Effect/Col" ) );
	}

	void loadSound( ) {
		_sound = new GameObject[ ( int )SOUND.MAX ];
		_sound[ ( int )SOUND.BGM  ] = Instantiate( Resources.Load< GameObject >( "Play/Prefab/Sound/Bgm" ) );
		_sound[ ( int )SOUND.REF  ] = Instantiate( Resources.Load< GameObject >( "Play/Prefab/Sound/Ref" ) );
		_sound[ ( int )SOUND.GOAL ] = Instantiate( Resources.Load< GameObject >( "Play/Prefab/Sound/Goal" ) );
	}

	public void addEffect( EFFECT effect, Vector3 pos ) {
		_effect[ ( int )effect ].transform.position = pos;
		_effect[ ( int )effect ].GetComponent< ParticleSystem >( ).Play( );
	}

	public void addSound( SOUND sound ) {
		_sound[ ( int )sound ].GetComponent< AudioSource >( ).Play( );
	}


	//------------------------------------------------//
}
