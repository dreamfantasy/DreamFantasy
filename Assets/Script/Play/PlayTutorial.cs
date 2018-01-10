using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayTutorial : MonoBehaviour {
	List< List< string > > _texts = new List< List< string > >( );
	Text _ui_text;
	GameObject _ui_box;

	Play _play;
	Play.STATE _before_state = Play.STATE.WAIT;

	int _line = 0;

	void Awake( ) {
		_play = GameObject.Find( "Script" ).GetComponent< Play >( );
		_ui_box = Instantiate( Resources.Load< GameObject >( "Play/Prefab/Tutorial/TextBox" ) );
		_ui_box.transform.SetParent( GameObject.Find( "UI" ).transform, false );
		_ui_text = _ui_box.transform.Find( "Text" ).GetComponent< Text >( );
	}

	void Start( ) {
		setText( );
		_ui_box.SetActive( false );
	}

	void Update( ) {
		Play.STATE now_state = _play.state;
		if ( _before_state != Play.STATE.PLAY &&
			 _play.state   == Play.STATE.PLAY ) {
			//プレイヤーがテキストボックス以外
			//操作できないようにする
			PlayLock( true );
			_line = 0;

			if ( !readLine( ) ) {
				PlayLock( false );
			}
		}
		_before_state = _play.state;

		//テキストを出さない場合は抜ける
		if ( !_ui_box.activeSelf ) {
			return;
		}

		//テキスト処理
		if ( Device.Instanse.Phase == Device.PHASE.ENDED ) {
			if ( !readLine( ) ) {
				PlayLock( false );
			}
		}
	}

	void setText( ) {
		//横14文字まで

		{//Pエリア１開始時
			List< string > txts = new List< string >( );
			txts.Add( "それじゃあこのゲームの\n説明をさせてもらうわ" );
			txts.Add( "これは魔法弾と言って\n夢の世界で使える魔法の弾なの" );
			txts.Add( "この魔法弾がステージにある扉\nにたどり着けばクリアになるの" );
			txts.Add( "弾は画面のどこでもいいから\nタッチしてスライド\nすることで" );
			txts.Add( "魔法弾を飛ばす方向を決めて\n指を離すと矢印の方向に飛ぶわ" );
			txts.Add( "それじゃあ早速\nやってみましょうか\n下の魔法弾を扉に運んでみて" );

			_texts.Add( txts );
		}
		{//Phase2( エリア２開始時 )
			List< string > txts = new List< string >( );
			txts.Add( "飲み込みが早くて助かるわ\nそれじゃあ\n次のステップに進むわよ" );
			txts.Add( "実は魔法弾には耐久力があるの\n進む分には耐久は減らないけど" );
			txts.Add( "障害物に当たったり\n壁に当たると\n反射するけど耐久は減るわ" );
			txts.Add( "残りの耐久は色でわかるn耐久が0になると\n壊れてスタートに戻るの" );
			txts.Add( "壊れてスタートに戻ると\n左上の残段数が1つ減っていき" );
			txts.Add( "0になるとゲームオーバーに\nなるから残段数には\n気を付けてね" );

			_texts.Add( txts );
		}
		{//
			List< string > txts = new List< string >( );
			txts.Add( "あら？今度はステージに\n丸いものがあるわね？" );
			txts.Add( "これはスイッチと言って\n魔法弾が通過すると作動するわ" );
			txts.Add( "効果には扉が出たり\n岩が動いたりいろいろだけど\n今回は扉が出るようね" );
			txts.Add( "それじゃあここからは\n自由にやって\nこのステージをクリアして！" );
			_texts.Add( txts );
		}
	}

	bool readLine( ) {
		if ( _line >= _texts[ _play.area ].Count ) {
			return false;
		}
		_ui_text.text = _texts[ _play.area ][ _line ];
		_line++;
		return true;
	}

	void PlayLock( bool value ) {
		_ui_box.SetActive( value );
		_play.getPlayer( ).Lock = value;
	}
}
