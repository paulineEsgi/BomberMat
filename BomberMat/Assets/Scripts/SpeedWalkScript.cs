using UnityEngine;
using System.Collections;

public class SpeedWalkScript : MonoBehaviour {

	private Vector3 direction;
	private Vector3 playerPos;
	private GameObject player;
	private Vector3 swPos;
	private bool moove =false;
	void Start () {

	}
	
	
	void OnTriggerEnter(Collider collider)
	{
		swPos = this.transform.position;
		player =collider.gameObject;
		playerPos = player.transform.position;
		if(this.transform.rotation==Quaternion.Euler(0,0,0))
		{
			//direction =new Vector3(swPos.x-1, playerPos.y, swPos.z);
			direction = Vector3.left;
		}
		else if(this.transform.rotation ==Quaternion.Euler(0,90,0))
		{
			//direction =new Vector3(swPos.x, playerPos.y, swPos.z+1f);
			direction = new Vector3(0,0,1f);
		}
		else if(this.transform.rotation==Quaternion.Euler(0,180,0))
		{
			//direction = new Vector3(swPos.x+1, playerPos.y, swPos.z);
			direction = Vector3.right;
		}
		else if(this.transform.rotation==Quaternion.Euler(0,270,0))
		{
			//direction = new Vector3(swPos.x, playerPos.y, swPos.z-1);
			direction = new Vector3(0,0,-1f);
		}

		if(collider.gameObject.tag != "terrain")
		{
			
			//player.transform.position = swPos;
			Debug.Log(direction);
			//collider.gameObject.transform.position= new Vector3(direction.x, playerPos.y, direction.z);
			//collider.gameObject.transform.rigidbody.AddForce(new Vector3(direction.x, playerPos.y, direction.z));
			//playerPos = swPos;
			//StartCoroutine(Moove());
			//collider.gameObject.transform.Translate( direction);
		}
		
	}
	
	
	 void OnTriggerStay(Collider other) 
	{
		player.transform.rigidbody.AddForce(direction* Time.deltaTime *350);
	}
	
	void Update () {
		
	}
}
