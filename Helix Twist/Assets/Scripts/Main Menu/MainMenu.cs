using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private GameObject helpPanel;

    private void Start()
    {
        anim = GetComponent<Animator>();
        helpPanel.SetActive(false);
    }

    public void Play()
    {
        EventSystem.current.enabled = false;
        anim.Play("Fade");
    }

    public void Help()
    {
        helpPanel.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadLevel()
    {
        SceneManager.LoadSceneAsync("SampleScene");
        Time.timeScale = 0;
    }
}
