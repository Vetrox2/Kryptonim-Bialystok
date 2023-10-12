using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBackground : MonoBehaviour
{
    [SerializeField] List<Sprite> newSprite = new();
    private int i = 0;
    public void ChangeSprite()
    {
        if (newSprite.Count > i)
        {
            GetComponent<SpriteRenderer>().sprite = newSprite[i];
            i++;
        }

    }
    public void StopBlackout()
    {
        GetComponent<Animator>().SetBool("ChangeBG", false);
    }
}
