using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Transform checkPointLeft, checkPointRight;
    [SerializeField] float movementSpeed = 1f;
    float direction = 1f;
    private void FixedUpdate()
    {
        if (transform.position.x >= checkPointRight.position.x)
            direction = -1f;
        if (transform.position.x <= checkPointLeft.position.x)
            direction = 1f;
        GetComponent<Rigidbody2D>().velocity = new Vector3(movementSpeed * direction, 0);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Character>().ChangeToExtrapolate();
            collision.gameObject.GetComponent<Character>().AddVelocity(GetComponent<Rigidbody2D>().velocity);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Character>().ChangeToInterpolate();
            collision.gameObject.GetComponent<Character>().AddVelocity(Vector2.zero);
        }
    }

}
