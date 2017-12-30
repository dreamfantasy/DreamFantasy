using UnityEngine;
using System.Collections.Generic;

public class PlayData { 
    public GameObject player { get; set; }
    public GameObject goal   { get; set; }
    public GameObject[ ] walls   { get; set; }
    public GameObject[ ] switchs { get; set; }


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
		foreach ( GameObject component in walls ) {
			component.SetActive( value );
		}
		//Switch
		foreach ( GameObject component in switchs ) {
			component.SetActive( value );
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
		foreach ( GameObject component in walls ) {
			component.GetComponent< Wall >( ).reset( );
		}
		//Switch
		foreach ( GameObject component in switchs ) {
			component.GetComponent< Switch >( ).reset( );
		}
	}
}