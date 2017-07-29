using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts
{
    public class AINavTest : MonoBehaviour
    {

        public GameObject[] Goal;

        public float SeeDistance = 10f;

        private int _index;
        private NavMeshAgent _agent;
        private int _tankLayer = 1 << 8;

        void Start()
        {
            _agent = GetComponent<NavMeshAgent>();

            _agent.destination = Goal[_index].transform.position;
        }

        void FixedUpdate()
        {

            if (_agent.remainingDistance < 0.05f)
            {
                _index++;

                if (_index >= Goal.Length)
                    _index = 0;

                _agent.destination = Goal[_index].transform.position;
            }

            //Look for tanks near by


            var hits = Physics.OverlapSphere(transform.position, SeeDistance, _tankLayer);

            foreach (var h in hits)
            {
                if (h.gameObject == gameObject)
                    continue;

                Debug.Log("AI can see: " + h.transform.name);
            }
        }

    }
}
