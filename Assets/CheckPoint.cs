using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : ActivatableObject
{
    private bool _isActive = true;
    public override bool isActive
    {
        get => _isActive;
        set => _isActive = value;
    }
    public void Awake()
    {
        transform.parent = GameObject.Find("GameManager").transform;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            isActive = true;
            Debug.Log("CheckPoint reached");
            GetComponent<Animator>().SetTrigger("active");
            GameManager.instance.checkPoint = transform.GetChild(0);
        }
    }

}
