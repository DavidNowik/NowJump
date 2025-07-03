using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    private string travelToString = "";//is sowas wie 1_1 oder 2_4
    public string travelOverride = "";//is sowas wie 1_1 oder 2_4

    private void Awake()
    {
        travelToString = CanvasManager.GetCurrentLevelString();
    }
    public void CheckLock()
    {
        Debug.Log("Compare "+ PlayerPrefs.GetInt(travelOverride[0] + "_level")+" to "+ travelOverride[travelOverride.Length - 1]);
        int finishedLevelPlusTwo = PlayerPrefs.GetInt(travelOverride[0] + "_level")+2;
        int targetLevel = travelOverride[travelOverride.Length - 1] - '0';

        if (finishedLevelPlusTwo <= targetLevel && travelOverride[travelOverride.Length - 1] != '1')
        {
            Debug.Log("Lock Portal: " + travelOverride);
            transform.GetChild(0).gameObject.SetActive(true);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if (travelOverride == "-1")
            {//FULL RESET
                GameManager.instance.FullReset();
                GameManager.instance.canvasManager.GoToLevel("1_1");
                GameManager.instance.canvasManager.Reset(false);

            }
            else if (travelOverride != "")
            {
                GameManager.instance.canvasManager.GoToLevel(travelOverride);
                GameManager.instance.canvasManager.Reset(false);
            }
            else
            {
                if(travelToString.Length == 0)
                {
                    Debug.Log("I was triggered "+CanvasManager.GetCurrentLevelString());
                    travelToString = CanvasManager.GetCurrentLevelString();
                }
                char lastChar = travelToString[travelToString.Length - 1];

                if (lastChar == '9')
                {
                    travelToString = "9_9";
                }
                else
                {
                    char incrementedChar = (char)(lastChar + 1);
                    travelToString = travelToString.Substring(0, travelToString.Length - 1) + incrementedChar;
                }

                string world_ = travelToString.Substring(0, travelToString.Length - 1);

                if (lastChar - '0' > PlayerPrefs.GetInt(world_ + "level"))
                {
                    Debug.Log("Saving prefs: " + world_ + "level" + " as " + (world_ + lastChar));
                    PlayerPrefs.SetInt(world_ + "level", lastChar - '0');
                }

                GameManager.instance.canvasManager.PrepareLevelChange();//Deletes all Checkpoints
                GameManager.instance.canvasManager.NextLevel();

                SceneManager.LoadScene("Level" + travelToString.ToString());
                GameManager.instance.canvasManager.Reset(false);
            }
        }
           
    }
}
