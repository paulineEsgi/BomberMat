using UnityEngine;
using System.Collections;

public class StartNetwork : MonoBehaviour {

    public bool isServer;
    public int listenPort = 25000; //port d'écoute du serveur
    public string remoteIP; //adresse IP du serveur
    public string _name;

    void Awake()
    {
        //ne pas détruire le script et le gameObject au chargement
        DontDestroyOnLoad(this);
    }

	void Start () {
        
        // si c'est le serveur, l'initialiser
        if (isServer)
        {
            Network.InitializeSecurity();
            Network.InitializeServer(8, listenPort, false); //TODO limiter le nb de joueur selon le monde

            // prévenir tous les objets que le réseau est lancé
            foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
                go.SendMessage("OnNetworkLoadedLevel", SendMessageOptions.DontRequireReceiver);

            Application.LoadLevel("menuServer");
        }
        // sinon connecter les clients au server
        else
        {
            Network.Connect(remoteIP, listenPort);
            //Les clients sont directement envoyé dans la salle d'attente
            Application.LoadLevel("waitingRoom");
        }
	}

    void OnConnectedToServer()
    {
       
    }

    void OnFailedToConnect(NetworkConnectionError error)
    {
        Debug.Log("Could not connect to server: " + error);
        Application.LoadLevel("Diapo");
    }

    void OnPlayerConnected(NetworkPlayer player)
    {
        if (isServer)
        {
            Debug.Log("Connecté !");
            networkView.RPC("AskSession", player, player);
        }
    }

    [RPC]
    void AskSession(NetworkPlayer player)
    {
        //On test si le joueur à une session (qui se ferme à l'arrêt normal du jeu)
        if (isSessionExist(_name))
        {
            Debug.Log("session existante");
            //Si oui, on demande au serveur si la session du joueur existe dans la partie en cours
            networkView.RPC("testSession", RPCMode.Server, _name, player);
        }
        else
        {
            //Si non on crée la session chez le joueur et chez le serveur
            StartSession();
            networkView.RPC("StartSessionS", RPCMode.Server, _name);
        }
    }
    //Fonction de création de session
    [RPC]
    void StartSession()
    {
        Debug.Log(_name);
        PlayerPrefs.SetInt(_name, 0);
    }

    //Fonction de création de session
    [RPC]
    void StartSessionS(string name)
    {
        PlayerPrefs.SetInt(name, 0);
        if (isServer)
        {
            StaticBoard.namePlayers.Add(name);
        }
    }

    //Fonction de fermeture de session
    [RPC]
    void CloseSession()
    {
        PlayerPrefs.DeleteAll();
    }

    [RPC]
    void testSession(string key, NetworkPlayer player)
    {
        //Test si la session du joueur existe
        if(!isSessionExist(key))
        {
            //Si non, on ferme la session du joueur et on en ouvre une nouvelle
            StartSessionS(_name);
            networkView.RPC("StartSession", player);
        }
    }

    bool isSessionExist(string key)
    {
        return PlayerPrefs.HasKey(key);
    }
    
    void OnLevelWasLoaded()
    {
        if ( Application.loadedLevelName == "Diapo")
        Destroy(this.gameObject);
        // prévenir tous les objets que le réseau et le jeu sont lancés
        foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
            go.SendMessage("OnNetworkLoadedLevel", SendMessageOptions.DontRequireReceiver);
    }

    void OnPlayerDisconnected(NetworkPlayer player)
    {
       Network.RemoveRPCs(player);
    }


	void Update () {
	
	}
}
