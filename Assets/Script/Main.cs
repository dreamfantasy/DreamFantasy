using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {
	void Awake( ) {
		gameObject.AddComponent< Device >( );
		gameObject.AddComponent< Game >( );
		DontDestroyOnLoad( gameObject );
	}

	void Start( ) {
		Game.loadScene( Game.SCENE.SCENE_TITLE );
	}
	
	void Update( ) {
	}
}
