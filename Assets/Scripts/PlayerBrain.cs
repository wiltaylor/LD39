using System;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts
{
    public class PlayerBrain : TankBrain
    {

        private Rigidbody _rigidbody;
        private NavMeshAgent _agent;
        private TankController _controller;
        private GameObject _gameObject;
        private float _jumpRemaining;
        private HPHandler _hpHandler;

        private bool _jumplock;

        public override void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag("levelgeom"))
            {
                _jumplock = false;
                _agent.enabled = true;
                _rigidbody.isKinematic = true;
            }
        }

        public override void OnStart(GameObject obj)
        {
            _rigidbody = obj.GetComponent<Rigidbody>();
            _jumplock = false;
            _agent = obj.GetComponent<NavMeshAgent>();
            _controller = obj.GetComponent<TankController>();
            _hpHandler = obj.GetComponent<HPHandler>();
            _jumpRemaining = _controller.JumpPower;
            _gameObject = obj;

            _controller.StateText = "Player";

            _agent.updateRotation = false;

        }

        public override void Update()
        {
            if (!_jumplock)
            {
                _jumpRemaining += _controller.JumpRechargeRate * Time.deltaTime;

                if (_jumpRemaining > _controller.JumpPower)
                    _jumpRemaining = _controller.JumpPower;
            }

            if (PlayerManager.Instance != null)
            {
                PlayerManager.Instance.MaxJump = _controller.JumpPower;
                PlayerManager.Instance.JumpLeft = _jumpRemaining;
                PlayerManager.Instance.MaxPower = _hpHandler.MaxPower;
                PlayerManager.Instance.PowerLeft = _hpHandler.Power;
                PlayerManager.Instance.MaxAmmo = _controller.MaxBullets;
                PlayerManager.Instance.AmmoLeft = _controller.BulletsLeft;
            }

            if (Input.GetAxis("Vertical") > 0.01f)
            {
                if (!_jumplock)
                    _controller.Speed += _controller.Acceleration;

                if (_controller.Speed > _controller.Maxspeed)
                    _controller.Speed = _controller.Maxspeed;

                _agent.velocity = _gameObject.transform.forward * _controller.Speed * Time.deltaTime;
            }
            else
            {
                _controller.Speed = 0f;
            }
            
            if (Input.GetAxis("Vertical") < -0.01f & !_jumplock)
            {
                _controller.Break += _controller.BreakForce;

                if (_controller.Break > _controller.MaxBreakSpeed)
                    _controller.Break = _controller.MaxBreakSpeed;

                _agent.velocity = -_gameObject.transform.forward * _controller.Break * Time.deltaTime;

            }
            else
            {
                _controller.Break = 0f;
            }

            if (Input.GetAxis("Horizontal") < -0.01f)
            {
                _gameObject.transform.Rotate(0, -_controller.RotationSpeed * Time.deltaTime, 0);
            }

            if (Input.GetAxis("Horizontal") > 0.01f)
            {
                _gameObject.transform.Rotate(0, _controller.RotationSpeed * Time.deltaTime, 0);
            }

            if (Input.GetButton("Jump"))
            {
                _agent.enabled = false;
                _rigidbody.isKinematic = false;
                _rigidbody.AddForce(Vector3.up * _controller.JumpSpeed * Time.deltaTime, ForceMode.Force);
                _rigidbody.AddForce(_gameObject.transform.forward * _controller.Speed * Time.deltaTime);

                _jumpRemaining -= _controller.JumpUseRate * Time.deltaTime;
                _jumplock = true;

            }

            HandleTurret();
        }

        private void HandleTurret()
        {
            var playerPlane = new Plane(Vector3.up, _controller.TurretTransform.position);
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            var hitdist = 0f;

            if (playerPlane.Raycast(ray, out hitdist))
                _controller.AimCannon(ray.GetPoint(hitdist));

            if (Input.GetButton("Fire1") && _controller.CanShoot())
                _controller.ShootCannon();

        }

        public override void FixedUpdate()
        {
        }
    }
}
