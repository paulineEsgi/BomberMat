/****
 * Classe de règles de jeu qui liste les règles changeant d'un mode à l'autre 
 */
public class GameRules {

    private float playerDrag; //Frottement du joueur avec le sol

    private bool isHoles; //Si pique ou non
	private bool isSpeedWalk; //Si accelerateur ou non

	// Use this for initialization
    public GameRules(float playerDrag, bool isHoles, bool isSpeedWalk)
    {
        this.playerDrag = playerDrag;
        this.isHoles = isHoles;
		this.isSpeedWalk = isSpeedWalk;
    }


    //Uniquement des assesseurs et des mutateurs

    public void setPlayerDrag (float value)
    {
        playerDrag = value;
    }

    public float getPlayerDrag ()
    {
        return playerDrag;
    }

    public void setIsHoles(bool value)
    {
        isHoles = value;
    }

    public bool getIsHoles()
    {
        return isHoles;
    }
	
	public void setIsSpeedWalk(bool value)
    {
        isSpeedWalk = value;
    }

    public bool getIsSpeedWalk()
    {
        return isSpeedWalk;
    }
}
