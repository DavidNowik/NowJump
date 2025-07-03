using System.Collections;
using UnityEngine;

public class Item_Fruit : MonoBehaviour
{
    [SerializeField] private float power = 1f;
    private void Awake()
    {
        if (name.StartsWith("Banane"))
        {
            if (PlayerPrefs.GetInt("WallJump") > 0)
                gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            GetComponent<Animator>().SetTrigger("pick_up");
            Destroy(GetComponent<BoxCollider2D>());

            if (name.StartsWith("Apple"))
            {
                player.IncreaseSpeed(power);
                StartCoroutine(RemoveAppleBuffAfterTime(player, power, 5f));
                if (GameManager.instance.canvasManager != null)
                    GameManager.instance.canvasManager.appleText.text
                        = "Apples: " + player.GetActiveSpeedBuff();
            }
            if (name.StartsWith("Kiwi"))
            {
                player.IncreaseJump(power);
                Debug.Log("Increase Jump");
                StartCoroutine(RemoveKiwiBuffAfterTime(player, power, 5f));
                if (GameManager.instance.canvasManager != null)
                GameManager.instance.canvasManager.kiwiText.text
                        = "Kiwi: " + player.GetActiveJumpBuff();
            }
            if (name.StartsWith("Banana"))
            {
                PlayerPrefs.SetInt("WallJump", 1);
                player.CheckPrefs();
                Debug.Log("Activate WallJump");
            }
            if (name.StartsWith("Star"))
            {
                GameManager.instance.canvasManager.IncreaseStars();
                Debug.Log("Collect Star");
                if (transform.childCount > 0)
                {
                    Transform particleTransform = transform.GetChild(0);
                    ParticleSystem ps = particleTransform.GetComponent<ParticleSystem>();

                    if (ps != null)
                    {
                        var main = ps.main;
                        var emission = ps.emission;
                        var shape = ps.shape;

                        // Exaggerate everything
                        main.startSpeed = 10f;
                        main.startSize = 1.5f;
                        main.startLifetime = 1.5f;
                        main.simulationSpeed = 2f;

                        emission.rateOverTime = 100f;
                        emission.rateOverDistance = 50f;

                        shape.radius = 0.5f;
                        shape.angle = 25f;

                        ps.Play();
                    }
                }//Lass das ParticleSystem ausrasten

            }

            if (name.StartsWith("Pineapple"))
            {
                PlayerPrefs.SetInt("DashJump", 1);
                player.CheckPrefs();
                Debug.Log("Activate DashJump");
            }
            if (name.StartsWith("Cherry"))
            {
                player.IncreaseJumpCount((int)power);
                if (GameManager.instance.canvasManager != null)
                    GameManager.instance.canvasManager.jumpsText.text = "Jumps: "+player.GetJumpCount();
                Debug.Log("Increase JumpCount");
                StartCoroutine(RemoveCherryBuffAfterTime(player, (int)power, 6f));
            }

        }
    }

    private IEnumerator RemoveAppleBuffAfterTime(Player player, float amount, float delay)
    {
        yield return new WaitForSeconds(delay);
        player.DecreaseSpeed(amount);
        GameManager.instance.canvasManager.appleText.text
                    = "Apples: " + player.GetActiveSpeedBuff();
        DeleteSelf();
    }
    private IEnumerator RemoveKiwiBuffAfterTime(Player player, float amount, float delay)
    {
        yield return new WaitForSeconds(delay);
        player.DecreaseJump(amount);
        GameManager.instance.canvasManager.kiwiText.text
            = "Kiwi: " + player.GetActiveJumpBuff();
        DeleteSelf();
    }
    private IEnumerator RemoveCherryBuffAfterTime(Player player, int amount, float delay)
    {
        yield return new WaitForSeconds(delay);
        player.DecreaseJumpCount(amount);
        if (GameManager.instance.canvasManager != null)
            GameManager.instance.canvasManager.jumpsText.text = "Jumps: " + player.GetJumpCount();
        DeleteSelf();
    }

    public void DeleteSelf()
    {
        Destroy(transform.parent.gameObject, 0.1f);
    }

}
