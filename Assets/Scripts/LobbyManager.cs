using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public LevelReview[] world1 = new LevelReview[9];
    public LevelReview[] world2 = new LevelReview[9];
    public LevelReview[] world3 = new LevelReview[9];

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
        int allStars = PlayerPrefs.GetInt("Stars");
        lock2Text.text = allStars + "/15";
        lock3Text.text = allStars + "/30";
        if (allStars >= 15)
        {
            world2Lock.GetComponent<ActivatableObject>().isActive = false;
            lock2Text.text = "15/15!";
        }
        if (allStars >= 30)
        {
            world3Lock.GetComponent<ActivatableObject>().isActive = false;
            lock3Text.text = "30/30!";
        }
    }
    public void LockLevelW1(int level)
    {
        if (level == 1) return;
        world1[level-1].transform.GetChild(5).GetChild(0).gameObject.SetActive(true);
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
