using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelReview : MonoBehaviour
{
    public int worldDisplayed = 1;
    public int levelDisplayed = 1;

    private TextMesh _levelText;
    private GameObject starDisplay1, starDisplay2, starDisplay3;

    private void Awake()
    {
        _init();
    }
    private void _init()
    {
        _levelText = transform.GetChild(1).GetComponent<TextMesh>();
        _levelText.text = "Level: " + levelDisplayed;
        starDisplay1 = transform.GetChild(2).GetChild(0).gameObject;
        starDisplay2 = transform.GetChild(3).GetChild(0).gameObject;
        starDisplay3 = transform.GetChild(4).GetChild(0).gameObject;

        transform.GetChild(5).GetComponent<Transition>().travelOverride = 
            worldDisplayed + "_" + levelDisplayed;
        // Turn off all animations initially
        starDisplay1.GetComponent<Animator>().SetBool("isActive", false);
        starDisplay2.GetComponent<Animator>().SetBool("isActive", false);
        starDisplay3.GetComponent<Animator>().SetBool("isActive", false);
    }

    public void ActivateDetails()
    {
        // Start staggered animation based on how many stars player has earned
        int stars = PlayerPrefs.GetInt("Level" + worldDisplayed +"_"+ levelDisplayed + "Stars");
        StartCoroutine(ActivateStars(stars));
    }
    private IEnumerator ActivateStars(int stars)
    {
        if (stars >= 1)
        {
            yield return new WaitForSeconds(0.1f);
            starDisplay1.GetComponent<Animator>().SetBool("isActive", true);
        }
        if (stars >= 2)
        {
            yield return new WaitForSeconds(0.2f); // slight additional delay
            starDisplay2.GetComponent<Animator>().SetBool("isActive", true);
        }
        if (stars >= 3)
        {
            yield return new WaitForSeconds(0.2f); // and again
            starDisplay3.GetComponent<Animator>().SetBool("isActive", true);
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        ActivateDetails();
    }
}
