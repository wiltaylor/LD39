using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class MainMenuHandler : MonoBehaviour
    {
        void Start()
        {
            GameManager.Instance.DisableTargetingView();
        }

        void Update()
        {
            if (Input.GetButton("Jump"))
            {
                GameManager.Instance.NextLevel();
            }
        }

    }
}
