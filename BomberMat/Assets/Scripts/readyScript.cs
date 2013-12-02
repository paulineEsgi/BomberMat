/****
 * Script le bouton prêt dans le lobby
 */
using UnityEngine;
using System.Collections;

public class readyScript : MonoBehaviour {

    public Material red;
    public Material green;

    private MeshRenderer color;

    [SerializeField]
    private GameObject networkManager;

    private ManagePlayerScript MPScript;

    private bool ready;


	// Use this for initialization
	void Start () {
        color = gameObject.GetComponent<MeshRenderer>();
        ready = false;
        color.material = red;
        MPScript = networkManager.GetComponent<ManagePlayerScript>();
	}
	
	// Update is called once per frame
	void Update () {

        //regarde où le joueur clique
        var ray = new Ray();
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hit = new RaycastHit();
        if (Input.GetMouseButtonDown(0))
            if (collider.Raycast(ray, out hit, 100.0f))
            {
                clicReady();
                networkView.RPC("sayReadyToServ", RPCMode.Server, ready);
            }

        if (collider.Raycast(ray, out hit, 100.0f))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.green, 1);
        }
	}

    //Fonction appelé lors de l'appuie sur le bouton
    void clicReady()
    {
        //S'il était déjà prêt, il inverse le choix
        if (ready)
        {
            ready = false;
            color.material = red;
        }
        else
        {
            ready = true;
            color.material = green;
        }
    }

    //Fonction envoyé au serveur pour dire qu'un joueur est prêt
    [RPC]
    void sayReadyToServ(bool b)
    {
        if (!b)
        {
            StaticBoard.playersReady--;
        }
        else
        {
            StaticBoard.playersReady++;
            MPScript.setTime();
        }
    }
}
