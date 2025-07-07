using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherShroom : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            Debug.Log("Touched Launcher");
            GetComponent<Animator>().SetTrigger("touched");
        }
    }
}
