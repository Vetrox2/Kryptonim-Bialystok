using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volume : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] float volume;
    public float GetVolume() { return volume; }
    private void Awake()
    {
        volume = GetComponent<AudioSource>().volume;
    }
}
