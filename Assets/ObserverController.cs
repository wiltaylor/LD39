using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserverController : MonoBehaviour
{

    public float MoveSpeed = 10f;
    public float RotationSpeed = 1f;

	void Update ()
    {
        if (Input.GetAxis("Horizontal") < -0.01f)
        {
            transform.Rotate(0, -RotationSpeed * Time.deltaTime, 0);
        }

        if (Input.GetAxis("Horizontal") > 0.01f)
        {
            transform.Rotate(0, RotationSpeed * Time.deltaTime, 0);
        }

        if (Input.GetAxis("Vertical") > 0.01f)
        {
            transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);
        }

        if (Input.GetAxis("Vertical") < -0.01f)
        {
            transform.Translate(Vector3.back * MoveSpeed * Time.deltaTime);
        }

        if (Input.GetButton("FlyUp"))
        {
            transform.Translate(Vector3.up * MoveSpeed * Time.deltaTime);
        }

        if (Input.GetButton("FlyDown"))
        {
            transform.Translate(Vector3.down * MoveSpeed * Time.deltaTime);
        }

    }
}
