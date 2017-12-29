using UnityEngine;
using System.Collections.Generic;

public class PlayData : MonoBehaviour { 
    public GameObject _player;
    public GameObject   _goal  ;
    public List< GameObject > _wall   = new List< GameObject >( );
    public List< GameObject > _switch = new List< GameObject >( );

	public void setActives( bool value ) {
		//Player
		if ( _player != null ) {
			_player.SetActive( value );
		}
		//Goal
		if ( _goal != null ) {
			_goal.SetActive( value );
		}
		//Wall
		foreach ( GameObject component in _wall ) {
			component.SetActive( value );
		}
		//Switch
		foreach ( GameObject component in _switch ) {
			component.SetActive( value );
		}
	}

	public void reset( ) {
		//Player
		if ( _player != null ) {
			_player.GetComponent< Player >( ).reset( );
		}
		//Goal
		if ( _goal != null ) {
			_goal.GetComponent< Goal >( ).reset( );
		}
		//Wall
		foreach ( GameObject component in _wall ) {
			component.GetComponent< Wall >( ).reset( );
		}
		//Switch
		foreach ( GameObject component in _switch ) {
			component.GetComponent< Switch >( ).reset( );
		}
	}
}