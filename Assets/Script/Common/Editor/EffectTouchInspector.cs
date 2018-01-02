using UnityEditor;
using UnityEngine;

[CustomEditor( typeof( EffectTouch ) ) ]
public class EffectTouchInspector : Editor {
	public override void OnInspectorGUI( ) {
		base.OnInspectorGUI( );
		if ( GUILayout.Button( "Start" ) ) {
			EffectTouch com = target as EffectTouch;
			com.touch( );
		}
	}
}
