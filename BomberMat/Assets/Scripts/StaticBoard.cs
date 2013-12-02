using UnityEngine;
using System.Collections;
using System;

public class StaticBoard : MonoBehaviour {

    //Position des blocks
    static public GameObject[][] map;

    //Largeur de la map
    static public int sizeX;

    //Longueur de la map
    static public int sizeZ;

    //Position des bombs
    static public GameObject[][] bomb;

    //Nombre de bombes déposés
    static public int bombDropped;

    //Liste des joueurs sur le serveur
    public static ArrayList players = new ArrayList();

    //Liste des joueurs dans la partie en cours
    public static ArrayList gamePlayers = new ArrayList();

    //Noms des joueurs sur le serveur
    public static ArrayList namePlayers = new ArrayList();

    //Nombre de joueurs prêts à démarrer la partie
    public static int playersReady;

    //Enumération des types de bombes
    public enum bombType
    {
        PAWN,       //Pion
        QUEEN,      //Reine
        KNIGHT,     //Cavalier
        ROOK,       //Tour
        BISHOP,     //Fou
        KING        //Roi
    };

    
    static public bool AmIThisGuy(string ID)
    {
        return (Network.player.ToString() == ID);
    }

    //Enumération des types de terrain.
    public enum gameRule
    {
        CLASSIC,
        CLASSIC_HOLES,
        ICE,
        ICE_HOLES,
		CLASSIC_SPEEDWALK,
		CLASSIC_HOLES_SPEEDWALK,
        NB_ELEMENTS //Doit toujours être en dernier, donne le nombre de terrain
    };

    //Régles de la partie
    static public GameRules rule;
    static public int ruleID;
    static public bool solo;
}
