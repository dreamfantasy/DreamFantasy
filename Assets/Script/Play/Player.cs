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
	
	public bool Lock { get; set; }

	enum ACTION {
		WAIT,
		NORMAL,
		MOVE,
		STRETCH,
		NONE,
	};
	GameObject _allow;
	GameObject _line;
	Vector2 _touch_start_pos;
	Vector3 _start_pos;
	System.Action Action;
	Play _play;
	int _hp;
	bool _col = false;


	//初期化処理( Start前 )
	void Awake( ) {
		_play = GameObject.Find( "Script" ).GetComponent< Play >( );
		loadArrow( ); //矢印
		loadLine( );  //予測線
		addCol( );
		addRd( );
		MOVE_SPEED *= 50;
	}

	//初期化処理( Awake後 )
	void Start( ) {
		Lock = false;
		_start_pos = transform.position;
		_allow.transform.localScale = Vector3.zero;
		_hp = _sprites.Length;
		GetComponent< SpriteRenderer >( ).sprite = _sprites[ _hp - 1 ];//画像入れ替え
		Action = ActOnStartWait;
	}

	//更新処理
	void Update( ) {
		if ( Lock ) {
			return;
		}
		Action( );
		_col = false;
	}

	
	void OnCollisionEnter2D( Collision2D collision ) {
		if ( _col ) {
			return;
		}
		_col = true;
		damage( );
		//Effect
		_play.addEffect( Play.EFFECT.COL, collision.contacts[ 0 ].point );
		//Sound
		_play.addSound( Play.SOUND.REF );
	}


	//Allowのプレハブを読み込む
	void loadArrow( ) {
		_allow = Resources.Load( Play.getPrefabDir( ) + "/Other/Allow" ) as GameObject;
		_allow = Instantiate( _allow );
		_allow.transform.position = Vector3.zero;
		_allow.transform.parent = gameObject.transform;
	}

	//予測線のプレハブ
	void loadLine( ) {
		_line = Resources.Load( Play.getPrefabDir( ) + "/Other/Line" ) as GameObject;
		_line = Instantiate( _line );
		_line.transform.parent = gameObject.transform;
		_line.transform.position = Vector3.back * 2;
		_line.transform.localScale = Vector3.forward + Vector3.right + Vector3.up * 1000;
		_line.SetActive( false );
	}


	//Colliderを追加する
	void addCol( ) {
		CircleCollider2D col = gameObject.AddComponent< CircleCollider2D >( );
		col.sharedMaterial = Resources.Load< PhysicsMaterial2D >( "Play/Material/Ref" );
		col.radius = 82;
	}

	//RigidBodyを追加する
	void addRd( ) {
		Rigidbody2D rd = gameObject.AddComponent< Rigidbody2D >( );
		rd.freezeRotation = true;
		rd.gravityScale = 0.0f;
		rd.useAutoMass = false;
		rd.mass = 0.0f;
		rd.drag = 0.0f;
		rd.angularDrag = 0.0f;
	}

	//速度を一定にする
	void adjustVec( ) {
		Rigidbody2D rd = gameObject.GetComponent< Rigidbody2D >( );
		rd.velocity = rd.velocity.normalized * MOVE_SPEED;
	}

	//壁などによるダメージ
	void damage( ) {
		_hp--;
		if ( _hp <= 0 ) {
			_hp = 0;
			gameObject.GetComponent< Rigidbody2D >( ).velocity = Vector2.zero;
			return;
		}
		GetComponent< SpriteRenderer >( ).sprite = _sprites[ _hp - 1 ];//画像入れ替え
	}

	
	
	//--------------------------public---------------------------//
	public bool isFinished( ) {
		return _hp <= 0;
	}

	public void reset( ) {
		transform.position = _start_pos;
		GetComponent< Rigidbody2D >( ).velocity = Vector2.zero;
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
		if ( Device.Instanse.Phase == Device.PHASE.BEGAN ) {
			_touch_start_pos = Device.Instanse.Pos;
			Action = actOnStretch;
		}
	}
	void actOnStretch( ) {
		Vector2 vec = _touch_start_pos - Device.Instanse.Pos;

		if ( Device.Instanse.Phase == Device.PHASE.MOVED ) {
			//指を動かしているとき
			if ( vec.magnitude > MOVE_RANGE ) {
				//球が動く範囲の場合
				if ( !_line.activeSelf ) {
					_line.SetActive( true );
				}

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
				_line.transform.localRotation = rot;
			} else {
				//球が動かない範囲の場合

				//矢印非表示
				_allow.transform.localScale = Vector3.zero;
			}
		}

		if ( Device.Instanse.Phase == Device.PHASE.ENDED ) {
			//指を離したとき

			//矢印を見えなくする
			_allow.transform.localScale = Vector3.zero;
			//線を見えなくする
			_line.SetActive( false );

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
