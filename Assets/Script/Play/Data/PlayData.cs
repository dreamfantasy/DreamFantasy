using UnityEngine;
using System.Collections.Generic;

public class PlayData : MonoBehaviour { 
    public GameObject Player;
    public GameObject Goal  ;
    public List< GameObject > Wall   = new List< GameObject >( );
    public List< GameObject > Switch = new List< GameObject >( );

	public void setActives( bool value ) {
		//Player
		if ( Player != null ) {
			Player.SetActive( value );
		}
		//Goal
		if ( Goal != null ) {
			Goal.SetActive( value );
		}
		//Wall
		foreach ( GameObject obj in Wall ) {
			obj.SetActive( value );
		}
		//Switch
		foreach ( GameObject obj in Switch ) {
			obj.SetActive( value );
		}
	}
}