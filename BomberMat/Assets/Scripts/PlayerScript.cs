using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    public GameObject _selector; //Affichage des prochaines bombes
    public float player_speed=3; //vitesse du joueur
    
    private Vector3 lastBombPosition; //position de la dernière bombe posée
    private Vector3 roundedPlayerPosition; //position arroudie du joueur

    public GameObject winText;  //Text qui s'affiche lorsque le joueur gagne
    private MeshRenderer winTextMR;
	
	public GameObject looseText;
	public AudioSource winSound;
	public AudioSource looseSound;

    private float timeBeforeNext = 5f; //Temps d'arrêt à la fin d'une partie gagnée
    private float time = 5f;

    private SelectBombScript selectorScript; //Script de sélection des prochaines bombes
    

	void Start () {
		selectorScript = _selector.GetComponent<SelectBombScript>();
        transform.rigidbody.drag = StaticBoard.rule.getPlayerDrag();
        selectorScript.Init(); //Initialisation des bombes
        winTextMR = winText.GetComponent<MeshRenderer>();
	}

    void FixedUpdate()
    {
        //Le serveur n'a pas le droit de bouger
        if (!Network.isServer)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.rigidbody.AddForce(Vector3.forward * Time.deltaTime * player_speed * 100 * BonusScript.run);
                transform.localEulerAngles = new Vector3(transform.rotation.x, 0, transform.rotation.z);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.rigidbody.AddForce(Vector3.back * Time.deltaTime * player_speed * 100 * BonusScript.run);
                transform.localEulerAngles = new Vector3(transform.rotation.x, 180, transform.rotation.z);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.rigidbody.AddForce(Vector3.right * Time.deltaTime * player_speed * 100 * BonusScript.run);
                transform.localEulerAngles = new Vector3(transform.rotation.x, 90, transform.rotation.z);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.rigidbody.AddForce(Vector3.left * Time.deltaTime * player_speed * 100 * BonusScript.run);
                transform.localEulerAngles = new Vector3(transform.rotation.x, 270, transform.rotation.z);
            }
        }
       

        //mort du joueur s'il tombe sur le bord de droite
        if (transform.position.x <= -0.5)
        {
            rigidbody.useGravity = true;
            transform.rigidbody.constraints = RigidbodyConstraints.None;
            transform.rigidbody.AddForce(Vector3.left * Time.deltaTime* 200);
            if (transform.position.y < -10)
                Die();
        }
    }

    void Update()
    {
        //poser une bombe
        if (Input.GetKeyDown(KeyCode.Space) && !Network.isServer  && StaticBoard.bombDropped < BonusScript.bomb)
        {
            DropBomb();
        }

        //Affichage d'une victoire
        if (winTextMR.enabled)
        {
            if (Time.time - time > timeBeforeNext)
            {
                winTextMR.enabled = false;
                networkView.RPC("finish", RPCMode.All);
            }
        }
        //Sert à mettre à jour le rigidbody pour le calcul de collision, sans ça un joueur immobile ne craint pas les explosions
        transform.rigidbody.AddForce(Vector3.back * Time.deltaTime);
    }

    //Fonction qui sert à poser une bombe
    void DropBomb()
    {
        //Compte le nombre de bombes posées
        StaticBoard.bombDropped++;


        roundedPlayerPosition = new Vector3((float)((int)(transform.position.x + 0.5)),
                                        0,
                                        (float)((int)(transform.position.z + 0.5)));
        
       if (StaticBoard.bomb[(int)roundedPlayerPosition.x][(int)roundedPlayerPosition.z] == null)//ne pose pas une bombe sur une autre
       {
           GameObject bomb = null;
            //charge le prefab Bomb du type correspondant
           switch (selectorScript.GetNextBomb())
           {
               case (int)StaticBoard.bombType.BISHOP:
                   bomb = (GameObject)Instantiate(Resources.Load("Prefab/Bomb/Bishop"));
                   bomb.transform.position = roundedPlayerPosition;
                   break;
               case (int)StaticBoard.bombType.KING:
                   bomb = (GameObject)Instantiate(Resources.Load("Prefab/Bomb/King"));
                   bomb.transform.position = roundedPlayerPosition;
                   break;
               case (int)StaticBoard.bombType.KNIGHT:
                   bomb = (GameObject)Instantiate(Resources.Load("Prefab/Bomb/Knight"));
                   bomb.transform.position = roundedPlayerPosition;
                   break;
               case (int)StaticBoard.bombType.QUEEN:
                   bomb = (GameObject)Instantiate(Resources.Load("Prefab/Bomb/Queen"));
                   bomb.transform.position = roundedPlayerPosition;
                   break;
               case (int)StaticBoard.bombType.PAWN:
                   bomb = (GameObject)Instantiate(Resources.Load("Prefab/Bomb/Pawn"));
                   bomb.transform.position = roundedPlayerPosition;
                   break;
               case (int)StaticBoard.bombType.ROOK:
                   bomb = (GameObject)Instantiate(Resources.Load("Prefab/Bomb/Rook"));
                   bomb.transform.position = roundedPlayerPosition;
                   break;
           }
           
           if(bomb !=null)
               StaticBoard.bomb[(int)roundedPlayerPosition.x][(int)roundedPlayerPosition.z] = bomb;
        }
    }

    //Détection d'une collision mortelle pour le joueur
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "fire")
        {
            Die();
        }

        if (collision.collider.tag == "spike")
        {
            Die();
        }
    }

    //Fonction de mort pour le joueur
    public void Die()
    {
		looseSound.Play();
		looseText.GetComponent<MeshRenderer>().enabled=true;
        if (!StaticBoard.solo)
        {
            //Envoie au serveur qui est mort
            networkView.RPC("IAmDying", RPCMode.Server, Network.player.ToString());
            //Demande les points de la partie au serveur
            networkView.RPC("givePoints", RPCMode.Server, GameObject.FindGameObjectWithTag("net").GetComponent<StartNetwork>()._name, false);
            //Returne au lobby
            Application.LoadLevel("waitingRoom");
        }
        else
            Application.LoadLevel(0);
        
    }

    //Fonction qui sert à informer le serveur de la mort d'un joueur
    [RPC]
    void IAmDying(string id)
    {
        if (Network.isServer)
        {
            //On retire le joueur de la liste des participant
            StaticBoard.gamePlayers.RemoveAt(StaticBoard.gamePlayers.IndexOf(id));
            //S'il en reste qu'un, il gagne
            if (StaticBoard.gamePlayers.Count < 2)
            {
                networkView.RPC("IWon", RPCMode.Others);
            }
        }
    }

    //Fonction de victoire
    [RPC]
    void IWon()
    {
		winSound.Play();
        networkView.RPC("givePoints", RPCMode.Server, GameObject.FindGameObjectWithTag("net").GetComponent<StartNetwork>()._name, true);
        winTextMR.enabled = true;
        time = Time.time;
    }

    //Fonction pour attribuer les points
    [RPC]
    void givePoints(string pseudo, bool winner)
    {
        if (Network.isServer)
        {            
            int points = (8 - StaticBoard.gamePlayers.Count) * 2 + 1;
            if (winner)
                points += 2;
            PlayerPrefs.SetInt(pseudo, PlayerPrefs.GetInt(pseudo) + points);
        }
    }

    //Fonction pour forcer tout le monde à revenir au lobby
    [RPC]
    void finish()
    {
        Application.LoadLevel("waitingRoom");
    }
}
