using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Bullet
{
    private void FixedUpdate()
    {
        RocketFly(BulletSpeed);
    }
    protected void RocketFly(float Speed)
    {
        rb.velocity = new Vector3(-Speed * Mathf.Sin(transform.localRotation.eulerAngles.z / 180 * Mathf.PI),
                Speed * Mathf.Cos(transform.localRotation.eulerAngles.z / 180 * Mathf.PI));
    }
}
