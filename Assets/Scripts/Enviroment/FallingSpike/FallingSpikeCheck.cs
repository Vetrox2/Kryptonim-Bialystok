using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpikeCheck : MonoBehaviour
{
    [SerializeField] GameObject Spike;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Health>() != null)
        {
            Spike.GetComponent<Rigidbody2D>().gravityScale = 1f;
        }
    }
}
