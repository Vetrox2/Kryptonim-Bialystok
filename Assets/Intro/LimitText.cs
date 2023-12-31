using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LimitText : MonoBehaviour
{
    [Range(0,1)]
    public float fill;
    [SerializeField] Animator backgroundCtr;
    [SerializeField] Animator textbackgroundCtr;
    [SerializeField] List<string> texts = new List<string>();
    [SerializeField] AudioSource src;
    [SerializeField] List<AudioClip> Clips;
    [SerializeField] bool canBlackOut = true;
    [SerializeField] GameObject endScreen;
    private int i = 0;
    private TextMeshPro text;
    private string textContent;
    private bool textEnded= false;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshPro>();
        if (!canBlackOut)
        {
            GetComponent<Animator>().SetBool("CanBlackOut", false);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (textContent != null && !textEnded)
        {
            text.text = textContent.Substring(0, (int)(textContent.Length * fill));
            if(fill==1)
                textEnded = true;
        }
        if(texts.Count == i && textEnded) {
            
            Invoke("NextLVL", 2f);
        }
    }
    public void StartBGBlackout()
    {
        if (texts.Count != i && backgroundCtr != null && textbackgroundCtr != null)
        {
            backgroundCtr.SetBool("ChangeBG", true);
            textbackgroundCtr.SetBool("ChangeBG", true);
        }
    }
    public void GetNextText()
    {
        if (texts.Count > i)
        {
            fill = 0;
            text.text = "";
            textContent = texts[i];
            src.clip = Clips[i];
            src.Play();
            i++;
            if(texts.Count == i)
            {
                GetComponent<Animator>().SetBool("Ended", true);
            }
            textEnded = false;
        }
    }
    private void NextLVL()
    {
        if (SceneManager.sceneCountInBuildSettings != SceneManager.GetActiveScene().buildIndex + 1)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
        {
            textContent = "";
            text.text = "";
            endScreen.SetActive(true);
            textbackgroundCtr.gameObject.SetActive(false);
            Invoke("GetToMenu", 15f);
        }
    }
    private void GetToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
