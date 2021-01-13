using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private int savedScore = 0;
    public int SavedScore { get { return savedScore; } }
    [SerializeField] private ColorTheme[] colorThemes;
    private List<ColorTheme> colorThemesList = new List<ColorTheme>();
    [SerializeField] private Material[] matsToChange;
    [SerializeField] private ColorTheme currentColorTheme;

    private Color bgGolor;
    public Color BgColor { get { return bgGolor; } }
    [SerializeField] private int level = 0;
    public int Level { get { return level; } }

    [SerializeField] private int partsToRemovePerSpiral = 5;
    [SerializeField] private int deathPartsPerSpiral = 1;

    private int startingPartsToRemove = 5;
    private int startingDeathParts = 5;

    public int PartsToRemovePerSpiral { get { return partsToRemovePerSpiral; } }
    public int DeathPartsPerSpiral { get { return deathPartsPerSpiral; } }

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

    private void Start()
    {
        startingPartsToRemove = partsToRemovePerSpiral;
        startingDeathParts = deathPartsPerSpiral;

        for (int i = 0; i < matsToChange.Length; i++)
        {
            matsToChange[i].color = colorThemes[0].colors[i];
        }

        currentColorTheme = colorThemes[0];

        colorThemesList = new List<ColorTheme>(colorThemes);

        colorThemesList.RemoveAt(0);

        Time.timeScale = 0;
    }

    public void PlayerDied()
    {
        colorThemesList = new List<ColorTheme>(colorThemes);
        colorThemesList.RemoveAt(0);

        partsToRemovePerSpiral = startingPartsToRemove;
        deathPartsPerSpiral = startingDeathParts;

        level = 0;
        savedScore = 0;

        currentColorTheme = colorThemes[0];

        LoadLevel();
    }

    public void NextLevel(int scoreToSave)
    {
        level++;

        if (level == 5 || level == 20 || level == 40)
        {
            partsToRemovePerSpiral--;
        }

        if (level == 8 || level == 30 || level == 50 || level == 80)
        {
            deathPartsPerSpiral++;
        }

        ColorTheme prevColorTheme = currentColorTheme;

        int rand = Random.Range(0, colorThemesList.Count);

        currentColorTheme = colorThemesList[rand];
        savedScore = scoreToSave;

        colorThemesList.RemoveAt(rand);

        colorThemesList.Add(prevColorTheme);

        LoadLevel();
    }

    private void LoadLevel()
    {
        for (int i = 0; i < matsToChange.Length; i++)
        {
            matsToChange[i].color = currentColorTheme.colors[i];
        }

        bgGolor = currentColorTheme.colors[4];

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        Time.timeScale = 0;
    }
}

[System.Serializable]
public class ColorTheme
{
    public Color[] colors;
}
