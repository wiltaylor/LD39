using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPHandler : MonoBehaviour
{
    public float MaxPower = 100f;
    public float Power = 100f;
    public float ShowParticleSystemAt = 10f;
    public float DecayRate = 0.01f;
    public GameObject ParticleSystem;

    public void Hit(float dmg)
    {
        Power -= dmg;
    }

    private void Update()
    {
        if (Power <= 0f)
        {
            Destroy(gameObject);
        }

        Power -= DecayRate * Time.deltaTime;

        if(Power < ShowParticleSystemAt)
            ParticleSystem.SetActive(true);
        else
            ParticleSystem.SetActive(false);
    }

}
