using UnityEngine;
public class Wall : MonoBehaviour {
	public virtual void Awake( ) {
		gameObject.AddComponent< BoxCollider2D >( );
		gameObject.isStatic = true;
	}
	public virtual void Start ( ) { }
	public virtual void Update( ) { }
	public virtual void reset( ) { }
}
