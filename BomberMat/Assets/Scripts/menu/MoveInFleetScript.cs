/**
 *by Jules Maurer
 *
 * Desc :
 * Script for move in gest fleet
 * 
 * 
**/
using UnityEngine;
using System.Collections;

public class MoveInFleetScript : MonoBehaviour {

    public GameObject _prefab;

    public GameObject _arrowLeft;
    public GameObject _arrowRight;
    public GameObject[] _tabs;
	public Material[] _mat;

    private GameObject[][] gameMode;

    private int actualY;
    private int[] actualX;

    private GameObject substitu;



	// Use this for initialization
    void Start()
    {
        DefRules.starting();
        gameMode = GetComponent<CreatFleetScript>().myTab;
        this.actualX = new int[gameMode.Length];
        Vector3 pos = new Vector3(0, 0, 0);
        Quaternion angle = Quaternion.Euler(new Vector3(270, 0, 0));
        for (var i = 0; i < gameMode.Length; i++)
        {
            if (this.gameMode[i].Length != 0)
            {
                pos = new Vector3(0, -i * 7, 0);
                //this.gameMode[i][0] = myInstantiate(pos, angle, "Mode" + (i+1) + " - 1 " );
				this.gameMode[i][0] = myInstantiate(pos, angle, "Arene " + (i+1) );
				this.gameMode[i][0].GetComponent<MeshRenderer>().material = _mat[i];
            }
            else
            {
                //On grise l'onglet et on en parle plus
                this._tabs[i].SetActive(false);
            }
            this.actualX[i] = 0;
        }

        StaticBoard.rule = DefRules.rules[actualY];
        
        setCard();
    }
	
	// Update is called once per frame
    void Update()
    {

    }

    GameObject myInstantiate(Vector3 f_pos, Quaternion f_angle, string f_text)
    {
        GameObject inter = Instantiate(_prefab, f_pos, f_angle) as GameObject;
        inter.GetComponentInChildren<TextMesh>().text = f_text;
        return inter;
    }

    public void updateXbyRight()
    {
        //this.destroyCard();
        if (this.gameMode[actualY].Length == 2)
        {
            Destroy(this.substitu);
        }
        else
        {
            Destroy(this.gameMode[actualY][this.actualX[this.actualY] > 0 ? this.actualX[this.actualY] - 1 : this.gameMode[this.actualY].Length - 1]);
        }
        Destroy(this.gameMode[actualY][actualX[this.actualY]]);
        this.actualX[this.actualY] = ++this.actualX[this.actualY] % this.gameMode[actualY].Length;
        this.setCard();
    }

    public void updateXbyLeft()
    {
        if (this.gameMode[actualY].Length == 2)
        {
            GameObject inter = this.substitu;
            this.substitu = this.gameMode[actualY][actualX[this.actualY] == 0 ? 1 : 0];
            this.gameMode[actualY][actualX[this.actualY] == 0 ? 1 : 0] = inter;
            Destroy(this.substitu);
        }
        else
        {
            Destroy(this.gameMode[actualY][this.actualX[this.actualY] == this.gameMode[this.actualY].Length - 1 ? 0 : this.actualX[this.actualY] + 1]);
        }
        Destroy(this.gameMode[actualY][actualX[this.actualY]]);
        this.actualX[this.actualY] = this.actualX[this.actualY] == 0 ? this.gameMode[this.actualY].Length - 1 : this.actualX[this.actualY] -= 1;
         
        this.setCard();
    }

    public void updateY(int f_y)
    {
        
        this.destroyCard();
        this.actualY = f_y;
        this.setCard();
        this.showArrow();
    }

    public void showArrow()
    {
        if (this.gameMode[this.actualY].Length == 1)
        {
            this._arrowLeft.SetActive(false);
            this._arrowRight.SetActive(false);
        }
        else
        {
            this._arrowLeft.SetActive(true);
            this._arrowRight.SetActive(true);
        }
    }

