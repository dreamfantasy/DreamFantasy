using UnityEngine;
public class Wall : MonoBehaviour {
	public virtual void Awake( ) {
		gameObject.isStatic = true;
		gameObject.AddComponent< BoxCollider2D >( );
	}
	public virtual void Start ( ) {
		initCol( );
	}
	public virtual void Update( ) { }
	public virtual void reset( ) { }

	void initCol( ) {
		//BoxCollider2D col = gameObject.GetComponent< BoxCollider2D >( );
		//col.size = GetComponent< SpriteRenderer >( ).size;
		//col.offset = Vector2.up * col.size.y * 0.5f;
		//col.edgeRadius = 5.0f;
		//col.edgeRadius = 0.01f;
	}
}
