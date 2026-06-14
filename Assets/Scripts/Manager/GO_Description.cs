using UnityEngine;

[DisallowMultipleComponent]
public class GO_Description : MonoBehaviour
{
    [TextArea(3, 10)]
    [SerializeField] private string description;

    public string Text => description;
}