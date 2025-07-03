using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class LightToggle : Trap
{
    private bool _isActive;

    public Sprite offImage, onImage;
    [SerializeField] private ActivatableObject[] activateObjects; // direct reference to base class
    [SerializeField] private GameObject activateObjectsParent;

    private Animator anim;

    public override bool isActive
    {
        get => _isActive;
        set
        {
            _isActive = value;
        }
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        isActive = false;
        PopulateActivateObjects();
    }
    private void PopulateActivateObjects()
    {

        if (activateObjectsParent != null)
        {
            // Get all from parent, including inactive ones
            var foundObjects = activateObjectsParent.GetComponentsInChildren<ActivatableObject>(includeInactive: true);

            // Combine with existing ones if any (and remove duplicates)
            if (activateObjects != null && activateObjects.Length > 0)
            {
                // Use a HashSet to avoid duplicates
                var combined = new HashSet<ActivatableObject>(activateObjects);
                foreach (var obj in foundObjects)
                {
                    combined.Add(obj);
                }

                activateObjects = new ActivatableObject[combined.Count];
                combined.CopyTo(activateObjects);
            }
            else
            {
                // No manual ones, just use found
                activateObjects = foundObjects;
            }
        }
    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        isActive = !isActive;
        anim.SetBool("isActive", isActive);
        ToggleObjects();
    }

    private void ToggleObjects()
    {
        foreach (var obj in activateObjects)
        {
            if (obj != null)
                obj.isActive = !obj.isActive;
        }
    }
}
