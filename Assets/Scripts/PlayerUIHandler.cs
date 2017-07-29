using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHandler : MonoBehaviour
{
    public Slider Power;

    public Slider Jump;

    public Slider Ammo;


    void Update()
    {
        if (PlayerManager.Instance != null)
        {
            Power.maxValue = PlayerManager.Instance.MaxPower;
            Power.minValue = 0;
            Power.value = PlayerManager.Instance.PowerLeft;

            Jump.maxValue = PlayerManager.Instance.MaxJump;
            Jump.minValue = 0;
            Jump.value = PlayerManager.Instance.JumpLeft;

            Ammo.maxValue = PlayerManager.Instance.MaxAmmo;
            Ammo.minValue = 0;
            Ammo.value = PlayerManager.Instance.AmmoLeft;
        }
    }
}
