using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour {
	public Sprite[ ] _sprites;
	[ Range( 0, 50 ) ]
	public float MOVE_SPEED = 10;
	[ Range( 1, 100 ) ]
	public float MOVE_RANGE = 30;
	[ Range( 0.1f, 2 ) ]
	public float MAX_ALLOW_SIZE = 0.5f;
	[ Range( 0.0001f, 0.005f ) ]
	public float ALLOW_SIZE_RATIO = 0.001f;
	
	enum ACTION {
		WAIT,
		NORMAL,
		MOVE,
		STRETCH,
		NONE,
	};
	LineRenderer _line;
	GameObject _allow;
	Vector2 _touch_start_pos;
	Vector3 _start_pos;
	System.Action Action;
	Play _play;
	int _hp;


	//初期化処理( Start前 )
	void Awake( ) {
		_play = GameObject.Find( "Script" ).GetComponent< Play >( );
		_line = gameObject.AddComponent< LineRenderer >( );//予測線
		CircleCollider2D col = gameObject.AddComponent< CircleCollider2D >( );
		col.sharedMaterial = Resources.Load< PhysicsMaterial2D >( "Play/Material/Ref" );
		col.radius = 82;
		loadArrow( );
		addRd( );
	}

	//初期化処理( Awake後 )
	void Start( ) {
		MOVE_SPEED *= 50;
		_start_pos = transform.position;
		_allow.transform.localScale = Vector3.zero;
		_hp = _sprites.Length;
		GetComponent< SpriteRenderer >( ).sprite = _sprites[ _hp - 1 ];//画像入れ替え
		Action = ActOnStartWait;
	}

	//更新処理
	void Update( ) {
		Action( );
	}

	//Allowのプレハブを読み込む
	void loadArrow( ) {
		_allow = Resources.Load( "Play/Prefab/Allow" ) as GameObject;
		_allow.transform.position = Vector3.zero;
	}

	//RigidBodyを追加する
	void addRd( ) {
		Rigidbody2D rd = gameObject.AddComponent< Rigidbody2D >( );
		rd.freezeRotation = true;
		rd.gravityScale = 0.0f;
	}

	//速度を一定にする
	void adjustVec( ) {
		Rigidbody2D rd = gameObject.GetComponent< Rigidbody2D >( );
		rd.velocity = rd.velocity.normalized * MOVE_SPEED;
	}

	//線の設定
	void initLine( ) {
		//マテリアル
		_line.material = new Material( Shader.Find("Unlit/Color") );
		_line.material.color = new Color( 1.0f, 0, 0, 0.2f );
		//線の幅
		_line.startWidth = 0.02f;
		_line.endWidth   = 0.02f;
	}

	//線の位置を初期化
	void resetLine( ) {
		Vector3[ ] positions = { };
		_line.SetPositions( positions );
	}

	//壁などによるダメージ
	void damage( ) {
		_hp--;
		if ( _hp <= 0 ) {
			_hp = 0;
		}
		GetComponent< SpriteRenderer >( ).sprite = _sprites[ _hp - 1 ];//画像入れ替え
	}
	
	
	//--------------------------public---------------------------//
	public bool isFinished( ) {
		return _hp <= 0;
	}

	public void reset( ) {
		transform.position = _start_pos;
		Start( );
	}
	//-----------------------------------------------------------//

	//--------------------------Action------------------------//
	void ActOnStartWait( ) {
		if ( _play.state == Play.STATE.PLAY ) {
			Action = actOnTouchWait;
		}
	}
	void actOnTouchWait( ) {
		if ( Device.getTouchPhase( ) == Device.PHASE.BEGAN ) {
			_touch_start_pos = Device.getPos( );
			Action = actOnStretch;
		}
	}
	void actOnStretch( ) {
		Vector2 vec = _touch_start_pos - Device.getPos( );

		if ( Device.getTouchPhase( ) == Device.PHASE.MOVED ) {
			//指を動かしているとき
			if ( vec.magnitude > MOVE_RANGE ) {
				//球が動く範囲の場合

				//矢印のサイズ計算
				Vector2 size = Vector2.one * vec.magnitude * ALLOW_SIZE_RATIO;
				if ( size.magnitude > MAX_ALLOW_SIZE ) {
					//矢印は一定以上の大きさにしない
					size = size.normalized * MAX_ALLOW_SIZE;
				}
				_allow.transform.localScale = size;

				//矢印の向きを計算( cross(外積)で回転方向、angleで角度を求めることによってrotを求める )
				float angle = Vector2.Angle( Vector2.up, vec );
				Vector3 axis = Vector3.Cross( Vector3.up, vec );
				Quaternion rot = Quaternion.AngleAxis( angle, axis );
				_allow.transform.localRotation = rot;

				//線描画
				_line = gameObject.GetComponent< LineRenderer >( );
				Vector3 add1 = vec.normalized * 0.5f;
				Vector3 add2 = vec.normalized * 100.0f;
				Vector3 [ ] positions = {
					transform.position + add1,
					transform.position + add2
				};
				positions[ 0 ].z = -1.0f;
				positions[ 1 ].z = -1.0f;
				_line.SetPositions( positions );
			} else {
				//球が動かない範囲の場合

				//矢印非表示
				_allow.transform.localScale = Vector3.zero;
			}
		}

		if ( Device.getTouchPhase( ) == Device.PHASE.ENDED ) {
			//指を離したとき

			//矢印を見えなくする
			_allow.transform.localScale = Vector3.zero;
			//線を見えなくする
			resetLine( );

			if ( vec.magnitude > MOVE_RANGE ) {
				//指の位置が変わってた場合動かす
				Rigidbody2D rd = GetComponent< Rigidbody2D >( );
				rd.velocity = vec.normalized * MOVE_SPEED;
				Action = actOnMove;
			} else {
				//指の位置が変わらなかった場合待機状態へ戻る
				Action = actOnTouchWait;
			}
		}
	}

	void actOnMove( ) {
		adjustVec( );
	}
	//---------------------------------------------------------//

}
