using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class StageSelect : MonoBehaviour {
	public Vector3 _chara_foot;
	GameObject _chara;
	GameObject[ ] _buttons;
	Color color_button_off = new Color( 1, 0, 0 );
	Color color_button_on  = new Color( 1, 1, 1 );
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
			if ( isSelectAble( stage ) ) {
				_buttons[ i ].GetComponent< Image >( ).color = color_button_on;
			} else {
				_buttons[ i ].GetComponent< Image >( ).color = color_button_off;
			}
		}
		_chara.transform.position = _buttons[ 0 ].transform.position + _chara_foot;
	}

	void Update( ) {
	}

	void selectButton( int stage ) {
		if ( !isSelectAble( stage ) ) {
			return;
		}
		if ( stage != _target ) {
			_target = stage;
			_chara.transform.position = _buttons[ stage ].transform.position + _chara_foot;
			return;
		}
		Game.Instance.stage = stage;
		Game.Instance.loadScene( Game.SCENE.SCENE_SCENARIO );
	}

	bool isSelectAble( int stage ) {
		return stage <= Game.Instance.clear_stage + 1;
	}
}
