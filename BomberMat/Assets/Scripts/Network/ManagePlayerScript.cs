using UnityEngine;
using System.Collections;

public class ManagePlayerScript : MonoBehaviour
{

    public float waitingTime = 5f;
    private float time;

    private int sec;
    public GameObject cmpt;
    private TextMesh tm;
    private MeshRenderer mr;

    public GameObject[] playerList;

    void Start()
    {
        tm = (TextMesh)cmpt.GetComponent<TextMesh>();
        mr = cmpt.GetComponent<MeshRenderer>();
        mr.enabled = false;
        foreach (GameObject pl in playerList)
            pl.SetActive(false);
        for (int i = 0; i < StaticBoard.namePlayers.Count; i++)
        {
            string name = (string)StaticBoard.namePlayers[i];
            networkView.RPC("setList", RPCMode.All, name, PlayerPrefs.GetInt(name), i);
        }

    }
    void OnGUI()
    {
        /*GUI.Box(new Rect(0, 0, 100, 50),"id player : "+ Network.player.ToString() );
        GUI.Box(new Rect(0, 50, 100, 50), "temps : " + (Time.time - time));
        GUI.Box(new Rect(200, 0, 100, 50), "count : " + StaticBoard.players.Count);
        for (int i = 0; i < StaticBoard.players.Count; i += 1)
            GUI.Box(new Rect(100, 50 * i, 100, 50), "players connected : " + StaticBoard.players[i]);*/
    }

    void FixedUpdate()
    {
        if(Network.isServer)
        {
            if (StaticBoard.players.Count == StaticBoard.playersReady && Network.connections.Length >= 2)
            {
                networkView.RPC("showTimer", RPCMode.All, true);
            }
            else
                networkView.RPC("showTimer", RPCMode.All, false);
            if (Time.time - time > waitingTime && Network.connections.Length >= 2 && StaticBoard.players.Count == StaticBoard.playersReady)
            {
                networkView.RPC("sendRulesToPlayers", RPCMode.Others, StaticBoard.ruleID);
                networkView.RPC("LaunchGame", RPCMode.All);
            }
        //compteur de secondes d'attentes
            sec = (int)(waitingTime - (Time.time - time));
            networkView.RPC("sendTime", RPCMode.Others, sec);
       
        }
        tm.text = sec.ToString();
        if (Network.isServer)
        {
            for (int i = 0; i < StaticBoard.namePlayers.Count; i += 1)
            {

                string name = (string)StaticBoard.namePlayers[i];
                networkView.RPC("setList", RPCMode.All, name, PlayerPrefs.GetInt(name), i);
            }
        }
    }

    void OnPlayerConnected(NetworkPlayer player)
    {
        if (Network.connections.Length > 1)
        {
            time = Time.time;
        }
        StaticBoard.players.Add(player.ToString());
        if (Network.isServer)
        {
            for (int i = 0; i < StaticBoard.namePlayers.Count; i += 1)
            {

                string name = (string)StaticBoard.namePlayers[i];
                networkView.RPC("setList", RPCMode.All, name, PlayerPrefs.GetInt(name), i);
            }
        }
    }

    void OnPlayerDisconnected(NetworkPlayer player)
    {
        if (Network.isServer)
        {
            for (int i = 0; i < StaticBoard.namePlayers.Count; i += 1)
            {
                string name = (string)StaticBoard.namePlayers[i];
                networkView.RPC("setList", RPCMode.All, name, PlayerPrefs.GetInt(name), i);
            }
        }
        Network.RemoveRPCs(player);
    }


    [RPC]
    void setList(string name, int score, int ID)
    {
        playerList[ID].SetActive(true);
        TextMesh tmpname = playerList[ID].GetComponent<SetScoreScript>()._name;
        TextMesh tmpscore = playerList[ID].GetComponent<SetScoreScript>()._score;

        tmpname.text = name;
        tmpscore.text = score.ToString();
    }

    [RPC]
    void sendRulesToPlayers(int ruleID)
    {
        DefRules.starting();
        StaticBoard.ruleID = ruleID;
        StaticBoard.rule = DefRules.rules[ruleID];
    }

    [RPC]
    void showTimer(bool value)
    {
        mr.enabled = value;
    }

    [RPC]
    void sendTime(int secondes)
    {
        sec = secondes;
    }

    [RPC]
    void LaunchGame()
    {
        StaticBoard.gamePlayers.Clear();
        foreach (object pl in StaticBoard.players)
            StaticBoard.gamePlayers.Add(pl);
        StaticBoard.playersReady = 0;
        Debug.Log(StaticBoard.gamePlayers.Count);
        Application.LoadLevel("scene2");
    }

    public void setTime()
    {
        time = Time.time;
    }

}
