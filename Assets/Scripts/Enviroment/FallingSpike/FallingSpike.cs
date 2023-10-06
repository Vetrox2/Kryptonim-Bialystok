using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FallingSpike : MonoBehaviour
{

    private void Start()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        Health a = collision.GetComponent<Health>();
        if (a != null)
        {
            a.TakenDamage(a.maxHealth);
        }
        if (collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
