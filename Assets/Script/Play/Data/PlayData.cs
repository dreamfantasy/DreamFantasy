using UnityEngine;
using System.Collections.Generic;

public class PlayData : MonoBehaviour { 
    public Player _player;
    public Goal   _goal  ;
    public List< Wall     > _wall   = new List< Wall    >( );
    public List< Switch   > _switch = new List< Switch >( );

	public void setActives( bool value ) {
		//Player
		if ( _player != null ) {
			_player.gameObject.SetActive( value );
		}
		//Goal
		if ( _goal != null ) {
			_goal.gameObject.SetActive( value );
		}
		//Wall
		foreach ( Wall component in _wall ) {
			component.gameObject.SetActive( value );
		}
		//Switch
		foreach ( Switch component in _switch ) {
			component.gameObject.SetActive( value );
		}
	}

	public void reset( ) {
		//Player
		if ( _player != null ) {
			_player.reset( );
		}
		//Goal
		if ( _goal != null ) {
			_goal.reset( );
		}
		//Wall
		foreach ( Wall component in _wall ) {
			component.reset( );
		}
		//Switch
		foreach ( Switch component in _switch ) {
			component.reset( );
		}
	}
}