using System.Collections;
using UnityEngine;

public class StoneAbbau : MonoBehaviour
{
    private bool abbauing = false;
    private bool automine = false;
    public static int stonesCollected = -1;

    private Coroutine abbauCoroutine;
    private Coroutine automineCoroutine;

    private bool automineWasActive = false; // track previous automine state

    void Update()
    {
        // Check if automine was just enabled
        if (automine && !automineWasActive)
        {
            automineCoroutine = StartCoroutine(AutoMineStones());
            automineWasActive = true;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            Debug.Log("Is currently abbauing");
            abbauing = true;

            if (abbauCoroutine == null)
                abbauCoroutine = StartCoroutine(CollectStones());

            if (stonesCollected == 0)
            {
                Debug.Log("OK aber bei vwie viel= " + stonesCollected);
                GameManager.instance.CreateTextArray(new string[]
                {
                    "Du baust jetzt Stein ab.",
                    "Mit Stein kannst du Gebäude und Platformen bauen.",
                });
            }
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null && player.carryingEquipment)
        {
            automine = true;
            transform.parent.GetChild(1).gameObject.SetActive(true);
            Destroy(GameObject.Find("CarriedEquipment"));
        }

        if (player != null)
        {
            abbauing = false;

            if (abbauCoroutine != null)
            {
                StopCoroutine(abbauCoroutine);
                abbauCoroutine = null;
            }
        }
    }

    private IEnumerator CollectStones()
    {
        while (abbauing)
        {
            stonesCollected++;
            UpdateStoneText();
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator AutoMineStones()
    {
        while (automine)
        {
            stonesCollected++;
            UpdateStoneText();
            yield return new WaitForSeconds(1f);
        }

        automineCoroutine = null;
        automineWasActive = false;
    }

    private void UpdateStoneText()
    {
        transform.parent.GetChild(2).GetComponent<TextMesh>().text = stonesCollected.ToString();
    }
}
