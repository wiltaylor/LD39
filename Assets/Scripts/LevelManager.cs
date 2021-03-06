﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class LevelManager : MonoBehaviour
    {
        public enum PlayMode
        {
            Playing,
            Lose,
            Winner
        }


        public static LevelManager Instance;

        public GameObject BulletPrefab;
        public int NumberOfBulletsInPool = 200;
        public PlayMode CurrentMode = PlayMode.Playing;
        public GameObject TankPrefab;
        public GameObject ObserverPrefab;
        public int TanksLeft;

        [HideInInspector]
        public ObjectPool BulletPool;
        private readonly List<GameObject> _tanks = new List<GameObject>();

        public bool Observe;
        
        public LevelManager()
        {
            Instance = this;
        }

        void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

        [HideInInspector]
        public NavPointInfo[] NavPoints;

        public SpawnPointInfo[] SpawnPoints;

        void Update()
        {
            if (_tanks.All(t => !t.activeInHierarchy))
            {
                GameManager.Instance.ReloadLevel();
                return;
                
            }

            if (Camera.main == null)
            {
                _tanks.First(t => t.activeInHierarchy).GetComponent<TankController>().CameraObject.SetActive(true);
                GameManager.Instance.DisableTargetingView();
            }

            TanksLeft = _tanks.Count(t => t.activeInHierarchy);

            if (_tanks.Count(t => t.activeInHierarchy && t.GetComponent<TankController>().BrainType == TankController.BrainTypeEnum.Human) < 1)
                CurrentMode = PlayMode.Lose;

            if (TanksLeft <= 1)
            {
                var winningtank = _tanks.FirstOrDefault(t => t.activeInHierarchy);

                if (winningtank == null)
                {
                    GameManager.Instance.ReloadLevel();
                }

                if(winningtank.GetComponent<TankController>().BrainType == TankController.BrainTypeEnum.Human)
                    CurrentMode = PlayMode.Winner;
            }
                
        }

        void Awake()
        {
            NavPoints = gameObject.GetComponentsInChildren<NavPointInfo>();
            SpawnPoints = gameObject.GetComponentsInChildren<SpawnPointInfo>();

            BulletPool = new ObjectPool(NumberOfBulletsInPool, BulletPrefab); 

            bool setPlayerTank = !Observe;

            foreach (var point in SpawnPoints)
            {
                var tank = Instantiate(TankPrefab, point.transform.position, Quaternion.identity);
                var controller = tank.GetComponent<TankController>();

                if (setPlayerTank)
                {
                    setPlayerTank = false;
                    controller.MakeHuman();
                }
                else
                {
                    controller.MakeAI();
                }

                tank.SetActive(true);

                _tanks.Add(tank);
            }
        }       
    }
}
