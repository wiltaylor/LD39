using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Maxspeed;
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

    private Rigidbody _rigidbody;
    private float _shootCooldown;
    private float _jumpRemaining;
    private bool _jumplock;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "levelgeom")
            _jumpRemaining = JumpPower;
    }


    void Start ()
	{
	    _rigidbody = GetComponent<Rigidbody>();
	    _jumpRemaining = JumpPower;
	    _jumplock = false;
	}

    void Update ()
    {


        if (Input.GetAxis("Vertical") > 0.01f)
        {
            Speed += Acceleration;

            if (Speed > Maxspeed)
                Speed = Maxspeed;

            
            _rigidbody.AddForce(transform.forward * Speed * Time.deltaTime);
        }


        if (Input.GetAxis("Vertical") < -0.01f)
        {
            Break += BreakForce;

            if (Break > Maxspeed)
                Break = Maxspeed;

            _rigidbody.AddForce(-transform.forward * Break * Time.deltaTime);
        }

        if (Input.GetAxis("Horizontal") < -0.01f)
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

        if (Input.GetButton("Fire1") && _shootCooldown <= 0f)
        {
            Spawner.Shoot();
            _shootCooldown = ShootCoolDown;
        }

        if (Input.GetButton("Jump") && _jumpRemaining > 0f)
        {
            _rigidbody.AddForce(Vector3.up * JumpSpeed * Time.deltaTime, ForceMode.Force);
            _jumpRemaining -= JumpUseRate * Time.deltaTime;
        }

    }
}
