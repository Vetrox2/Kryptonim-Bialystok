using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform LaserFirePoint;
    public float damage = 1;

    bool LaserDamageReady = true;
    private void Update()
    {
        ShootLaser();
    }
    public void ShootLaser()
    {
        Vector2 startPos = transform.InverseTransformPoint(LaserFirePoint.position);
        if (Physics2D.Raycast(LaserFirePoint.position, LaserFirePoint.transform.right))
        {
            //Debug.Log();
            RaycastHit2D[] hit = Physics2D.RaycastAll(LaserFirePoint.position, LaserFirePoint.transform.right, 50, 1 << LayerMask.NameToLayer("Action"));
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].collider.CompareTag("Obstacle"))
                {
                    DrawLaser(startPos, transform.InverseTransformPoint(hit[i].point));
                    break;
                }
            }
            if (LaserDamageReady)
                CheckCollisionWithPlayer(hit);
            
        }
    }
    void DrawLaser(Vector2 startPos, Vector2 endPos)
    {
        
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }
    void CheckCollisionWithPlayer(RaycastHit2D[] hit)
    {
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.CompareTag("Obstacle"))
                break;
            if (hit[i].collider.CompareTag("Player"))
            {
                hit[i].collider.GetComponent<Health>().TakenDamage(damage);
                LaserDamageReady = false;
                Invoke("DamageCooldown", 0.5f);
            }
        }
    }
    void DamageCooldown()
    {
        LaserDamageReady = true;
    }

}
