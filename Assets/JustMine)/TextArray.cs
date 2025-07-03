using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextArray : MonoBehaviour
{
    [TextArea(3, 10)]
    public string[] texts;

    private int currentIndex = 0;
    private TextMesh textMesh;

    void Start()
    {
        textMesh = GetComponent<TextMesh>();
        if (texts.Length > 0)
        {
            textMesh.text = texts[0];
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (texts.Length == 0) return;

            currentIndex++;

            if (currentIndex >= texts.Length)
            {
                // Alle Texte durch, Objekt l�schen
                Destroy(gameObject);
                return; // Nicht mehr weitermachen, da Objekt zerst�rt
            }

            textMesh.text = texts[currentIndex];
        }
    }
}
