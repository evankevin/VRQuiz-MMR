using UnityEngine;
using System.Collections;

public class LookAtGiven : MonoBehaviour {

	// Use this for initialization
	public Transform lookObjective;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(lookObjective!=null)
		{
            Vector3 dir = (lookObjective.position - transform.position);
            Vector3 objective = transform.position - dir; 

            transform.LookAt(objective);
            
		}
        else
        {

            lookObjective = Camera.main.transform;

            Vector3 dir = (lookObjective.position - transform.position);
            Vector3 objective = transform.position - dir;

            transform.LookAt(objective);
        }
	
	}
}
