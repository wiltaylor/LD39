using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts
{
    public class AIBrain : TankBrain
    {
        public float NabMeshPointTollerance = 0.05f;
        public float SeeDistance = 10f;
        public float LookForTime = 1f;

        public enum AIState
        {
            Search,
            Attack,
            LookingFor,
            GetPowerUp
        }

        private AIState _currentState = AIState.Search;
        private NavPointInfo[] _navpoints;
        private NavMeshAgent _agent;
        private Transform _transform;
        private GameObject _gameObject;
        private int _searchLayer = 1 << 8;
        private GameObject _AttackTarget;
        private TankController _controller;
        private float _LookingForTimeOut = 0;

        public override void OnCollisionEnter(Collision collision)
        {
            
        }

        public override void OnStart(GameObject obj)
        {
            _agent = obj.GetComponent<NavMeshAgent>();
            _transform = obj.transform;
            _gameObject = obj;

            _navpoints = LevelManager.Instance.NavPoints;

            MoveToRandomNode();

            _currentState = AIState.Search;
            _controller = obj.GetComponent<TankController>();
        }

        public override void Update()
        {
            
        }

        public override void FixedUpdate()
        {
            _controller.State = _currentState;
            switch (_currentState)
            {
                case AIState.Search:
                    SearchUpdate();
                    break;
                case AIState.Attack:
                    AttackUpdate();
                    break;
                case AIState.LookingFor:
                    LookingForUpdate();
                    break;
                case AIState.GetPowerUp:
                    GetPowerUpUpdate();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void GetPowerUpUpdate()
        {
            _controller.StateText = "GetPowerup";
            if (_AttackTarget == null || _agent.remainingDistance < NabMeshPointTollerance || _AttackTarget.activeInHierarchy == false)
            {
                _currentState = AIState.Search;
                MoveToRandomNode();
            }
        }

        private void LookingForUpdate()
        {
            _controller.StateText = "LookingFor";
            
            _LookingForTimeOut -= Time.deltaTime;

            if (_AttackTarget == null || _LookingForTimeOut < 0f || _AttackTarget.activeInHierarchy == false)
            {
                _currentState = AIState.Search;
                return;
            }

            _agent.isStopped = false;
            _agent.destination = _AttackTarget.transform.position;

            if (CanSeeTarget())
            {
                _agent.isStopped = true;
                _currentState = AIState.Attack;
            }
        }

        private bool CanSeeTarget()
        {
            RaycastHit hit;
            var direction = _AttackTarget.transform.position - _transform.position;

            if (Physics.Raycast(_controller.Spawner.transform.position, direction, out hit, SeeDistance))
            {
                if (hit.transform.gameObject == _AttackTarget)
                {
                    return false;
                }
            }

            return true;
        }

        private void AttackUpdate()
        {
            _controller.StateText = "Attack";
            if (_AttackTarget == null ||_AttackTarget.activeInHierarchy == false)
            {
                _currentState = AIState.Search;
                MoveToRandomNode();
                return;
            }

            //Checking we can still see target
            if (!CanSeeTarget())
            {
                _currentState = AIState.LookingFor;
                _LookingForTimeOut = LookForTime;
                return;
            }

            _agent.isStopped = true;
            _controller.AimCannon(_AttackTarget.transform.position);

            if(_controller.CanShoot())
                _controller.ShootCannon();
        }

        private void MoveToRandomNode()
        {
            var index = UnityEngine.Random.Range(0, _navpoints.Length);

            _agent.ResetPath();
            _agent.isStopped = false;
            if (!_agent.SetDestination(_navpoints[index].transform.position))
            {
                Debug.Log("Failed to move to positon!");
            }
        }

        private void SearchUpdate()
        {
            var hits = Physics.OverlapSphere(_controller.Spawner.transform.position, SeeDistance, _searchLayer);

            foreach (var h in hits)
            {
                if (h.gameObject == _gameObject)
                    continue;

                _AttackTarget = h.gameObject;

                if (h.CompareTag("player"))
                {
                    _currentState = AIState.Attack;
                    _agent.isStopped = true;
                    return;
                }
                if (h.CompareTag("pickup"))
                {
                    _currentState = AIState.GetPowerUp;
                    _agent.destination = _AttackTarget.transform.position;
                    _agent.isStopped = false;
                    return;
                }
            }

            if (_agent.remainingDistance < NabMeshPointTollerance)
            {
                MoveToRandomNode();
            }
        }
    }
}
