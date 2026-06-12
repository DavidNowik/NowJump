using JetBrains.Annotations;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public LevelReview[] world1 = new LevelReview[9];
    public LevelReview[] world2 = new LevelReview[9];
    public LevelReview[] world3 = new LevelReview[9];

    public TextMesh allStarsText;

    public GameObject world2Lock;
    public TextMesh lock2Text; 
    public GameObject world3Lock;
    public TextMesh lock3Text;


    private void Awake()
    {
        Debug.Log("Check if locking levels is necessary. Highest solved level is: "
            + PlayerPrefs.GetInt("1_level") + "(world 1) |"
            + PlayerPrefs.GetInt("2_level") + "(world 2)");
        for(int i = 9; i > PlayerPrefs.GetInt("1_level")+1; i--)
        {
            LockLevelW1(i);
        }
        for (int i = 9; i > PlayerPrefs.GetInt("2_level")+1; i--)
        {
            LockLevelW2(i);
        }
        for (int i = 9; i > PlayerPrefs.GetInt("3_level") + 1; i--)
        {
            LockLevelW3(i);
        }
        int allStars = AllStars();

        lock2Text.text = allStars + "/15";
        lock3Text.text = allStars + "/30";
        allStarsText.text ="all possible stars:\n" +allStars + "/90";

        if (allStars >= 15)
        {
            world2Lock.SetActive(false);
        }
        if (allStars >= 30)
        {
            world3Lock.SetActive(false);
        }
        // Aufruf in Start() oder Awake()
        StartCoroutine(UnlockAchievementsWhenReady(allStars));
    }
    private IEnumerator UnlockAchievementsWhenReady(int totalStars)
    {
        // Warte bis Steam initialisiert ist
        while (!Steamworks.SteamAPI.Init())
        {
            Debug.Log("Waiting for Steam to initialize...");
            yield return null;
        }

        Debug.Log("Steam initialized, unlocking achievements...");

        // Star achievements
        if (totalStars >= 10) Steamworks.SteamUserStats.SetAchievement("10_stars");
        if (totalStars >= 20) Steamworks.SteamUserStats.SetAchievement("20_stars");
        if (totalStars >= 30) Steamworks.SteamUserStats.SetAchievement("30_stars");
        if (totalStars >= 50) Steamworks.SteamUserStats.SetAchievement("50_stars");
        if (totalStars >= 70) Steamworks.SteamUserStats.SetAchievement("70_stars");
        if (totalStars >= 90) Steamworks.SteamUserStats.SetAchievement("90_stars");

        // World unlock achievements
        if (totalStars >= 15) Steamworks.SteamUserStats.SetAchievement("world_2");
        if (totalStars >= 30) Steamworks.SteamUserStats.SetAchievement("world_3");

        // Commit to Steam
        Steamworks.SteamUserStats.StoreStats();
        Debug.Log("Achievements unlocked and stored!");
    }




    private int AllStars()
    {
        int summe = 0;
        foreach (LevelReview lr in world1)
        {
            summe += lr.getStarAmount();
        }
        foreach (LevelReview lr in world2)
        {
            summe += lr.getStarAmount();
        }
        foreach (LevelReview lr in world3)
        {
            summe += lr.getStarAmount();
        }



        return summe;
    }
    public void LockLevelW1(int level)
    {
        if (level == 1) return;
        world1[level-1].transform.GetChild(5).GetChild(0).gameObject.SetActive(true);
        if(level >= 1)
            Steamworks.SteamUserStats.SetAchievement("first_level");
    }
    public void LockLevelW2(int level)
    {
        if (level == 1) return;
        world2[level-1].transform.GetChild(5).GetChild(0).gameObject.SetActive(true);
    }
    public void LockLevelW3(int level)
    {
        if (level == 1) return;
        world3[level - 1].transform.GetChild(5).GetChild(0).gameObject.SetActive(true);
    }
    void Update()
    {
        
    }

}
