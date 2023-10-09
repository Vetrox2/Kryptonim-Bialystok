using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LimitText : MonoBehaviour
{
    [Range(0,1)]
    public float fill;

    private TextMeshPro text;
    private string textContent;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshPro>();
        textContent = text.text;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        text.text = textContent.Substring(0, (int)(textContent.Length * fill));
    }
}
