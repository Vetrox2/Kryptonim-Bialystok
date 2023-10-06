using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTreeManager : MonoBehaviour
{
    [SerializeField] bool Blue = false;
    [SerializeField] bool Red = false;
    [SerializeField] bool Yellow = false;
    [HideInInspector]public bool Ready = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PuzzleCrowManager a;
        if (collision.TryGetComponent<PuzzleCrowManager>(out a))
        {
            if (collision.GetComponent<PuzzleCrowManager>().Blue && Blue)
            {
                Ready = true;
            }
            if (collision.GetComponent<PuzzleCrowManager>().Red && Red)
            {
                Ready = true;
            }
            if (collision.GetComponent<PuzzleCrowManager>().Yellow && Yellow)
            {
                Ready = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        PuzzleCrowManager a;
        if (collision.TryGetComponent<PuzzleCrowManager>(out a))
        {
            if (collision.GetComponent<PuzzleCrowManager>().Blue && Blue)
            {
                Ready = false;
            }
            if (collision.GetComponent<PuzzleCrowManager>().Red && Red)
            {
                Ready = false;
            }
            if (collision.GetComponent<PuzzleCrowManager>().Yellow && Yellow)
            {
                Ready = false;
            }
        }
    }
}
