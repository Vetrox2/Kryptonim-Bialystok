using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RocketLauncher : Weapon
{
    public GameObject Rocket;
    public Transform RocketSpawn;
    public float weaponDamageMultiplier = 1;

    public void shoot()
    {
        var rocket = Instantiate(Rocket, new Vector3(RocketSpawn.position.x, RocketSpawn.position.y, RocketSpawn.position.z), new Quaternion(0,0,0,0));
        IgnoreCollisionWithOwner(rocket);
    }
}
