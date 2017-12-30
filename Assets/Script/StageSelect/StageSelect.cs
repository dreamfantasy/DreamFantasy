using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class StageSelect : MonoBehaviour {
	public Vector3 _chara_foot;
	GameObject _chara;
	GameObject[ ] _buttons;
	int _target;

	void Awake( ) {
		_chara = GameObject.Find( "Character" );
		_buttons = GameObject.FindGameObjectsWithTag( "SelectButton" );
	}

	void Start( ) {
		_buttons = _buttons.OrderBy( n => n.GetComponent< StageSelectButton >( ).stage ).ToArray( );
		int size = _buttons.Length;
		for ( int i = 0; i < size; i++ ) {
			int stage = i + 0;
			_buttons[ i ].GetComponent< Button >( ).onClick.AddListener( ( ) => selectButton( stage ) ); 
		}
		_chara.transform.position = _buttons[ 0 ].transform.position + _chara_foot;
	}

	void Update( ) {
	}

	void selectButton( int stage ) {
		if ( stage != _target ) {
			_target = stage;
			_chara.transform.position = _buttons[ stage ].transform.position + _chara_foot;
			return;
		}
		Game.Instance.stage = stage;
		Game.Instance.loadScene( Game.SCENE.SCENE_SCENARIO );
	}
}
