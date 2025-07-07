using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineBlock : MonoBehaviour
{
    [SerializeField]
    private float force;
    private void Awake()
    {
        foreach(Transform child in transform)
        {
            if(child.GetComponent<Trampoline>() != null)
                child.GetComponent<Trampoline>().force = force;
        }
    }
}
