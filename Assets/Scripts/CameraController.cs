using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject player;
    [SerializeField] GameObject CM;
    private void Update()
    {
        if (player == null)
        {
            player = GameManager.FindPlayer();
            CM.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
        }
    }
}
