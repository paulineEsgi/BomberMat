/****
 * Script qui gère les bombes
 */
using UnityEngine;
using System.Collections;

public class BombScript : MonoBehaviour {

    
    //Direction des flammes
    enum direction
    {
        NORTH,
        SOUTH,
        EAST,
        WEST,
        SOUTH_EAST,
        SOUTH_WEST,
        NORTH_EAST,
        NORTH_WEST
    }



    public float timeBeforeExplosion = 5f; //Temps avant explosion
    public GameObject _fire; //Objet désignant une flamme

    private float time; 
    public int _type; //Type de la bombe

    private bool show = false; 



	void Start () {
        if(!show)
        time = Time.time;
	}
	

	void Update () {

        //Si le temps est écoulé, la bombe explose
        if (Time.time - time >= timeBeforeExplosion && !show)
        {
            explosion(_type);

            Destroy(gameObject);
        }
	}

    //une bombe est posée directement sur le joueur, il peut passer à travers pour partir mais après il est bloqué
    void OnTriggerExit()
    {
        transform.collider.isTrigger = false;
    }

    public void setShow(bool value)
    {
        show = value;
    }

    //Cette fonction sert à créer les patterns des bombes
    void explosion(int type)
    {
        //Compte le nombre de bombes explosées
        StaticBoard.bombDropped--;
        int x = (int) transform.localPosition.x;
        int z = (int) transform.localPosition.z;
        switch (type)
        {
            case (int)StaticBoard.bombType.PAWN:
                //explosion dans la direction de l'autre joueur ou vers le haut
                longFire(x, z, (int)direction.NORTH, 3);
                break;

            case (int)StaticBoard.bombType.KING:
                //petit +
                createFire(x, z);
                createFire(x - 1, z);
                createFire(x + 1, z);
                createFire(x, z - 1);
                createFire(x, z + 1);
                createFire(x - 1, z - 1);
                createFire(x + 1, z - 1);
                createFire(x - 1, z + 1);
                createFire(x + 1, z + 1);
                break;

            case (int)StaticBoard.bombType.KNIGHT:
                //Huit cases des huit possibilités d'un cavalier
                createFire(x, z);
                createFire(x - 1, z - 2);
                createFire(x - 1, z + 2);
                createFire(x - 2, z - 1);
                createFire(x - 2, z + 1);
                createFire(x + 1, z + 2);
                createFire(x + 1, z - 2);
                createFire(x + 2, z - 1);
                createFire(x + 2, z + 1);
                break;

            case (int)StaticBoard.bombType.QUEEN:
                //Gros + et x en même temps
                longFire(x, z, (int)direction.NORTH, 8);
                longFire(x, z, (int)direction.SOUTH, 8);
                longFire(x, z, (int)direction.EAST, 8);
                longFire(x, z, (int)direction.WEST, 8);
                longFire(x, z, (int)direction.NORTH_WEST, 8);
                longFire(x, z, (int)direction.NORTH_EAST, 8);
                longFire(x, z, (int)direction.SOUTH_EAST, 8);
                longFire(x, z, (int)direction.SOUTH_WEST, 8);
                break;

            case (int)StaticBoard.bombType.ROOK:
                //Gros +
                longFire(x, z, (int)direction.NORTH, 8);
                longFire(x, z, (int)direction.SOUTH, 8);
                longFire(x, z, (int)direction.EAST, 8);
                longFire(x, z, (int)direction.WEST, 8);
                break;

            case (int)StaticBoard.bombType.BISHOP:
                //Gros x
                longFire(x, z, (int)direction.NORTH_WEST, 8);
                longFire(x, z, (int)direction.NORTH_EAST, 8);
                longFire(x, z, (int)direction.SOUTH_EAST, 8);
                longFire(x, z, (int)direction.SOUTH_WEST, 8);
                break;

            default :
                break;

        }
    }

    //Fonction qui instantie les flammes
    void createFire(int x, int y)
    {
        if (!testCollision(x, y))
        {
            Instantiate(_fire, new Vector3(x, 0, y), Quaternion.identity);
        }   
    }

