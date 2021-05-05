using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PreLoader : MonoBehaviour
{
    // The name of the next scene
    public string nextSceneName;

    // For fading in and out of the scene
    public CanvasGroup fadeGroup;
    // The time it took to load
    private float loadTime = 0;
    // Minimum time of the scene
    public float minimumLogoTime = 3.0f;
    // The speed at which the scene fades in and out
    public float fadeSpeedMultiplier = 1f;

    // The text with the loading prompts
    public Text loadingText;

    // An async operation used to preload the next scene
    private AsyncOperation async;

    private IEnumerator Start()
    {
        // Start with a coloured view
        fadeGroup.alpha = 1;

        // Pre load the game

        // preload the next scene
        async = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Single);
        async.allowSceneActivation = false;
        
        // While the scene is loading display the progress
        while (async.progress < 0.9f)
        {
            loadingText.text = "Loading scenes " + (async.progress * 100) + "%";
            yield return null;
        }

        // Wait for an instance of save manager to be defined
        loadingText.text = "Loading save manager";
        while(SaveManager.Instance == null)
        {
            yield return null;
        }

        // Load the save file
        loadingText.text = "Loading save file";
        SaveManager.Instance.Load();

        // Get a timestamp of the completion time
        // if loadtime is fast, give it a buffer time so that we can see the logo
        if (Time.time < minimumLogoTime)
            loadTime = minimumLogoTime;
        else
            loadTime = Time.time;
    }	

    private void Update()
    {
        // Fade in
        if (Time.time < minimumLogoTime)
        {
            fadeGroup.alpha = 1 - Time.time * fadeSpeedMultiplier;
        }

        // Fade out
        if (Time.time > minimumLogoTime && loadTime != 0)
        {
            fadeGroup.alpha = (Time.time - minimumLogoTime) * fadeSpeedMultiplier;
            loadingText.text = "Done";
            if (fadeGroup.alpha >= 1)
            {
                // Switch to the next scene
                async.allowSceneActivation = true;
            }
        }
    }
}
