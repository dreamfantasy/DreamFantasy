using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {
	public static bool instance = false;
	void Awake( ) {
		InstanceGame( gameObject );
	}

	void Start( ) {
		Game.loadScene( Game.SCENE.SCENE_TITLE );
	}
	
	void Update( ) {
	}


	public static void InstanceGame( GameObject obj ) {
		if ( instance ) {
			return;
		}
		instance = true;
		obj.AddComponent< Device >( );
		obj.AddComponent< Game >( );
		DontDestroyOnLoad( obj );
	}
}
