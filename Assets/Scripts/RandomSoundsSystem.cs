using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundsSystem : MonoBehaviour
{
    [SerializeField] List<AudioClip> sounds = new List<AudioClip>();
    [SerializeField] AudioSource audioSource;
    void Start()
    {
        StartCoroutine(RandomClip());
    }

    IEnumerator RandomClip()
    {
        while (true)
        {
            int i = Random.Range(0, sounds.Count);
            audioSource.clip = sounds[i];
            audioSource.Play();
            yield return new WaitForSeconds(25f);
        }
    }
}
