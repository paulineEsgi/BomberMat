using UnityEngine;
using System.Collections;

public class BonusScript : MonoBehaviour {

    static public bool promo;
    static public float run;
    static public int bomb;

	// Use this for initialization
	void Start () {
        promo = false;
        run = 1;
        bomb = 1;
	}
	
	// Update is called once per frame
	void Update () {

	}

    //Si le joueur touche un bonus
    void OnCollisionEnter(Collision collision)
    {
        //Transforme le prochain pion en une autre pièce
        if (collision.collider.tag == "bonusPromo")
        {
            promo = true;
            //Détruit le bonus
            Destroy(collision.gameObject);
        }
        //Augmente la vitesse du joueur
        if (collision.collider.tag == "bonusRun")
        {
            run = run < 2 ? run += 0.1f : run;
            //Détruit le bonus
            Destroy(collision.gameObject);
        }
        //Augmente le nombre de bombe pouvant être posé
        if (collision.collider.tag == "bonusBomb")
        {
            bomb = bomb < 5 ? bomb+=1 : bomb;
            //Détruit le bonus
            Destroy(collision.gameObject);
        }
        
    }
}
