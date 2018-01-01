using System.Collections;
using UnityEngine;
//using UnityEditor.SceneManagement;
using UnityEditor;

[ExecuteInEditMode]
public class CameraController : MonoBehaviour {
	const float WIDTH  =  9.0f;
	const float HEIGHT = 16.0f;

	void Start( ) { setRect( );	}

	#if UNITY_EDITOR
	void Update( ) {
		if ( !Application.isPlaying ) {
			setRect( );
		}
	}
	#endif

	void setRect( ) {
		Rect rect = new Rect( );
		float target_aspect = WIDTH / HEIGHT;
		float screen_aspect = ( float )Screen.width / Screen.height;
		float scale_height = screen_aspect / target_aspect;

		if ( 1.0f > scale_height ) {
			rect.x = 0;
			rect.y = ( 1.0f - scale_height ) / 2.0f;
			rect.width = 1.0f;
			rect.height = scale_height;
		} else {
			float scale_width = 1.0f / scale_height;
			rect.x = ( 1.0f - scale_width ) / 2.0f;
			rect.y = 0.0f;
			rect.width = scale_width;
			rect.height = 1.0f;
		}
		GetComponent< Camera >( ).rect = rect;
	}
}
