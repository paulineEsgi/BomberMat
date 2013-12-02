/****
 * Script qui gère les flammes
 */
using UnityEngine;
using System.Collections;

public class FireScript : MonoBehaviour {

    public float timeBeforeDie = 0.5f; //Durer de vie d'une flamme

    private float time;
	// Use this for initialization
	void Start () {
        time = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        //A la fin de la vie de la flamme, on détruit l'objet
        if (Time.time - time >= timeBeforeDie)
        {
            Destroy(gameObject);
        }
	}
}
