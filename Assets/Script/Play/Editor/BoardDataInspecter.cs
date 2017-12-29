using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor( typeof( BoardDataManager ) ) ]
public class BoardDataInspecter : Editor {
	public override void OnInspectorGUI( ) {
		base.OnInspectorGUI( );
		BoardDataManager targetComponent = target as BoardDataManager;

		{//label
			string label = "( none )  [Create/Load]してください";
			if ( targetComponent.Data ) {
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
			{//wallMove
				string label = "Num:" + targetComponent.Data._WallMove.Count;
				EditorGUILayout.LabelField( "WallMove", label );
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
		if ( GUILayout.Button( "Create/Load" ) ) {
			targetComponent.loadAsset( );
		}
		if ( GUILayout.Button( "ReloadObject" ) ) {
			targetComponent.reloadObject( );
		}
		if ( GUILayout.Button( "UnLoad" ) ) {
			targetComponent.unLoad( );
		}
		GUILayout.EndHorizontal( );
	}
}