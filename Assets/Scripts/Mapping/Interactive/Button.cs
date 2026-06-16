using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    private bool hasBeenPressed;

    [SerializeField] private ActivatableObject[] activateObjects;
    [SerializeField] private GameObject activateObjectsParent;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        PopulateActivateObjects();
    }

    private void PopulateActivateObjects()
    {
        if (activateObjectsParent != null)
        {
            var foundObjects = activateObjectsParent.GetComponentsInChildren<ActivatableObject>(true);

            var combined = new HashSet<ActivatableObject>(activateObjects);

            foreach (var obj in foundObjects)
            {
                combined.Add(obj);
            }

            activateObjects = new ActivatableObject[combined.Count];
            combined.CopyTo(activateObjects);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasBeenPressed)
            return;

        hasBeenPressed = true;

        anim.SetTrigger("activate");

        ActivateObjects();
    }

    private void ActivateObjects()
    {
        foreach (var obj in activateObjects)
        {
            if (obj != null)
                obj.isActive = !obj.isActive;
        }
    }
}