using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class JavelinRocket : Rocket
{
    [SerializeField]float raisingTime = 0.3f;
    [SerializeField] float raisingSpeed = 6f;
    [SerializeField] float changeAngleTime = 1f;
    float raisingTimeLeft;
    bool hasTarged;
    Vector3 targerPosition;
    new private void Start()
    {
        base.Start();
        raisingTimeLeft = Time.time +raisingTime;
        hasTarged = false;
    }
    private void FixedUpdate()
    {
        if (raisingTimeLeft > Time.fixedTime)
        {
            RocketFly(raisingSpeed);
        }
        else if(!hasTarged)
        { 
            hasTarged = true;
            StartCoroutine(ChangeAngle());      
        }
        else
        {
            RocketFly(BulletSpeed);
        }
    }
    
    IEnumerator ChangeAngle()
    {
        float time = 0;
        float angle;
        float startAngle = transform.localRotation.eulerAngles.z;
        float endAngle = 0;
        while (time < changeAngleTime) 
        {
            targerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
            endAngle = Mathf.Atan((transform.position.y - targerPosition.y) / (transform.position.x - targerPosition.x)) / Mathf.PI * 180 + 90;
            if (transform.position.x < targerPosition.x)
            {
                endAngle -= 180;
            }
            angle = Mathf.Lerp(startAngle, endAngle, time/changeAngleTime);
            transform.rotation = Quaternion.Euler(0, 0, angle);
            time += Time.deltaTime;
            yield return null;
        }
        transform.rotation = Quaternion.Euler(0, 0, endAngle);
    }
}
