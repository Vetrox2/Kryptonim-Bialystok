using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] GameObject key;
    [SerializeField] List<GameObject> Trees = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(Loop());
    }
    IEnumerator Loop()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);
            if (CheckCrowPosition())
            {
                key.SetActive(true);
                break;
            }

        }
    }
    bool CheckCrowPosition()
    {
        bool ready = true;
        foreach (var t in Trees)
        {
            if (!t.GetComponent<PuzzleTreeManager>().Ready)
            {
                ready = false;
                break;
            }
        }
        return ready;
    }
}
