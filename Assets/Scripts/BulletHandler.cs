using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviour {

    public float Speed = 10;
    public float TimeToLive = 5f;
    public float Dmg = 10f;

    void Start ()
	{
        Destroy(gameObject, TimeToLive);
	}

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "player" || other.tag == "ai")
        {
            other.SendMessage("Hit", Dmg);
        }
             
        Destroy(gameObject);
    }

    void Update()
    {
        transform.position += transform.forward * Speed * Time.deltaTime; 
    }
}
