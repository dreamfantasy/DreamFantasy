using UnityEngine;

class InstanseGenerater : MonoBehaviour {
	void Awake( ) {
		if ( !Main.instance ) {
			Main.initialize( gameObject );
		}
	}
}
