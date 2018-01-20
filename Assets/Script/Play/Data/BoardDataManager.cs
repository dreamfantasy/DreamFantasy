#if UNITY_EDITOR
using UnityEngine;
using System.IO;
using UnityEditor;

public class BoardDataManager : MonoBehaviour {
	[SerializeField]
	public BoardData Data;
	[Range(0, Play.MAX_STAGE - 1)]
	public int Stage = 0;
	[Range(0, Play.MAX_AREA - 1)]
	public int Area = 0;
	public bool Tutorial = false;

	//-------------------コマンド-------------------------//


	void createAsset( ) {
		string dir = getAssetDir( );
		if ( !Directory.Exists( dir ) ) {
			Directory.CreateDirectory( dir );
		}

		BoardData asset = ScriptableObject.CreateInstance< BoardData >( );
		asset.copy( Data );
		AssetDatabase.CreateAsset( asset, getAssetPath( ) );
	}

	public void loadAsset( ) {
		BoardData asset = AssetDatabase.LoadAssetAtPath< BoardData >( getAssetPath( ) );
		if ( asset == null ) {
			return;
		}
		Data.copy( asset );
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
		//Saveがうまくいかないので削除して生成する
		AssetDatabase.DeleteAsset( path );
		createAsset( );

		Debug.Log( "Assetをセーブしました" );
	}

	public void serchObject( ) {
		Data.serchBoardObjects( );
	}
	
	//--------------------------------------------------//
	public void init( ) {
		Data = ScriptableObject.CreateInstance< BoardData >( );
		eraseGameObject( );
	}

	string getAssetPath( ) {
		if ( Tutorial ) {
			return Play.getAssetTutorialPath( Area );
		}
		return Play.getAssetPath( Stage, Area );
	}

	string getAssetDir( ) {
		if ( Tutorial ) {
			return Play.getAssetTutorialDir( );
		}
		return Play.getAssetDir( Stage );
	}

	void createGameObject( ) {
		Data.createBg( );
		Data.createPlayer( );
		Data.createGoal( );
		Data.createWalls( );
		Data.createSwitchs( );
        Data.createBoss( );
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
		{//switch削除
			string tag = Play.getTag( Play.BOARDOBJECT.SWITCH );
			GameObject[ ] objects = GameObject.FindGameObjectsWithTag( tag );
			foreach( GameObject obj in objects ) {
				DestroyImmediate( obj );
			}
		}
        {//Boss削除
            string tag = Play.getTag( Play.BOARDOBJECT.BOSS );
            GameObject obj = GameObject.FindGameObjectWithTag( tag );
            if ( obj ) {
                DestroyImmediate( obj );
            }
        }

	}
}
#endif