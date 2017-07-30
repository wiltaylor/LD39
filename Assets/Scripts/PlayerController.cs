using System.Runtime.InteropServices;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
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
    public float JumpPower = 10f;
    public float JumpUseRate = 2f;
    public float JumpRechargeRate = 1f;
    public int MaxBullets = 10;

    private Rigidbody _rigidbody;
    private float _shootCooldown;
    private float _jumpRemaining;
    private bool _jumplock;
    private HPHandler _hpHandler;
    private NavMeshAgent _agent;
    private int _bulletsLeft;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "levelgeom")
        {
            _jumplock = false;
            _agent.enabled = true;
            _rigidbody.isKinematic = true;
        }
    }


    void Start ()
	{
	    _rigidbody = GetComponent<Rigidbody>();
	    _jumpRemaining = JumpPower;
	    _jumplock = false;
	    _hpHandler = GetComponent<HPHandler>();
	    _agent = GetComponent<NavMeshAgent>();
	    _bulletsLeft = MaxBullets;
	}

    void Update ()
    {
        if (LevelManager.Instance.CurrentMode != LevelManager.PlayMode.Playing)
            return;
        
        if (!_jumplock)
        {
            _jumpRemaining += JumpRechargeRate * Time.deltaTime;
            if (_jumpRemaining > JumpPower)
                _jumpRemaining = JumpPower;
        }

        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.MaxJump = JumpPower;
            PlayerManager.Instance.JumpLeft = _jumpRemaining;
            PlayerManager.Instance.MaxPower = _hpHandler.MaxPower;
            PlayerManager.Instance.PowerLeft = _hpHandler.Power;
            PlayerManager.Instance.MaxAmmo = MaxBullets;
            PlayerManager.Instance.AmmoLeft = _bulletsLeft;
        }

        if (Input.GetAxis("Vertical") > 0.01f)
        {
            if (!_jumplock)
                Speed += Acceleration;

            if (Speed > Maxspeed)
                Speed = Maxspeed;


            //_rigidbody.AddForce(transform.forward * Speed * Time.deltaTime);
            _agent.velocity = transform.forward * Speed * Time.deltaTime;
        }
        else
        {
            Speed = 0f;
        }

        if (Input.GetAxis("Vertical") < -0.01f & !_jumplock)
        {
            Break += BreakForce;

            if (Break > MaxBreakSpeed)
                Break = MaxBreakSpeed;

            // _rigidbody.AddForce(-transform.forward * Break * Time.deltaTime);
            _agent.velocity = -transform.forward * Break * Time.deltaTime;
        }
        else
        {
            Break = 0f;
        }

        if (Input.GetAxis("Horizontal") < -0.01f & !_jumplock)
        {
            transform.Rotate(0, -RotationSpeed * Time.deltaTime, 0 );
        }

        if (Input.GetAxis("Horizontal") > 0.01f)
        {
            transform.Rotate(0, RotationSpeed * Time.deltaTime, 0);
        }

        var playerPlane = new Plane(Vector3.up, TurretTransform.position);
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        var hitdist = 0f;

        if (playerPlane.Raycast(ray, out hitdist))
        {
            var targetPoint =  ray.GetPoint(hitdist);

            var targetRotation = Quaternion.LookRotation(targetPoint - TurretTransform.position);
            TurretTransform.rotation = targetRotation;
        }

        if (_shootCooldown > 0f)
        {
            _shootCooldown -= Time.deltaTime;
        }

        if (Input.GetButton("Fire1") && _shootCooldown <= 0f && _bulletsLeft > 0)
        {
            Spawner.Shoot();
            _shootCooldown = ShootCoolDown;
            _bulletsLeft--;
        }

        if (Input.GetButton("Jump")) //&& _jumpRemaining > 0f)
        {
            _agent.enabled = false;
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(Vector3.up * JumpSpeed * Time.deltaTime, ForceMode.Force);
            _rigidbody.AddForce(transform.forward * Speed * Time.deltaTime);
            //_rigidbody.AddForce(_agent.velocity);
            //_agent.velocity = Vector3.up * JumpSpeed * Time.deltaTime;
            _jumpRemaining -= JumpUseRate * Time.deltaTime;
            _jumplock = true;
        }

    }
}
