using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI startScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject levelPanel;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject pausePanel;
    public GameObject LevelPanel { get { return levelPanel; } }
    private Animator anim;
    public bool died = false;
    public bool mainMenu = false;

    private int score = 0;

    private void Start()
    {
        anim = GetComponent<Animator>();
        levelPanel.SetActive(true);
        pauseButton.SetActive(false);
        pausePanel.SetActive(false);
        scoreText.gameObject.SetActive(false);
        levelText.text = (GameManager.instance.Level + 1).ToString();
        startScoreText.text = GameManager.instance.SavedScore.ToString();
        highScoreText.text = PlayerPrefs.GetInt("highScore", 0).ToString();
    }

    public void RemoveStartPanel()
    {
        levelPanel.SetActive(false);
        scoreText.gameObject.SetActive(true);
        pauseButton.SetActive(true);
        Time.timeScale = 1;
    }

    public void TogglePause(bool pause)
    {
        pauseButton.SetActive(!pause);
        pausePanel.SetActive(pause);
        Time.timeScale = pause ? 0 : 1;
    }

    public void ReturnToMainMenu()
    {
        EventSystem.current.enabled = false;
        mainMenu = true;
        anim.Play("Fade");
    }

    public void UpdateScore(int i)
    {
        score = i;
        scoreText.text = i.ToString();
    }

    public void StartFade(bool restart)
    {
        pauseButton.SetActive(false);
        died = restart;
        anim.Play("Fade");
    }

    public void LoadLevel()
    {
        if (mainMenu)
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        if (died)
        {
            GameManager.instance.PlayerDied();
        }
        else
        {
            GameManager.instance.NextLevel(score);
        }
    }
}
