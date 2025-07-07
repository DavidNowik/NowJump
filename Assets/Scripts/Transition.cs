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
                string world_ = travelToString.Substring(0, travelToString.Length - 1);

                if (lastChar == '9')
                {
                    travelToString = "9_9";
                    PlayerPrefs.SetInt(world_ + "level", 9);
                }
                else
                {
                    char incrementedChar = (char)(lastChar + 1);
                    travelToString = travelToString.Substring(0, travelToString.Length - 1) + incrementedChar;
                }


                if (lastChar - '0' > PlayerPrefs.GetInt(world_ + "level"))
                {
                    Debug.Log("Saving prefs: " + world_ + "level" + " as " + (lastChar - '0'));
                    PlayerPrefs.SetInt(world_ + "level", lastChar - '0');
                }
                else
                {
                    Debug.Log("Current last level (" + lastChar +
                        ") was lower then or equal to your highest " + PlayerPrefs.GetInt(world_ + "level"));
                }

                GameManager.instance.canvasManager.NextLevel();
                GameManager.instance.canvasManager.GoToLevel("Level" + travelToString.ToString());
                GameManager.instance.canvasManager.Reset(false);
            }
        }
           
    }
}
