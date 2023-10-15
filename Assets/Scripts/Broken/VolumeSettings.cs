using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [Range(0f, 1f)]
    public float Volume = 1f;
    [SerializeField] Slider volumeSlider;
    [SerializeField] TextMeshProUGUI volumeFloat;
    private void Start()
    {
        Volume = PlayerPrefs.GetFloat("Volume");
        volumeSlider.value = Volume;
        volumeFloat.text = Math.Round(Volume,2).ToString();
    }
    static public void SetVolume()
    {
        AudioSource[] adss = FindObjectsOfType<AudioSource>();
        foreach (AudioSource ads in adss)
        {
            Debug.Log("volume "+PlayerPrefs.GetFloat("Volume"));
            if(ads.gameObject.GetComponent<Volume>() != null)
                ads.volume = ads.gameObject.GetComponent<Volume>().GetVolume() * PlayerPrefs.GetFloat("Volume"); 
            Debug.Log(ads.volume);
        }
    }
    public void ChangeVolume()
    {
        Volume = volumeSlider.value;
        volumeFloat.text = Math.Round(Volume, 2).ToString();
        PlayerPrefs.SetFloat("Volume", Volume);
        SetVolume();
    }
}
