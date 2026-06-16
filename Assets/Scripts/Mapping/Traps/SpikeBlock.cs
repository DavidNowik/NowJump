using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBlock : ActivatableObject
{
    private bool _isActive = true;
    private Animator anim;
    [SerializeField] private bool startActive = true;

    private void Awake()
    {
        isActive = startActive;
        anim = GetComponent<Animator>();
        var foundObjects = transform.GetComponentsInChildren<ActivatableObject>(includeInactive: true);
        foreach (var foundObject in foundObjects)
        {
            foundObject.isActive = isActive;
        }
    }
    public override bool isActive
    {
        get => _isActive;
        set => _isActive = value;
    }

    private void Update()
    {
        anim.SetBool("isActive", isActive);
        
    }
}