    //Fonction récursive servant à créer les flammes contigue
    void longFire(int x, int y, int dir, int range)
    {

        //S'il n'y a pas de collision et que la portée de la flamme n'est pas encore atteinte
        if (!testCollision(x, y) && range > 0)
        {
           //On crée la flamme...
            Instantiate(_fire, new Vector3(x, 0, y), Quaternion.identity);
            //Puis en fonction de sa direction on continu la propagation
            switch (dir)
            {
                case (int)direction.NORTH :
                    longFire(x + 1, y, dir, range - 1);
                    break;
                case (int)direction.SOUTH :
                    longFire(x - 1, y, dir, range - 1);
                    break;
                case (int)direction.EAST:
                    longFire(x, y+1, dir, range - 1);
                    break;
                case (int)direction.WEST:
                    longFire(x, y-1, dir, range - 1);
                    break;
                case (int)direction.NORTH_EAST:
                    longFire(x + 1, y+1, dir, range - 1);
                    break;
                case (int)direction.NORTH_WEST:
                    longFire(x + 1, y-1, dir, range - 1);
                    break;
                case (int)direction.SOUTH_EAST:
                    longFire(x - 1, y+1, dir, range - 1);
                    break;
                case (int)direction.SOUTH_WEST:
                    longFire(x - 1, y-1, dir, range - 1);
                    break;
            }
        }  
    }

    //Fonction de test de collision
    bool testCollision(int x, int y)
    {
        //Il y a collision si on est contre un mur
        if (x < 0 || y < 0 || x >= StaticBoard.sizeX || y >= StaticBoard.sizeZ)
            return true;

        //Il y a collision si on tape contre un bloc
        if (StaticBoard.map[x][y] != null)
        {
            //S'il est destructible... on le détruit
            if (StaticBoard.map[x][y].tag == "destructible")
            {
                Destroy(StaticBoard.map[x][y]);

                //Si la map est avec piques, il y a une chance pour qu'un bloque de pique se crée
                if (StaticBoard.rule.getIsHoles() && Random.Range(0,6) > 4)
                {
                    StaticBoard.map[x][y] = Instantiate(Resources.Load("Prefab/Bomb/Spike"), new Vector3((float)x, 0f, (float)y), Quaternion.identity) as GameObject;
                }
                //De même pour les accélérateur
                else if (StaticBoard.rule.getIsSpeedWalk() && Random.Range(0, 8) > 4)
                {
                    int rdm = Random.Range(0, 3);
                    Debug.Log(rdm);
                    Quaternion rot;
                    if (rdm == 1)
                        rot = new Quaternion(0, 90f, 0, 0);
                    if (rdm == 2)
                        rot = new Quaternion(0, 180f, 0, 0);
                    if (rdm == 3)
                        rot = new Quaternion(0, 270f, 0, 0);
                    else rot = Quaternion.identity;
                    StaticBoard.map[x][y] = Instantiate(Resources.Load("Prefab/speedWalk"), new Vector3((float)x, -0.4f, (float)y), rot) as GameObject;
                }
                //Sinon on laisse un vide
                else
                {
                    StaticBoard.map[x][y] = null;
                    //Qui peut être comblé par un bonus
                    if (Random.Range(0, 5) == 4)
                    {
                        int rand = Random.Range(0, 3);
                        if (rand == 0)
                            StaticBoard.map[x][y] = Instantiate(Resources.Load("Prefab/Bonus1"), new Vector3((float)x, 0f, (float)y), Quaternion.identity) as GameObject;
                        if (rand == 1)
                            StaticBoard.map[x][y] = Instantiate(Resources.Load("Prefab/Bonus2"), new Vector3((float)x, 0f, (float)y), Quaternion.identity) as GameObject;
                        if (rand == 2)
                            StaticBoard.map[x][y] = Instantiate(Resources.Load("Prefab/Bonus3"), new Vector3((float)x, 0f, (float)y), Quaternion.identity) as GameObject;
                    }
                }
                //On test si une ligne est détruite
                if (testLine(x) && !StaticBoard.solo)
                    MalusManager.Instance.sendMalusToServer();
            }
            return true;
        }
        //Si on touche une autre bombe, explosion en chaine
        if (StaticBoard.bomb[x][y] != null)
        {
            StaticBoard.bomb[x][y].GetComponent<BombScript>().timeBeforeExplosion = 0;
        }
        return false ;
    }

    //Fonction qui vérifie si une ligne est détruite
    bool testLine(int x)
    {
        for (int i = 0; i < StaticBoard.sizeZ; i += 1)
        {
            if(StaticBoard.map[x][i] != null)
                if (StaticBoard.map[x][i].tag == "destructible")
                {
                    return false;
                }
        }
        return true;
    }
}
