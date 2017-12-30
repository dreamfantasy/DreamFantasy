using UnityEngine;
using UnityEditor;
using System.IO;

public class BoardDataManager : MonoBehaviour {
	[SerializeField]
	public BoardData Data;
	[Range(0, Play.MAX_STAGE - 1)]
	public int Stage = 0;
	[Range(0, Play.MAX_AREA - 1)]
	public int Area = 0;
	public bool Tutorial = false;

#if UNITY_EDITOR
	//-------------------コマンド-------------------------//


	void createAsset( ) {
		string dir = getAssetDir( );
		if ( !Directory.Exists( dir ) ) {
			Directory.CreateDirectory( dir );
		}

		BoardData asset = ScriptableObject.CreateInstance< BoardData >( );
		copy( Data, asset );
		AssetDatabase.CreateAsset( asset, getAssetPath( ) );
	}

	public void loadAsset( ) {
		BoardData asset = AssetDatabase.LoadAssetAtPath< BoardData >( getAssetPath( ) );
		if ( asset == null ) {
			return;
		}
		copy( asset, Data );
		eraseGameObject( );
		createGameObject( );
		Debug.Log( "Assetをロードしました" );
	}

	public void saveAsset( ) {
		string path = getAssetPath( );
		if ( !File.Exists( getAssetPath( ) ) ) {
			//ファイルがない場合は生成
			createAsset( );
			Debug.Log( "Assetを生成/保存しました" );
			return;
		}
		BoardData asset = AssetDatabase.LoadAssetAtPath< BoardData >( path );
		if ( asset == null ) {
			AssetDatabase.DeleteAsset( path );
			saveAsset( );
			return;
		}
		copy( Data, asset );
		AssetDatabase.SaveAssets( );
		Debug.Log( "Assetをセーブしました" );
	}

	public void serchObject( ) {
		Data.serchBoardObjects( );
	}
	
	//--------------------------------------------------//
	public void init( ) {
		Data = new BoardData( );
	}

	void copy( BoardData from, BoardData to ) {
		to._Bg = from._Bg;
		to._Player = from._Player;
		to._Goal = from._Goal;
		to._Wall = from._Wall;
		to._WallMove = from._WallMove;
		to._Switch = from._Switch;
	}


	string getAssetPath( ) {
		if ( Tutorial ) {
			return "Assets/Resources/" + Play.getDataTutorialPath( Area ) + ".asset";
		}
		return "Assets/Resources/" + Play.getDataPath( Stage, Area ) + ".asset";
	}

	string getAssetDir( ) {
		if ( Tutorial ) {
			return "Assets/Resources/" + Play.getDataTutorialDir( );
		}
		return "Assets/Resources/" + Play.getDataDir( Stage );
	}

	void createGameObject( ) {
		Data.createBg( );
		Data.createPlayer( );
		Data.createGoal( );
		Data.createWalls( );
		Data.createWallMoves( );
		Data.createSwitchs( );
	}

	void eraseGameObject( ) {
		{//BG削除
			string tag = Play.getTag( Play.BOARDOBJECT.BG );
			GameObject obj = GameObject.FindGameObjectWithTag( tag );
			if ( obj ) {
				DestroyImmediate( obj );
			}
		}
		{//Player削除
			string tag = Play.getTag( Play.BOARDOBJECT.PLAYER );
			GameObject obj = GameObject.FindGameObjectWithTag( tag );
			if ( obj ) {
				DestroyImmediate( obj );
			}
		}
		{//Goal削除
			string tag = Play.getTag( Play.BOARDOBJECT.GOAL );
			GameObject obj = GameObject.FindGameObjectWithTag( tag );
			if ( obj != null ) {
				DestroyImmediate( obj );
			}
		}
		{//wall削除
			string tag = Play.getTag( Play.BOARDOBJECT.WALL );
			GameObject[ ] objects = GameObject.FindGameObjectsWithTag( tag );
			foreach( GameObject obj in objects ) {
				DestroyImmediate( obj );
			}
		}
		{//wallMove削除
			string tag = Play.getTag( Play.BOARDOBJECT.WALL_MOVE );
			GameObject[ ] objects = GameObject.FindGameObjectsWithTag( tag );
			foreach( GameObject obj in objects ) {
				DestroyImmediate( obj );
			}
		}
		{//switch削除
			string tag = Play.getTag( Play.BOARDOBJECT.SWITCH );
			GameObject[ ] objects = GameObject.FindGameObjectsWithTag( tag );
			foreach( GameObject obj in objects ) {
				DestroyImmediate( obj );
			}
		}

	}
#endif
}