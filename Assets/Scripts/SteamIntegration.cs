using UnityEngine;
using Steamworks;

public class SteamIntegration : MonoBehaviour
{
    private bool steamInitialized = false;

    private void Start()
    {
        try
        {
            // Init Steamworks
            steamInitialized = SteamAPI.Init();
            if (steamInitialized)
            {
                Debug.Log("✅ Steam initialized as " + SteamFriends.GetPersonaName());
            }
            else
            {
                Debug.LogError("❌ Steam init failed!");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("❌ Steam init exception: " + e);
        }
    }

    private void Update()
    {
        // Steamworks.NET benötigt Update pro Frame
        if (steamInitialized)
            SteamAPI.RunCallbacks();
    }

    private void OnApplicationQuit()
    {
        if (steamInitialized)
            SteamAPI.Shutdown();
    }

    // Beispiel: Achievement setzen
    public void UnlockAchievement(string achievementId)
    {
        if (!steamInitialized) return;

        bool achieved;
        SteamUserStats.GetAchievement(achievementId, out achieved);
        if (!achieved)
        {
            SteamUserStats.SetAchievement(achievementId);
            SteamUserStats.StoreStats(); // speichern
            Debug.Log($"Achievement unlocked: {achievementId}");
        }
    }
}
