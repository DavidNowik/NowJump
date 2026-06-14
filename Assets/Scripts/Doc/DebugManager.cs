using UnityEngine;

/// <summary>
/// Global debug manager that tracks whether debug mode is enabled.
/// Debug mode can only be toggled through a developer hotkey and
/// provides a visual indicator of its current state.
/// </summary>
public class DebugManager : MonoBehaviour
{
    public static DebugManager Instance { get; private set; }

    /// <summary>
    /// Whether debug mode is currently active.
    /// </summary>
    public bool DebugEnabled => debugEnabled;

    [Header("Debug")]
    [SerializeField] private bool debugEnabled;

    [Header("Visual Indicator")]
    [SerializeField] private GameObject debugIndicator;

    #region Unity Events

    /// <summary>
    /// Initializes the singleton instance and ensures only one
    /// DebugManager exists in the scene.
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        UpdateIndicator();
    }

    /// <summary>
    /// Listens for the debug toggle hotkey.
    /// </summary>
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) &&
            Input.GetKeyDown(KeyCode.D))
        {
            ToggleDebug();
        }
    }

    #endregion

    #region Private Helpers

    /// <summary>
    /// Toggles the current debug mode state.
    /// </summary>
    private void ToggleDebug()
    {
        debugEnabled = !debugEnabled;
        UpdateIndicator();

        Debug.Log($"Debug Mode: {(debugEnabled ? "ON" : "OFF")}");
    }

    /// <summary>
    /// Updates the visual debug indicator to reflect the current state.
    /// </summary>
    private void UpdateIndicator()
    {
        if (debugIndicator != null)
        {
            debugIndicator.SetActive(debugEnabled);
        }
    }

    #endregion
}