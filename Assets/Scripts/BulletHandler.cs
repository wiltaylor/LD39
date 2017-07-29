using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviour {

    public float Speed = 10;
    public float TimeToLive = 5f;
    public float Dmg = 10f;
    public GameObject DeathAnim;

    private bool hit;

    private MeshRenderer _render;

    void Start ()
	{
        Destroy(gameObject, TimeToLive);
	    _render = GetComponent<MeshRenderer>();

	}

    void OnTriggerEnter(Collider other)
    {
        hit = true;

        if (other.tag == "player" || other.tag == "ai")
        {
            other.SendMessage("Hit", Dmg);
        }
        
        
        Destroy(gameObject, 0.5f);

        if(DeathAnim != null)
            DeathAnim.SetActive(true);

        if (_render != null)
            _render.enabled = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        hit = true;

        if (collision.transform.tag == "player" || collision.transform.tag == "ai")
        {
            collision.transform.SendMessage("Hit", Dmg);
        }

        Destroy(gameObject, 0.5f);
        DeathAnim.SetActive(true);
        _render.enabled = false;
    }

    void Update()
    {
        if(!hit)
        transform.position += transform.forward * Speed * Time.deltaTime; 
    }
}
