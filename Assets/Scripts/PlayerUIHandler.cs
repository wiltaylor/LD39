using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHandler : MonoBehaviour
{
    public Slider Power;
    public Slider Jump;
    public Slider Ammo;
    public Image Topbar;
    public GameObject WinText;
    public GameObject LoseText;


    void Update()
    {
        if (LevelManager.Instance.CurrentMode != LevelManager.PlayMode.Playing)
        {
            Topbar.enabled = false;
            Power.gameObject.SetActive(false);
            Jump.gameObject.SetActive(false);
            Ammo.gameObject.SetActive(false);
        }
        else
        {
            Topbar.enabled = true;
            Power.gameObject.SetActive(true);
            Jump.gameObject.SetActive(true);
            Ammo.gameObject.SetActive(true);
        }

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

        if (LevelManager.Instance.CurrentMode == LevelManager.PlayMode.Winner)
        {
            WinText.SetActive(true);

            if(Input.GetButton("Jump"))
                GameManager.Instance.NextLevel();
        }
        else
        {
            WinText.SetActive(false);
        }

        if (LevelManager.Instance.CurrentMode == LevelManager.PlayMode.Lose)
        {
            LoseText.SetActive(true);

            if (Input.GetButton("Jump"))
                GameManager.Instance.ReloadLevel();
        }
        else
        {
            LoseText.SetActive(false);
        }
    }
}
