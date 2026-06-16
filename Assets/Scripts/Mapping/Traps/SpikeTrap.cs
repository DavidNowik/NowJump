using UnityEngine;

public class SpikeTrap : Trap
{
    private bool _isActive = true; // default to active
    public override bool isActive
    {
        get => _isActive;
        set => _isActive = value;
    }
    private void Update()
    {
        if(GetComponent<SpriteRenderer>() != null)
            GetComponent<SpriteRenderer>().enabled = isActive;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActive) return;

        base.OnTriggerEnter2D(collision);
        // Optional: add specific SpikeTrap effects here (sound, particles, etc.)
    }
}
