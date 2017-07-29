using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;

        public GameObject TankPrefab;
        public GameObject ObserverPrefab;
        private List<GameObject> _tanks = new List<GameObject>();
        private GameObject _observer;

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
            if(Camera.main == null)
                _observer.SetActive(true);
        }

        void Awake()
        {
            NavPoints = gameObject.GetComponentsInChildren<NavPointInfo>();
            SpawnPoints = gameObject.GetComponentsInChildren<SpawnPointInfo>();

            bool setPlayerTank = !Observe;

            _observer = Instantiate(ObserverPrefab);
            _observer.transform.position = NavPoints[0].transform.position;
            _observer.SetActive(Observe);

            foreach (var point in SpawnPoints)
            {
                var tank = Instantiate(TankPrefab);
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

                tank.transform.position = point.transform.position;
                tank.SetActive(true);

                _tanks.Add(tank);
            }
        }       
    }
}
