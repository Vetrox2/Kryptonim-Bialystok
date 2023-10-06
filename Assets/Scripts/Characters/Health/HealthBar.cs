using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image healthBar;
    public GameObject Target;
    private void Update()
    {
        if (Target != null && healthBar != null)
            healthBar.fillAmount = Target.GetComponent<Health>().GetHealth() / Target.GetComponent<Health>().maxHealth;
    }
}
