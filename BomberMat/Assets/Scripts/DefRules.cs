/****
 * Script ayant la définition de toutes les modes de jeu différents
 */
using UnityEngine;
using System.Collections;

public class DefRules : MonoBehaviour {

    //Tableau de régle listant les modes de jeu
    static public GameRules[] rules;

	// Use this for initialization
	static public void starting () {
        rules = new GameRules[(int)StaticBoard.gameRule.NB_ELEMENTS];

        rules[(int)StaticBoard.gameRule.CLASSIC] = new GameRules(3, false, false);

        rules[(int)StaticBoard.gameRule.CLASSIC_HOLES] = new GameRules(3, true, false);

        rules[(int)StaticBoard.gameRule.ICE] = new GameRules(0.5f, false, false);

        rules[(int)StaticBoard.gameRule.ICE_HOLES] = new GameRules(0.5f, true, false);
		
		rules[(int)StaticBoard.gameRule.CLASSIC_SPEEDWALK] = new GameRules(3f, false, true);
		
		rules[(int)StaticBoard.gameRule.CLASSIC_HOLES_SPEEDWALK] = new GameRules(3f, true, true);
	}
}
