using System;
using System.Collections.Generic;
using System.Linq;
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

            _controller.EngineSound.volume = 0.5f;
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

        public override void HitBy(GameObject attacker)
        {
            if (_currentState == AIState.Search || _currentState == AIState.GetPowerUp)
            {
                _AttackTarget = attacker;
                _currentState = AIState.LookingFor;
                _LookingForTimeOut = LookForTime;
            }
        }

        private void GetPowerUpUpdate()
        {
            AttackEnemyIfSeen();

            if (_currentState != AIState.GetPowerUp)
                return;

            if (_AttackTarget == null || _agent.remainingDistance < NabMeshPointTollerance || _AttackTarget.activeInHierarchy == false)
            {
                _currentState = AIState.Search;
                MoveToRandomNode();
            }
        }

        private void LookingForUpdate()
        {
            //AttackEnemyIfSeen();

            if (_currentState != AIState.LookingFor)
                return;

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

        private Collider[] GetAllCanSee()
        {
            return Physics.OverlapSphere(_controller.Spawner.transform.position, SeeDistance, _searchLayer);
        }

        private GameObject FirstEnemySeen(IEnumerable<Collider> colliders)
        {
            var result = colliders.FirstOrDefault(c => c.gameObject != _gameObject &&
                                                 c.gameObject.CompareTag("player"));

            return result != null ? result.gameObject : null;
        }

        private GameObject FirstPickupSeen(IEnumerable<Collider> colliders)
        {
            var result = colliders.FirstOrDefault(c => c.gameObject != _gameObject &&
                                                       c.gameObject.CompareTag("pickup"));

            return result != null ? result.gameObject : null;
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

            //Checking if out of ammo
            if (_controller.BulletsLeft < 1)
            {
                _AttackTarget = null;
                _currentState = AIState.Search;
                MoveToRandomNode();
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

        private IEnumerable<Collider> AttackEnemyIfSeen()
        {
            var objects = GetAllCanSee();
            var tank = FirstEnemySeen(objects);

            if (tank == null || _controller.BulletsLeft <= 0) return objects;

            _AttackTarget = tank;
            _currentState = AIState.Attack;
            _agent.isStopped = true;

            return objects;
        }

        private void SearchUpdate()
        {

            var objs = AttackEnemyIfSeen();

            if (_currentState != AIState.Search)
                return;

            var pickup = FirstPickupSeen(objs);

            if (pickup != null)
            {
                _AttackTarget = pickup;
                _currentState = AIState.GetPowerUp;
                _agent.isStopped = false;
                _agent.destination = pickup.transform.position;
                return;
            }

            if (_agent.remainingDistance < NabMeshPointTollerance)
            {
                MoveToRandomNode();
            }
        }
    }
}