    private void setCard()
    {
        Quaternion angle = Quaternion.Euler(new Vector3(270,0,0));
        if (this.gameMode[actualY].Length == 2)
        {
            if (actualX[this.actualY] == 0)
            {
                this.gameMode[actualY][1] = myInstantiate(new Vector3(7, actualY * -7, 0), angle, "Arene " + 2) as GameObject;
                this.substitu = myInstantiate(new Vector3(-7, actualY * -7, 0), angle, "Arene - sub") as GameObject;
            }
            else
            {
                this.gameMode[actualY][0] = myInstantiate(new Vector3(7, actualY * -7, 0), angle, "Arene " + 1) as GameObject;
                this.substitu = myInstantiate(new Vector3(-7, actualY * -7, 0), angle, "Arene - sub") as GameObject;
            }
        }
        if (this.gameMode[actualY].Length > 2)
        {
            if (actualX[this.actualY] != 0)
            {
                this.gameMode[actualY][actualX[this.actualY] - 1] = myInstantiate(new Vector3(-7, actualY * -7, 0), angle, "Arene " + (actualX[this.actualY])) as GameObject;
                this.gameMode[actualY][actualX[this.actualY] - 1].GetComponent<MeshRenderer>().material = _mat[actualX[this.actualY]-1];
            }
            else
            {
                this.gameMode[actualY][this.gameMode[actualY].Length - 1] = myInstantiate(new Vector3(-7, actualY * -7, 0), angle, "Arene " + (this.gameMode[actualY].Length)) as GameObject;
                this.gameMode[actualY][this.gameMode[actualY].Length - 1].GetComponent<MeshRenderer>().material = _mat[this.gameMode[actualY].Length-1];
            }
            if (actualX[this.actualY] != this.gameMode[actualY].Length - 1)
            {
                this.gameMode[actualY][actualX[this.actualY] + 1] = myInstantiate(new Vector3(7, actualY * -7, 0), angle, "Arene " + (actualX[this.actualY] + 2)) as GameObject;
                this.gameMode[actualY][actualX[this.actualY] + 1].GetComponent<MeshRenderer>().material = _mat[actualX[this.actualY] + 1];
            }
            else
            {
                this.gameMode[actualY][0] = myInstantiate(new Vector3(7, actualY * -7, 0), angle, "Arene " + 1) as GameObject;
                this.gameMode[actualY][0].GetComponent<MeshRenderer>().material = _mat[0];
            }
        }
        StaticBoard.rule = DefRules.rules[actualX[actualY]];
        StaticBoard.ruleID = actualX[actualY];
    }

    private void destroyCard()
    {

        if (this.gameMode[actualY].Length == 2)
        {
            if (actualX[this.actualY] == 0)
            {
                Destroy(this.gameMode[actualY][1]);
            }
            else
                Destroy(this.gameMode[actualY][0]);
            Destroy(this.substitu);
        }
        if (this.gameMode[actualY].Length > 2)
        {
            if (actualX[this.actualY] != 0)
                Destroy(this.gameMode[actualY][actualX[this.actualY] - 1]);
            else
                Destroy(this.gameMode[actualY][this.gameMode[actualY].Length - 1]); 

            if (actualX[this.actualY] != this.gameMode[actualY].Length - 1)
                Destroy(this.gameMode[actualY][actualX[this.actualY] + 1]);
            else
                Destroy(this.gameMode[actualY][0]);
        }
    }


    public GameObject getFleet()
    {
        return this.gameMode[this.actualY][this.actualX[this.actualY]];
    }
    public GameObject getFleetNext()
    {
        if (this.actualX[this.actualY] + 1 >= this.gameMode[this.actualY].Length)
        {
            return this.gameMode[this.actualY][0];
        }
        return this.gameMode[this.actualY][this.actualX[this.actualY]+1];
    }
    public GameObject getFleetPrev()
    {
        if (this.gameMode[this.actualY].Length == 2)
        {
            Vector3 inter = this.substitu.transform.position;
            this.substitu.transform.position = this.gameMode[actualY][actualX[this.actualY] == 0 ? 1 : 0].transform.position;
            this.gameMode[actualY][actualX[this.actualY] == 0 ? 1 : 0].transform.position = inter;
        }
        if (this.actualX[this.actualY] != 0)
            return this.gameMode[this.actualY][this.actualX[this.actualY] - 1];
        return this.gameMode[this.actualY][this.gameMode[this.actualY].Length-1];
    }

    public GameObject[] getFleetOnY()
    {
        GameObject[] inter = new GameObject[3];
        if (this.gameMode[actualY].Length == 2)
        {
            if (actualX[this.actualY] == 0)
            {
                inter[2] = this.gameMode[this.actualY][1];
                inter[1] = this.gameMode[this.actualY][0];
                inter[0] = substitu;
            }
            else
            {
                inter[2] = this.gameMode[this.actualY][0];
                inter[1] = this.gameMode[this.actualY][1];
                inter[0] = substitu;
            }

            

        }
        else
        {
            inter[0] = this.gameMode[this.actualY][this.actualX[this.actualY]>0?this.actualX[this.actualY] - 1 : this.gameMode[this.actualY].Length-1];
            inter[1] = this.gameMode[this.actualY][this.actualX[this.actualY]];
            inter[2] = this.gameMode[this.actualY][this.actualX[this.actualY] == this.gameMode[this.actualY].Length - 1 ? 0 :this.actualX[this.actualY] + 1];
        }
        return inter;
    }

}
