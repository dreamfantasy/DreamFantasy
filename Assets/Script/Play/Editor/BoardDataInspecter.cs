using UnityEngine;
using UnityEditor;

[CustomEditor( typeof( BoardData ), true ) ]
public class BoardDataInspecter : Editor {
	bool folder_wall = false;
	public override void OnInspectorGUI( ) {
		base.OnInspectorGUI( );
		BoardData targetComponent = target as BoardData;
		//Wall
		folder_wall = EditorGUILayout.Foldout( folder_wall, "Wall継承 <List>" );
		if ( folder_wall ) {
			EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
			int count = 0;
			foreach ( BoardData.WallData data in targetComponent._Wall ) {
				count++;
				if ( data is BoardData.WallMoveData ) {
					EditorGUILayout.LabelField( count.ToString( ), "WallMove" );
					//Option
					BoardData.WallMoveData move = data as BoardData.WallMoveData;
					EditorGUILayout.Vector2Field( "Vec", move.option.vec );
					EditorGUILayout.IntField( "ReverseTime", move.option.reverse_time );
				} else {
					EditorGUILayout.LabelField( count.ToString( ), "Wall" );
				}
			}
			EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
		}
	}
}

