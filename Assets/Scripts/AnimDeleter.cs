using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimDeleter : MonoBehaviour
{
    public void DeleteSelf()
    {
        Destroy(gameObject);
    }
}

