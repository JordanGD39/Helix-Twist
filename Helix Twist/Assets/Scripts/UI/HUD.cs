using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject levelPanel;
    public GameObject LevelPanel { get { return levelPanel; } }
    private Animator anim;
    public bool died = false;

    private int score = 0;

    private void Start()
    {
        anim = GetComponent<Animator>();
        levelPanel.SetActive(true);
        scoreText.gameObject.SetActive(false);
        levelText.text = (GameManager.instance.Level + 1).ToString();
    }

    public void RemoveStartPanel()
    {
        levelPanel.SetActive(false);
        scoreText.gameObject.SetActive(true);
        Time.timeScale = 1;
    }

    public void UpdateScore(int i)
    {
        score = i;
        scoreText.text = i.ToString();
    }

    public void StartFade(bool restart)
    {
        died = restart;
        anim.Play("Fade");
    }

    public void LoadLevel()
    {
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
