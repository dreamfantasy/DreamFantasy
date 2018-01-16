using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
[ExecuteInEditMode]
#endif
public class Wall : MonoBehaviour {
	public virtual void Awake( ) {
		gameObject.isStatic = true;
		//Layer
		GetComponent< MeshRenderer >( ).sortingLayerName = "Default";
		GetComponent< MeshRenderer >( ).sortingOrder = 3;
		//print( GetComponent< MeshRenderer >( ).sortingLayerName + ":" + GetComponent< MeshRenderer >( ).sortingOrder.ToString( ) );
	}
	public virtual void Start ( ) {
	}
	public virtual void Update( ) { }
	public virtual void reset( ) { }
}
