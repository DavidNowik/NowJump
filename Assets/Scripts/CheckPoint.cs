using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : ActivatableObject
{
    private bool _isActive = false;
    public override bool isActive
    {
        get => _isActive;
        set => _isActive = value;
    }
    private void Update()
    {
        GetComponent<Animator>().SetBool("active", isActive);
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            GameManager.instance.checkPointManager.activate(this);
        }
    }

}
