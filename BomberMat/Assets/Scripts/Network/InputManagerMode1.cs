using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManagerMode1 : MonoBehaviour {

    public Transform[] Players;

    class PlayerIntents
    {
        public bool _wantToMoveUp = false;
        public bool _wantToMoveDown = false;
        public bool _wantToMoveRight = false;
        public bool _wantToMoveLeft = false;
    }

    private Dictionary<NetworkPlayer, PlayerIntents> _playersIntents;
    private Dictionary<NetworkPlayer, PlayerIntents> PlayersIntents
    {
        get { return _playersIntents; }
        set { _playersIntents = value; }
    }

    private NetworkView _myNetworkView = null;

	void Start () {
        PlayersIntents = new Dictionary<NetworkPlayer, PlayerIntents>();
        _myNetworkView = this.gameObject.GetComponent<NetworkView>();
	}

    void OnPlayerConnected(NetworkPlayer p)
    {
        PlayersIntents.Add(p, new PlayerIntents());
        _myNetworkView.RPC("NewPlayerConnected", RPCMode.OthersBuffered, p);
    }

    [RPC]
    void NewPlayerConnected(NetworkPlayer p)
    {
        PlayersIntents.Add(p, new PlayerIntents());
    }

	void Update () {

        /*for (int i = 0; i < Players.Length; i++ )
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _myNetworkView.RPC("PlayerWantToMoveDown", RPCMode.Server, Network.player);
                Players[i].GetComponent<PlayerScript>().MovePlayer(1); Debug.Log("move");
            }
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                _myNetworkView.RPC("PlayerWantToMoveDown", RPCMode.Server, Network.player);
                Players[i].GetComponent<PlayerScript>().MovePlayer(0);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _myNetworkView.RPC("PlayerWantToMoveUp", RPCMode.Server, Network.player);
                Players[i].GetComponent<PlayerScript>().MovePlayer(2); Debug.Log("move");
            }
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                _myNetworkView.RPC("PlayerWantToMoveUp", RPCMode.Server, Network.player);
                Players[i].GetComponent<PlayerScript>().MovePlayer(3);
            }
        }*/
	}

   /*void FixedUpdate()
    {
        int i = 0;
        foreach (var p in PlayersIntents)
        {
            if (p.Value._wantToMoveDown)
            {
                Players[i].GetComponent<PlayerScript>().MovePlayer(1); Debug.Log("move");
            }
            if (p.Value._wantToMoveUp)
            {
                Players[i].GetComponent<PlayerScript>().MovePlayer(0);
            }
            if (p.Value._wantToMoveRight)
            {
                Players[i].GetComponent<PlayerScript>().MovePlayer(2); Debug.Log("move");
            }
            if (p.Value._wantToMoveLeft)
            {
                Players[i].GetComponent<PlayerScript>().MovePlayer(3);
            }
            i++;
        }
    }*/

    [RPC]
    void PlayerWantToMoveUp(NetworkPlayer p)
    {
        if (Network.isServer)
        {
            _myNetworkView.RPC("PlayerWantToMoveUp", RPCMode.Others, p);
        }
    }

    [RPC]
    void PlayerWantToMoveDown(NetworkPlayer p)
    {
        if (Network.isServer)
        {
            _myNetworkView.RPC("PlayerWantToMoveDown", RPCMode.Others, p);
        }
    }
    [RPC]
    void PlayerWantToMoveRight(NetworkPlayer p)
    {
        
        if (Network.isServer)
        {
            _myNetworkView.RPC("PlayerWantToMoveRight", RPCMode.Others, p);
        }
    }

    [RPC]
    void PlayerWantToMoveLeft(NetworkPlayer p)
    {
        
        if (Network.isServer)
        {
            _myNetworkView.RPC("PlayerWantToMoveLeft", RPCMode.Others, p);
        }
    }
}
