using UnityEngine;
using UnityEditor;
using System.IO;

public class BoardDataManager : MonoBehaviour {
	[SerializeField]
	public BoardData Data { get; private set; }
	[Range(0, Play.MAX_STAGE - 1)]
	public int Stage = 0;
	[Range(0, Play.MAX_AREA - 1)]
	public int Area = 0;

	int asset_stage = 0;
	int asset_area  = 0;

#if UNITY_EDITOR
	//-------------------コマンド-------------------------//
	void createAsset( ) {
		if ( Data == null ) {
			Data = ScriptableObject.CreateInstance< BoardData >( );
		}

		string dir = getAssetDir( );
		if ( !Directory.Exists( dir ) ) {
			Directory.CreateDirectory( dir );
		}

		string path = getAssetPath( );
		AssetDatabase.CreateAsset( Data, path );
		AssetDatabase.Refresh( );
		Debug.Log( "Assetを生成しました" );
		saveTmpData( );
	}

	public void loadAsset( ) {
		Data = AssetDatabase.LoadAssetAtPath< BoardData >( getAssetPath( ) );
		AssetDatabase.Refresh( );
		if ( Data == null ) {
			createAsset( );
		} else {
			Debug.Log( "Assetをロードしました" );
			saveTmpData( );
		}
	}

	public void reloadObject( ) {
		eraseGameObject( );
		createGameObject( );
	}

	public void unLoad( ) {
		Data = null;
	}
	
	//--------------------------------------------------//

	bool isCopy( ) {
		bool result = true;
		if ( asset_area  == Area &&
			 asset_stage == Stage ) {
			result = false;
		}
		return result;
	}

	public void serchObject( ) {
		if ( Data == null ) {
			return;
		}
		Data.serchBoardObjects( );
	}

	string getAssetPath( ) {
		return "Assets/Resources/" + Play.getDataPath( Stage, Area ) + ".asset";
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