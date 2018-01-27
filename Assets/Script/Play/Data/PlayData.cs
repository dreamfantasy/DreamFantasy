using UnityEngine;
using System.Collections.Generic;

public class PlayData { 
    public GameObject player { get; set; }
    public GameObject goal   { get; set; }
    public GameObject[ ] walls   { get; set; }
    public GameObject[ ] switchs { get; set; }
    public GameObject[ ] boss    { get; set; }


	public void setActives( bool value ) {
		//Player
		if ( player != null ) {
			player.SetActive( value );
		}
		//Goal
		if ( goal != null ) {
			goal.SetActive( value );
		}
		//Wall
		foreach ( GameObject obj in walls ) {
			obj.SetActive( value );
		}
		//Switch
		foreach ( GameObject obj in switchs ) {
			obj.SetActive( value );
		}
		//Boss
		foreach ( GameObject obj in boss ) {
			obj.SetActive( value );
		}
	}

	public void reset( ) {
		//Player
		if ( player != null ) {
			player.GetComponent< Player >( ).reset( );
		}
		//Goal
		if ( goal != null ) {
			goal.GetComponent< Goal >( ).reset( );
		}
		//Wall
		foreach ( GameObject obj in walls ) {
			obj.GetComponent< Wall >( ).reset( );
		}
		//Switch
		foreach ( GameObject obj in switchs ) {
			obj.GetComponent< Switch >( ).reset( );
		}
		//boss
		foreach ( GameObject obj in boss ) {
			obj.GetComponent< Boss >( ).reset( );
		}
	}
}