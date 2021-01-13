using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private int savedScore = 0;
    [SerializeField] List<ColorTheme> colorThemes = new List<ColorTheme>();
    [SerializeField] private Material[] matsToChange;
    [SerializeField] private ColorTheme currentColorTheme;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void NextLevel(int scoreToSave)
    {
        ColorTheme prevColorTheme = currentColorTheme;

        currentColorTheme = colorThemes[Random.Range(0, colorThemes.Count)];

        if (savedScore > 0)
        {
            colorThemes.Add(prevColorTheme);
        }

        savedScore += scoreToSave;

        colorThemes.Remove(currentColorTheme);

        for (int i = 0; i < matsToChange.Length; i++)
        {
            matsToChange[i].color = currentColorTheme.colors[i];
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

[System.Serializable]
public class ColorTheme
{
    public Color[] colors;
}
