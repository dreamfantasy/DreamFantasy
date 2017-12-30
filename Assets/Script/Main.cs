using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {
	public static bool instance = false;
	void Awake( ) {
		initialize( gameObject );
	}

	void Start( ) {
		Game.Instance.loadScene( Game.SCENE.SCENE_TITLE );
	}
	
	void Update( ) {
	}


	public static void initialize( GameObject obj ) {
		if ( instance ) {
			return;
		}
		instance = true;
		obj.AddComponent< Device >( );
		obj.AddComponent< Game >( );
		DontDestroyOnLoad( obj );
	}
}
