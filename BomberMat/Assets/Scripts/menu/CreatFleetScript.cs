/**
 *by Jules Maurer
 *
 * Desc :
 * Script for an example for gest fleet
 * 
 * 
**/

using UnityEngine;
using System.Collections;

public class CreatFleetScript : MonoBehaviour {

    public int _nbMode;
    public GameObject[][] myTab;

	// Use this for initialization
	void Awake () {

	    this.myTab = new GameObject[_nbMode][];
        for (var i = 0; i < _nbMode; i++)
        {
            this.myTab[i] = new GameObject[(int)StaticBoard.gameRule.NB_ELEMENTS];
        }
     /*   for (var i = 0; i < 5; i++)
        {
            for (var j = 0; j < this.myTab[i].Length; j++)
            {
            }
        } 
        */
	}

    // Update is called once per frame
    void Update()
    {
	
	}
}
