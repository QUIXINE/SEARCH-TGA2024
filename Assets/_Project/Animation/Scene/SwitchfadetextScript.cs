using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class SwitchfadetextScript : MonoBehaviour
{
    public Animator transition;
    public TextMeshProUGUI Text1;
    public int transitionTime = 1;
    private bool Switch=false;
    private bool Cango = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (Switch == false)
            {
                LoadNextLevel();
                Switch = true;
            }

            if (Switch == true && Input.GetKeyDown(KeyCode.Space))
            {
                FadeIN();

            }
            if (Cango == true && Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }
        }
    }
    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(1));
    }
    public void FadeIN()
    {
        StartCoroutine(Fadein(1));
    }
    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        Text1.text = "SEARCH";
    }
    IEnumerator Fadein(int FadeDu)
    {
        yield return new WaitForSeconds(transitionTime);
        transition.SetTrigger("Re");
        Cango = true;

    }

}
