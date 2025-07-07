using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public Text levelText;
    public Text deathsText;
    public Text jumpsText;
    public Text appleText;
    public Text kiwiText;
    public Text starText;


    private int stars = 0;
    private bool isResetting = false;

    private void Awake()
    {
        // Ensure text fields are properly assigned
        if (appleText == null)
        {
            GameObject appleObj = GameObject.Find(nameof(appleText));
            if (appleObj != null)
                appleText = appleObj.GetComponent<Text>();
        }

        if (kiwiText == null)
        {
            GameObject kiwiObj = GameObject.Find(nameof(kiwiText));
            if (kiwiObj != null)
                kiwiText = kiwiObj.GetComponent<Text>();
        }

        if (starText == null)
        {
            GameObject starObj = GameObject.Find(nameof(starText));
            if (starObj != null)
                starText = starObj.GetComponent<Text>();
        }

        if (levelText == null)
        {
            GameObject levelObj = GameObject.Find(nameof(levelText));
            if (levelObj != null)
            {
                levelText = levelObj.GetComponent<Text>();
            }
        }
        if (deathsText == null)
        {
            GameObject deathsObj = GameObject.Find("DeathsText");
            if (deathsObj != null)
            {
                deathsText = deathsObj.GetComponent<Text>();
            }
        }
        if (jumpsText == null)
        {
            GameObject jumpsObj = GameObject.Find("JumpsText");
            if (jumpsObj != null)
            {
                jumpsText = jumpsObj.GetComponent<Text>();
            }
        }
        // Delay setting levelText until after it's confirmed it's not null
        if (levelText != null)
        {
            string currentLevel = GetCurrentLevelString();
            levelText.text = "Level: " + currentLevel;
        }
        if (deathsText != null)
        {
            int deaths = PlayerPrefs.GetInt("Deaths");
            deathsText.text = "Deaths: " + deaths;
        }
        isResetting = false;
    }
    public void Reset(bool died)
    {
        if (isResetting) return;
        stars = 0;
        starText.text = "Stars: " + stars;

        if (died)
        {
            GameManager.instance.levelChangingFlag = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            int deaths = PlayerPrefs.GetInt("Deaths");
            PlayerPrefs.SetInt("Deaths", deaths + 1);
            deathsText.text = "Deaths: " + (deaths + 1);
            isResetting = true;
            
        } 
    }
    public void ResetAfterSceneLoad()
    {
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            isResetting = false;  // Reset the flag to false
        };
    }
    public void IncreaseStars()
    {
        int allStars = PlayerPrefs.GetInt("Stars");
        PlayerPrefs.SetInt("Stars", allStars + 1);
        string currentLevel = GetCurrentLevelString();
        stars++;
        starText.text = "Stars: "+stars;
    }
    public void NextLevel()
    {
        string currentLevel = GetCurrentLevelString();
        if (stars <= 3)
        {
            PlayerPrefs.SetInt("Level" + currentLevel + "Stars", stars);
        }
        levelText.text = "Level: " + currentLevel;
    }
    public void GoToLevel(string level)
    {
        if (level.StartsWith("Level"))
        {
            GameManager.instance.levelChangingFlag = true;
            PlayerPrefs.SetInt("cp", -1);
            SceneManager.LoadScene(level);
        }
        else
        {
            GameManager.instance.levelChangingFlag = true;
            PlayerPrefs.SetInt("cp", -1);
            SceneManager.LoadScene("Level" + level);
        }
    }
    public static string GetCurrentLevelString()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName.Length >= 3)
        {
            return sceneName.Substring(sceneName.Length - 3);
        }
        else
        {
            Debug.LogWarning("Scene name is too short to contain a valid level identifier.");
            return "0_0";
        }
    }
}
