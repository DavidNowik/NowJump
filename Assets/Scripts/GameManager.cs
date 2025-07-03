using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public CanvasManager canvasManager;
    public GameObject canvasPrefab;
    public Player player;
    public GameObject textArray;
    public Transform checkPoint;
    [SerializeField] private GameObject dropTextPrefab;


    private void Awake()
    {
        // If there's already an instance and it's not this one, destroy the duplicate
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);
        instance = this;
    }
    private void Update()
    {
        MakeSureOfCanvas();
        MakeSureOfPlayer();
        if (Input.GetKeyDown(KeyCode.I))
        {
            canvasManager.PrepareLevelChange();
            canvasManager.GoToLevel("9_9");
        }
    }

    public void MakeSureOfCanvas()
    {
        if (canvasPrefab == null)
        {
            // Load the prefab from Resources/Prefab/Canvas.prefab
            canvasPrefab = Resources.Load<GameObject>("Prefab/Canvas");
            if (canvasPrefab == null)
            {
                Debug.LogError("Canvas prefab not found in Resources/Prefab!");
                return;
            }
        }

        if (canvasManager == null)
        {
            GameObject canvasGO = GameObject.Find("Canvas");
            if (canvasGO == null)
            {
                canvasGO = Instantiate(canvasPrefab);
                canvasGO.name = "Canvas"; // Optional: ensure consistent naming
                canvasManager = canvasGO.GetComponent<CanvasManager>();
            }
            else
            {
                canvasManager = canvasGO.GetComponent<CanvasManager>();
            }
        }
    }
    public void MakeSureOfPlayer()
    {
        if(player == null)
        {
            player = GameObject.Find("Player").GetComponent<Player>();
        }
    }
    public void CreateTextArray(string[] texts)
    {
        Vector3 spawnPos = player.transform.position + Vector3.up*4; // 1 unit above
        Instantiate(textArray, spawnPos, Quaternion.identity)
            .GetComponent<TextArray>().texts = texts;
    }

    public void FullReset()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save(); // Optional but ensures it's written immediately
    }
    public void DropText(string text, int fontSize, Vector3 position)
    {
        TMP_Text tmpText = 
            Instantiate(dropTextPrefab, position, Quaternion.identity)
            .transform.GetChild(0).GetComponent<TMP_Text>();
        tmpText.text = text;
        tmpText.fontSize = fontSize; 
        tmpText.gameObject.GetComponent<Animator>()
            .SetBool("DropRight", UnityEngine.Random.value > 0.5f);

    }
    public void DropText(string text)
    {
        DropText(text, 40, Vector3.zero);
    }
    public void DropText(string text, Vector2 position)
    {
        DropText(text, 40,position);
    }


}
