using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scenario : MonoBehaviour {
	struct Data {
		public string name;
		public string msg;
	};
	Text _msg_text;
	Text _name_text;
	Data[ ] _data;
	int _line;

	void Awake( ) {
		_msg_text  = GameObject.Find( "Canvas/MsgBox/Text" ).GetComponent< Text >( );
		_name_text = GameObject.Find( "Canvas/NameBox/Text" ).GetComponent< Text >( );
		loadData( );
	}

	void Start( ) {
		if ( _data.Length <= 0 ) {
			return;
		}
		
		for ( int i = 0; i < _data.Length; i++ ) {
			print( "msg" + i.ToString( ) + ":" + _data[ i ].msg );
		}
		_line = 0;
		setData( );
	}

	void Update( ) {
		if ( Device.Instanse.Phase == Device.PHASE.ENDED ) {
			print( "touch" );
			_line++;
			if ( _line >= _data.Length ) {
				Game.Instance.loadScene( Game.SCENE.SCENE_PLAY );
				return;
			}
			setData( );
			Device.Instanse.StopLittle( 3 );
		}
	}

	void loadData( ) {
		TextAsset text = Resources.Load( "Scenario/Story/" +
			Game.Instance.chapter.ToString( ) + "/" +
			Game.Instance.stage.ToString( ) ) as TextAsset;
		if ( !text ) {
			print( "シナリオがありません\n" );
			return;
		}
		string[ ] lines = text.text.Split( '[' );
		int size = lines.Length - 1;
		_data = new Data[ size ];
		for ( int i = 0; i < size; i++ ) {
			string[ ] data = lines[ i + 1 ].Split( ']' );
			if ( data.Length != 2 ) {
				continue;
			}
			_data[ i ].name = data[ 0 ];
			_data[ i ].msg  = data[ 1 ];
		}
	}

	void setData( ) {
		_msg_text.text  = _data[ _line ].msg;
		_name_text.text = _data[ _line ].name;
	}
}
