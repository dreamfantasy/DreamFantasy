using UnityEngine;
using UnityEditor;

[CustomEditor( typeof( BoardDataManager ) ) ]
public class BoardDataManagerInspecter : Editor {
	public override void OnInspectorGUI( ) {
		base.OnInspectorGUI( );
		BoardDataManager targetComponent = target as BoardDataManager;

		{//label
			string label = "( none )  [Create/Load]してください";
			if ( targetComponent.Data != null ) {
				label = "BoardData" + targetComponent.Data.ToString( );
			}
			EditorGUILayout.LabelField( "Data", label );
		}


		if ( targetComponent.Data ) {
			{//player
				string label = targetComponent.Data._Player != null ? "有" : "無";
				EditorGUILayout.LabelField( "Player", label );
			}
			{//Goal
				string label = targetComponent.Data._Goal != null ? "有" : "無";
				EditorGUILayout.LabelField( "Goal", label );
			}
			{//wall
				string label = "Num:" + targetComponent.Data._Wall.Count;
				EditorGUILayout.LabelField( "Wall", label );
			}
			{//switch
				string label = "Num:" + targetComponent.Data._Switch.Count;
				EditorGUILayout.LabelField( "Switch", label );
			}
			//button
			if ( GUILayout.Button( "更新" ) ) {
				targetComponent.serchObject( );
			}
		}
		EditorGUILayout.LabelField( "" );
		EditorGUILayout.LabelField( "Asset" );

		GUILayout.BeginHorizontal( );
		//button
		if ( GUILayout.Button( "Init" ) ) {
			targetComponent.init( );
		}
		if ( GUILayout.Button( "Load" ) ) {
			targetComponent.loadAsset( );
		}
		if ( GUILayout.Button( "Save" ) ) {
			targetComponent.serchObject( );
			targetComponent.saveAsset( );
		}
		GUILayout.EndHorizontal( );
	}
}