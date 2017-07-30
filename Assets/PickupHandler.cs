using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHandler : MonoBehaviour
{
    public int Bullets;
    public float Power;
    public float RotateSpeed = 1f;
    public float CoolDown = 10f;

    private MeshRenderer _mesh;
    private BoxCollider _colider;
    private float _currentCooldown = 0f;
    public AudioSource PickupSound;

    void Start()
    {
        _mesh = GetComponent<MeshRenderer>();
        _colider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (_currentCooldown > 0f)
        {
            _currentCooldown -= Time.deltaTime;
            return;
        }

        _colider.enabled = true;
        _mesh.enabled = true;


        transform.Rotate(0, RotateSpeed * Time.deltaTime, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {

            if (Bullets > 0)
                other.SendMessage("GetBullets", Bullets);
            if (Power > 0)
                other.SendMessage("GetPower", Power);
        }

        _mesh.enabled = false;
        _colider.enabled = false;
        _currentCooldown = CoolDown;
        PickupSound.Play();
    }
}
