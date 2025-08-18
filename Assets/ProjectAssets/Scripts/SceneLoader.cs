using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float blackScreenDuration = 0.5f; 

    private static bool shouldFadeIn = false;
    public static bool isManualTutorialLoad = false;

    private void Awake()
    {
        if (shouldFadeIn)
        {
            fadeImage.color = Color.black;
            StartCoroutine(FadeIn());
            shouldFadeIn = false;
        }
        else
        {
            fadeImage.color = new Color(0, 0, 0, 0);
        }
    }

    public void LoadTutorialManually(string sceneName)
    {
        isManualTutorialLoad = true;
        LoadScene(sceneName);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneWithFade(sceneName));
    }

    public void LoadSceneWithoutFade(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator LoadSceneWithFade(string sceneName)
    {
        yield return StartCoroutine(FadeOut());
        yield return new WaitForSeconds(blackScreenDuration); 
        shouldFadeIn = true;
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator FadeOut()
    {
        float timer = 0;
        while (timer <= fadeDuration)
        {
            fadeImage.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, timer / fadeDuration));
            timer += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator FadeIn()
    {
        float timer = 0;
        while (timer <= fadeDuration)
        {
            fadeImage.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, timer / fadeDuration));
            timer += Time.deltaTime;
            yield return null;
        }
    }

    public void QuitGame()
    {
        SaveManager.Instance.SaveGame();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}