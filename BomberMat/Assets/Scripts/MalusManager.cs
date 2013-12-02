/****
 * Script qui gère les malus
 */
using UnityEngine;
using System.Collections;

public class MalusManager : MonoBehaviour {

    public GameObject _generate; //GO comprenant le script de génération de ligne
    public GameObject _selector; //GO comprenant le script de sélection de bombe 

    private GenerateLineScript generateLineScript;
    private SelectBombScript selectorScript;

    private int nbRoque = 5; //nombre de roque pour le malus roque
    private static MalusManager instance;
    public static MalusManager Instance
    {
        get { return instance; }
    }


    public GameObject malusText;
    private float timeMalusAnimDurtion = 3f;
    private float time;


	// Use this for initialization
	void Start () {
        generateLineScript = _generate.GetComponent<GenerateLineScript>();
        selectorScript = _selector.GetComponent<SelectBombScript>();
        nbRoque = 6;
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
        //apparation/disparition du message de malus
        if (malusText.GetComponent<MeshRenderer>().enabled)
            if (Time.time - time > timeMalusAnimDurtion)
                malusText.GetComponent<MeshRenderer>().enabled = false;
	}

    //Fonction appelé pour dire au serveur d'envoyer un malus à un adversaire
    public void sendMalusToServer()
    {
        networkView.RPC("sendMalus", RPCMode.Server, Network.player.ToString());
    }

    //Fonction qui envoie le malus
    [RPC]
    void sendMalus(string ID)
    {
        int IdSender = StaticBoard.players.IndexOf(ID);
        int IdReceiver = 0;

        if (IdSender == StaticBoard.players.Count-1)
            IdReceiver = 0;
        else
            IdReceiver = IdSender + 1;
        networkView.RPC("chooseMalus", RPCMode.Others, StaticBoard.gamePlayers[IdReceiver]);

    }

    //Fonction de choix de malus, l'id est la victime
    [RPC]
    void chooseMalus(string ID)
    {
        if (StaticBoard.AmIThisGuy(ID))
        {
            switch ((int)Random.Range(0, 3))
            {
                case 0:
                    addBlocks();
                    break;
                case 1:
                    roquer();
                    break;
                case 2:
                    newLine();
                    break;
                default:
                    break;
            }
            malusAnim();
        }
    }

    //Fonction qui fait apparaitre le texte malus
    void malusAnim()
    {
        if (!malusText.GetComponent<MeshRenderer>().enabled)
        {
            malusText.GetComponent<MeshRenderer>().enabled = true;
            time = Time.time;
        }


    }
    
    //Ajoute des blocs indestructible de manière aléatoire chez un adversaire
    void addBlocks()
    {
        int nbBlocks = (int)Random.Range(2, 5);
        int x=(int)Random.Range(0, StaticBoard.sizeX), y=(int)Random.Range(0, StaticBoard.sizeZ);
        for (int i = 0; i < nbBlocks; i += 1) 
            while (StaticBoard.map[x][y] != null)
            {
                x = (int)Random.Range(0, StaticBoard.sizeX);
                y = (int)Random.Range(0, StaticBoard.sizeZ);
            }

        StaticBoard.map[x][y] = Instantiate(Resources.Load("Prefab/IndestructibleBloc"), new Vector3((float)x, 0f, (float)y), Quaternion.identity) as GameObject;
    }

    //Fonction dit au sélecteur que les prochaines tours seront des rois
    void roquer()
    {
        selectorScript.setRoque(nbRoque);
    }

    //Fonction qui invoke instantanément une nouvelle ligne
    void newLine()
    {
        generateLineScript.CreateLine();
    }


}
