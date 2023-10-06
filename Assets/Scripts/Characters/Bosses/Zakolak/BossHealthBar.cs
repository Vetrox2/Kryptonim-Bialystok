using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] Image HealthBar;
    public GameObject Target;
    
    private void Update()
    {
        if (Target == null)
        {
            StartCoroutine(HideHealthBar());
        }
        if (Target != null && HealthBar != null)
            HealthBar.fillAmount = Target.GetComponent<Health>().GetHealth() / Target.GetComponent<Health>().maxHealth;
    }
    IEnumerator HideHealthBar()
    {
        yield return new WaitForSeconds (2f);
        if(Target == null)
            gameObject.SetActive (false);
    }
}
