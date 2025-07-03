using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class XantiScript : MonoBehaviour
{
    [SerializeField] private string[] texts;
    public GameObject starHolder;

    private TextMesh talkText;
    private int textCount = 0;
    private Animator anim;
    private void Awake()
    {
        anim = transform.parent.GetComponent<Animator>();
        talkText = transform.GetChild(0).GetComponent<TextMesh>();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null)
        {
            if(collision.name == "Player")
            {
                anim.SetTrigger("talk");
                if (textCount >= 2)
                {
                    if(starHolder !=null)
                        starHolder.SetActive(true);
                }

                talkText.text = texts[textCount++];
                if(textCount == texts.Length)
                {
                    textCount = texts.Length - 1;
                }
            }
        }
    }
}
