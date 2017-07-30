using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class TankController : MonoBehaviour
    {
        private TankBrain _brain;
        public float Maxspeed;
        public float MaxBreakSpeed;
        public float Acceleration;
        public float BreakForce;
        public float Speed;
        public float Break;
        public float RotationSpeed = 1f;
        public float JumpSpeed = 100f;
        public Transform TurretTransform;
        public ProjectileSpawner Spawner;
        public float ShootCoolDown;
        public float CurrentShootCoolDown;
        public float JumpPower = 10f;
        public float JumpUseRate = 2f;
        public float JumpRechargeRate = 1f;
        public int MaxBullets = 10;
        public int BulletsLeft = 10;
        public string StateText;
        public AIBrain.AIState State;
        public BrainTypeEnum BrainType = BrainTypeEnum.AI;
        public GameObject CameraObject;
        public AudioSource EngineSound;

        public float IdleSound = 0.75f;
        public float RevSound = 1.5f;

        public AudioSource ShootSound;

        private HPHandler _hpHandler;
        private float _enginePitch;
        private bool _revUp;

        public void SetRevUp(bool revup)
        {
            _revUp = revup;
        }

        public TankController(TankBrain brain)
        {
            _brain = brain;
        }

        public enum BrainTypeEnum
        {
            AI,
            Human
        }

        void OnCollisionEnter(Collision collision)
        {
            _brain.OnCollisionEnter(collision);
        }

        public void MakeAI()
        {
            CameraObject.SetActive(false);
            _brain = new AIBrain();
            _brain.OnStart(gameObject);
            BrainType = BrainTypeEnum.AI;
        }

        public void MakeHuman()
        {
            CameraObject.SetActive(true);
            _brain = new PlayerBrain();
            _brain.OnStart(gameObject);
            BrainType = BrainTypeEnum.Human;
        }

        void Start()
        {
            _hpHandler = GetComponent<HPHandler>();
        }

        void Update()
        {
            _enginePitch = Mathf.Lerp(_enginePitch, _revUp ? RevSound : IdleSound, Time.deltaTime);
            EngineSound.pitch = _enginePitch;

            if (CurrentShootCoolDown > 0f)
                CurrentShootCoolDown -= Time.deltaTime;

            if(_brain != null)
                _brain.Update();

        }

        void FixedUpdate()
        {


            if (_brain != null)
                _brain.FixedUpdate();
        }

        public bool CanShoot()
        {
            return CurrentShootCoolDown <= 0f && BulletsLeft > 0;
        }

        public void ShootCannon()
        {
            if (!CanShoot())
            {
                return;
            }

            ShootSound.Play();
            Spawner.Shoot();
            CurrentShootCoolDown = ShootCoolDown;
            BulletsLeft--;
        }

        public void AimCannon(Vector3 point)
        {
            var targetRotation = Quaternion.LookRotation(point - TurretTransform.position);
            TurretTransform.rotation = targetRotation;

        }

        public void ResetCannonAim()
        {
            TurretTransform.rotation = Quaternion.identity;
        }

        public void GetBullets(int bullets)
        {
            BulletsLeft += bullets;

            if (BulletsLeft > MaxBullets)
                BulletsLeft = MaxBullets;
        }

        public void GetPower(float power)
        {
            _hpHandler.Power = power;

            if (_hpHandler.Power > _hpHandler.MaxPower)
                _hpHandler.Power = _hpHandler.MaxPower;
        }

        public void HitBy(GameObject attacker)
        {
            _brain.HitBy(attacker);
        }
    }
}
