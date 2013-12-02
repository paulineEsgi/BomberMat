/****
 * Script qui gère les animations de l'avatar
 */

using UnityEngine;
using System.Collections;

public class AnimationScript : MonoBehaviour {

	public Animation anim;
	public GameObject armature;
	private Transform pos;
	void Start () {
		
	}
	

	void Update () {
	
		 if(!(Input.GetKey(KeyCode.UpArrow) ||
			Input.GetKey(KeyCode.DownArrow) ||
			Input.GetKey(KeyCode.RightArrow) ||
			Input.GetKey(KeyCode.LeftArrow)))
		{
			anim.Stop();
		}
		else if (Input.GetKey(KeyCode.UpArrow) ||
			Input.GetKeyDown(KeyCode.DownArrow) ||
			Input.GetKeyDown(KeyCode.RightArrow) ||
			Input.GetKeyDown(KeyCode.LeftArrow))
		{
			anim.Play("Walk");
		}
		if (Input.GetKeyDown(KeyCode.Space))
        {
            animation.Play("Jump");
        }
	}
}
