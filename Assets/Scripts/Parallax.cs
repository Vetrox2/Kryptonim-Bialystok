using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    Camera Cam;
    [SerializeField] float parallaxModifierX = 0f;
    [SerializeField] float parallaxModifierY = 0f;
    float startPos;
    float startPosY, startCamY;
    float lenght;
    [SerializeField] bool tp = true;
    private void Start()
    {
        Cam = Camera.main;
        startCamY = Cam.transform.position.y;
        startPos = transform.position.x;
        startPosY = transform.position.y;
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
    }
    private void Update()
    {
        float temp = (Cam.transform.position.x * (1 - parallaxModifierX));
        float dist = (Cam.transform.position.x * parallaxModifierX);
        float distY = (startCamY - Cam.transform.position.y) * parallaxModifierY;
        transform.position = new Vector3(startPos + dist, startPosY - distY);
        if (tp)
        {
            if (temp > startPos + lenght) startPos += lenght;
            else if (temp < startPos - lenght) startPos -= lenght;
        }
    }
}
