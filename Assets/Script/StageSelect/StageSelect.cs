using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class StageSelect : MonoBehaviour {
	public enum SOUND {
		MOVE,
		SELECT,
		MAX
	};

	public Vector3 _chara_foot;
	GameObject _chara;
	GameObject[ ] _buttons;
	GameObject[ ] _sound;
	Color color_button_off = new Color( 1, 0, 0 );
	Color color_button_on  = new Color( 1, 1, 1 );
	Vector3 chara_defalt_pos;
	static int _target = 0;
	int _next_count = 0;
	int _count { get; set; }
	bool _select { get; set; }

	const float FLOATING_SPEED = 0.01f;
	const float FLOATING_RANGE = 20.0f;
	const int NEXT_WAIT_COUNT = 60;

	void Awake( ) {
		loadSound( );
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
		chara_defalt_pos = _chara.transform.position;
		_count  = 0;
		_select = false;
	}

	void Update( ) {
		if ( _select ) {
			_next_count++;
			if ( _next_count > NEXT_WAIT_COUNT ) {
				Game.Instance.loadScene( Game.SCENE.SCENE_SCENARIO );
			}
		}

		Vector3 add = Vector3.up * ( float )Math.Abs( Math.Sin( _count * FLOATING_SPEED ) ) * FLOATING_RANGE;
		_chara.transform.position = chara_defalt_pos + add;
		_count++;
	}

	void selectButton( int stage ) {
		if ( _select ) {
			return;
		}
		if ( !isSelectAble( stage ) ) {
			return;
		}
		if ( stage != _target ) {
			_target = stage;
			_chara.transform.position = _buttons[ stage ].transform.position + _chara_foot;
			chara_defalt_pos = _chara.transform.position;
			addSound( SOUND.MOVE );
			return;
		}
		addSound( SOUND.SELECT );
		_select = true;
		_next_count = 0;
		Game.Instance.stage = stage;
	}

	bool isSelectAble( int stage ) {
		return stage <= Game.Instance.clear_stage + 1;
	}

	void loadSound( ) {
		_sound = new GameObject[ ( int )SOUND.MAX ];
		_sound[ ( int )SOUND.MOVE   ] = Instantiate( Resources.Load< GameObject >( "StageSelect/Prefab/Sound/Move" ) );
		_sound[ ( int )SOUND.SELECT ] = Instantiate( Resources.Load< GameObject >( "StageSelect/Prefab/Sound/Select" ) );
	}

	void addSound( SOUND sound ) {
		_sound[ ( int )sound ].GetComponent< AudioSource >( ).Play( );
	}

	bool isPlayingSound( SOUND sound ) {
		return _sound[ ( int )sound ].GetComponent< AudioSource >( ).isPlaying;
	}
}
