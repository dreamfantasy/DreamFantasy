using UnityEngine;
class PlayDebuger : MonoBehaviour {
	//Awakeは非アクディブでも実行される?
	public int stage = 0;

	void Awake( ) {
		if ( Main.instance ) {
			return;
		}
		Main.InstanceGame( gameObject );
		Game.chapter = 0;
		Game.stage = stage;
		Game.loadScene( Game.SCENE.SCENE_PLAY );
	}
}
