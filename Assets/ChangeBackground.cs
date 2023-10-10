using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBackground : MonoBehaviour
{
    [SerializeField] Sprite newSprite;
    public void ChangeSprite()
    {
        GetComponent<SpriteRenderer>().sprite = newSprite;
    }
}
