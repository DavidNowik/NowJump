using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public GameObject CheckPointPrefab;
    private List<CheckPoint> CheckPoints = new List<CheckPoint>();

    private void Awake()
    {
        DeactivateAll();
        StartCoroutine(InitializeWhenReady());
    }

    private IEnumerator InitializeWhenReady()
    {
        // Wait until GameManager.instance exists
        yield return new WaitUntil(() => GameManager.instance != null);

        // Register this component
        GameManager.instance.checkPointManager = this;

        Debug.Log("Level was changed and not reloaded!" +
            " \nThis means all checkpoints-proxies will be instantiated");

        foreach (Transform child in transform)
        {
            GameObject go = Instantiate(CheckPointPrefab);
            go.transform.localPosition = child.position;
            child.gameObject.SetActive(false);
            CheckPoints.Add(go.GetComponent<CheckPoint>());
        }

        if (!GameManager.instance.levelChangingFlag)
        {
            Debug.Log("Level was reloaded!" +
               " \nThis means player will be relocated to active checkpoint");

            int index = PlayerPrefs.GetInt("cp");
            if (index > -1 && index < CheckPoints.Count)
            {
                StartCoroutine(WaitForPlayerThenRelocate());
            }
        }
    }



    IEnumerator WaitForPlayerThenRelocate()
    {
        yield return new WaitUntil(() => GameManager.instance.player != null);

        Debug.Log("Relocating Player!");

        CheckPoint activeCp = CheckPoints[PlayerPrefs.GetInt("cp")];
        activate(activeCp);
        GameManager.instance.player.transform.localPosition = activeCp.transform.position + Vector3.up*2;
    }
    public void activate(CheckPoint cp)
    {
        DeactivateAll();
        cp.isActive = true;
        PlayerPrefs.SetInt("cp", CheckPoints.IndexOf(cp));
    }
    public void DeactivateAll()
    {
        foreach (CheckPoint checkPoint in CheckPoints)
        {
            checkPoint.isActive = false;
        }
    }

}
