using UnityEngine;
using UnityEditor;
using System.IO;

public class BoardDataManager : MonoBehaviour {
	public BoardData Data { get; private set; }
	[Range(0, Play.MAX_STAGE)]
	public int Stage = 0;
	[Range(0, Play.MAX_AREA)]
	public int Area = 0;

	int asset_stage = 0;
	int asset_area  = 0;

#if UNITY_EDITOR
	void createAsset( ) {
		if ( Data == null ) {
			Data = ScriptableObject.CreateInstance< BoardData >( );
		}
		string dir = getAssetDir( );
		if ( !Directory.Exists( dir ) ) {
			Directory.CreateDirectory( dir );
		}
		AssetDatabase.CreateAsset( Data, getAssetPath( ) + ".asset" );
		saveTmpData( );
		Debug.Log( "Assetを生成しました" );
	}

	public void loadAsset( ) {
		Data = AssetDatabase.LoadAssetAtPath< BoardData >( getAssetPath( ) + ".asset" );
		if ( Data == null ) {
			createAsset( );
		} else {
			eraseGameObject( );
			createGameObject( );
			Debug.Log( "Assetをロードしました" );
		}
		saveTmpData( );
	}

	public void saveAsset( ) {
		if ( Data == null ) {
			return;
		}
		Debug.Log( "Assetを保存しました" );
		if ( !File.Exists( getAssetPath( ) + ".asset" ) ||
			  asset_area  != Area ||
			  asset_stage != Stage ) {
			createAsset( );
			return;
		}
		AssetDatabase.SaveAssets( );
	}

	public void serchObject( ) {
		if ( Data == null ) {
			return;
		}
		Data.serchBoardObjects( );
	}

	string getAssetPath( ) {
		return "Assets/Resources/" + Play.getDataPath( Stage, Area );
	}

	string getAssetDir( ) {
		return "Assets/Resources/" + Play.getDataDir( Stage );
	}

	void saveTmpData( ) {
		asset_stage = Stage;
		asset_area = Area;
	}

	void createGameObject( ) {
		Data.createPlayer( );
		Data.createGoal( );
		Data.createWalls( );
		Data.createWallMoves( );
		Data.createSwitchs( );
	}

	void eraseGameObject( ) {
		{//Player削除
			string tag = Play.getTag( Play.BOARDOBJECT.PLAYER );
			GameObject obj = GameObject.FindGameObjectWithTag( tag );
			Destroy( obj );
		}
		{//Goal削除
			string tag = Play.getTag( Play.BOARDOBJECT.GOAL );
			GameObject obj = GameObject.FindGameObjectWithTag( tag );
			Destroy( obj );
		}
		{//wall削除
			string tag = Play.getTag( Play.BOARDOBJECT.WALL );
			GameObject[ ] objects = GameObject.FindGameObjectsWithTag( tag );
			foreach( GameObject obj in objects ) {
				Destroy( obj );
			}
		}
		{//wallMove削除
			string tag = Play.getTag( Play.BOARDOBJECT.WALL_MOVE );
			GameObject[ ] objects = GameObject.FindGameObjectsWithTag( tag );
			foreach( GameObject obj in objects ) {
				Destroy( obj );
			}
		}
		{//switch削除
			string tag = Play.getTag( Play.BOARDOBJECT.SWITCH );
			GameObject[ ] objects = GameObject.FindGameObjectsWithTag( tag );
			foreach( GameObject obj in objects ) {
				Destroy( obj );
			}
		}

	}
#endif
}