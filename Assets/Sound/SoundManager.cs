using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio")]
    public AudioClip musicClip;
    private AudioSource audioSource;

    [Header("UI")]
    public Slider volumeSlider;

    private const string VolumePrefKey = "MasterVolume";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = musicClip;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, 0.1f);
        audioSource.volume = savedVolume;

        // Starte Musik
        audioSource.Play();

        // Slider initialisieren
        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    public void SetVolume(float value)
    {
        audioSource.volume = value;
        if(value < 0.5f)
        {
            audioSource.volume = value/2;
        }

        // Speichere Wert sofort
        PlayerPrefs.SetFloat(VolumePrefKey, value);
        PlayerPrefs.Save();
    }
}
