using UnityEngine;
class PlayDebuger : MonoBehaviour {
	//Awakeは非アクディブでも実行される?
	public int stage = 0;

	void Awake( ) {
		if ( Main.instance ) {
			return;
		}
		Main.initialize( gameObject );
		Game game = Game.Instance;
		game.chapter = 0;
		game.stage = stage;
		game.loadScene( Game.SCENE.SCENE_PLAY );
	}
}
