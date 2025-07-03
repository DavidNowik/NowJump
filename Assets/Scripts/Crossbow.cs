using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbow : MonoBehaviour
{

    private Animator anim;
    private bool loaded;
    private bool loading;
    private bool powerLoaded;

    private float loadTimer;

    [Header("Arrow info")]
    [SerializeField] private SpriteRenderer loadedArrowRenderer;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float shotForce;


    [Header("Power Load Settings")]
    [SerializeField] private GameObject flashPrefab;
    [SerializeField] private float perfectMin = 0.5f;
    [SerializeField] private float perfectMax = 0.7f;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        anim.SetBool("loading", loading);
        anim.SetBool("loaded", loaded);
        // Start loading
        if (Input.GetMouseButtonDown(1) && !loaded)
        {
            loading = true;
            loadTimer = 0f;
        }

        // While holding, count loading time
        if (loading)
        {
            anim.ResetTrigger("shoot");
            loadTimer += Time.deltaTime;
        }

        // Releasing the button
        if (Input.GetMouseButtonUp(1) && loading)
        {
            loading = false;
            anim.SetBool("loading", false);

            if (loadTimer >= perfectMin && loadTimer <= perfectMax)
            {
                Instantiate(flashPrefab, transform.position, Quaternion.identity);
                powerLoaded = true;
                Debug.Log("🔥 Power Loaded!");
                FinishLoad();
            }
            else if (loadTimer > perfectMin)
            {
                powerLoaded = false;
                Debug.Log("✅ Normal Load");
                FinishLoad();
            }
            anim.SetBool("loaded", loaded);
        }

        // Shoot if loaded
        if (Input.GetMouseButtonDown(1) && loaded)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        anim.SetTrigger("shoot");
        loading = false;
        loaded = false;
        int powerLoadBuff = 0;

        float finalForce = shotForce;

        

        // Get spawn position, rotation, and direction
        Vector3 spawnPos = loadedArrowRenderer.transform.position;
        Quaternion spawnRot = loadedArrowRenderer.transform.rotation;
        int facingDir = GameManager.instance.player.facingDirection;

        // Instantiate arrow
        GameObject arrow = Instantiate(arrowPrefab, spawnPos, spawnRot);
        int currentPower =
            arrow.GetComponent<IWeaponPower>().Power;

        if (powerLoaded)
        {
            Debug.Log("💥 Power Shot!");
            finalForce *= 1.3f;
            powerLoadBuff = (int)(currentPower * 1.5f);
            arrow.GetComponentInParent<Arrow>().Power = powerLoadBuff;
        }

        // Flip arrow visually if facing left
        if (facingDir == -1)
        {
            arrow.transform.localScale = new Vector3(
                -arrow.transform.localScale.x,
                arrow.transform.localScale.y,
                arrow.transform.localScale.z
            );
        }

        // Calculate shoot direction and apply force
        Vector2 shootDir = (Vector2)(
            facingDir * loadedArrowRenderer.transform.right +
            loadedArrowRenderer.transform.up * 0.05f
        ).normalized;

        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.AddForce(shootDir * finalForce, ForceMode2D.Impulse);

        // Apply a small clockwise rotation to the arrow (positive torque)
        float rotationForce = -facingDir * 0.01f; // Small clockwise rotation, adjust as needed
        rb.AddTorque(rotationForce, ForceMode2D.Impulse);

        // Hide the visual arrow since it's now fired
        loadedArrowRenderer.enabled = false;

    }



    public void FinishLoad()
    {
        loaded = true;
    }
}
