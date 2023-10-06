using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : Enemy
{
    public bool Activated = false;
    bool play = false;
    [SerializeField] AudioSource ads;
    private void Update()
    {
        if(Activated)
        {
            if (!play)
            {
                ads.Play();
                play = true;
            }
            EnemyMove();
        }
    }
}
