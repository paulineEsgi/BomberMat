/**
 *by Jules Maurer
 *
 * Desc :
 * Script for buttons, enter a var as an action.
 * 
 * creation : 12/12/12
 * last modification 12/02/13
 * 
**/

using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour
{
    /**
     * Name of the action to do
     */
    public string action;

    //private MeshRenderer texte;

    void Start()
    {
      //  texte = GetComponentInChildren<MeshRenderer>();
    }



    // Update is called once per frame
    void Update()
    {
        var ray = new Ray();
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hit = new RaycastHit();
        if (Input.GetMouseButtonDown(0))
            if (collider.Raycast(ray, out hit, 100.0f))
            {
                if (action.Equals("start"))
                {
                    Application.LoadLevel("waitingRoom");
                }
                else if (action.Equals("quit"))
                {
                    if (Application.loadedLevel == 0)
                        Application.Quit();
                    QuitGame();
                }
            }

        if (collider.Raycast(ray, out hit, 100.0f))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.green, 1);
          //  texte.material.color = new Color(01 / 255, 47 / 255, 98 / 2);
        }
        else
        {
           // texte.material.color = Color.white;
        }
    }

    [RPC]
    void QuitGame()
    {
        if (Network.isServer)
        {
            networkView.RPC("QuitGame", RPCMode.Others);
            PlayerPrefs.DeleteAll();
            StaticBoard.namePlayers.Clear();
            StaticBoard.gamePlayers.Clear();
            StaticBoard.players.Clear();
            Network.Disconnect();
            Application.LoadLevel("Diapo");
        }
        else
        {
            GameObject.FindGameObjectWithTag("player").GetComponent<PlayerScript>().Die();
            networkView.RPC("SayToServerIQuit", RPCMode.Server, GameObject.FindGameObjectWithTag("net").GetComponent<StartNetwork>()._name, Network.player);
            PlayerPrefs.DeleteAll();
            Network.Disconnect();
            Application.LoadLevel("Diapo");
        }
    }

    [RPC]
    void SayToServerIQuit(string name, NetworkPlayer id)
    {
        PlayerPrefs.DeleteKey(name);
        StaticBoard.namePlayers.Remove(name);
        Network.RemoveRPCs(id);       
    }

}
