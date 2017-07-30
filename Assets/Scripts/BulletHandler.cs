using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviour {

    public float Speed = 10;
    public float TimeToLive = 5f;
    public float Dmg = 10f;
    public GameObject DeathAnim;
    public GameObject Owner;

    private bool hit;

    private MeshRenderer _render;
    private float _timeleft;

    void OnEnable()
    {
        hit = false;
        _timeleft = TimeToLive;
       _render = GetComponent<MeshRenderer>();
        _render.enabled = true;
        DeathAnim.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        hit = true;

        //Stop you shooting yourself.
        if (other.gameObject == Owner)
            return;

        if (other.tag == "player" || other.tag == "ai")
        {
            other.SendMessage("Hit", Dmg);
            other.SendMessage("HitBy", Owner);
        }


        _timeleft = 0.5f;

        if (DeathAnim != null)
            DeathAnim.SetActive(true);

        if (_render != null)
            _render.enabled = false;
    }

    void Update()
    {
        _timeleft -= Time.deltaTime;

        if(_timeleft < 0f)
            gameObject.SetActive(false);

        if(!hit)
            transform.position += transform.forward * Speed * Time.deltaTime; 
    }
}
