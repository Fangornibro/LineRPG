using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    private static SceneTransition instance;
    private static bool shouldPlayOpening = false;
    private AsyncOperation loadingSceneOperation;

    public TextMeshProUGUI loadingText;
    public Image loadingImage;

    public static bool inAnimation = false;

    private Animator anim;
    public static void SwitchToScene(string sceneName)
    {
        if (!inAnimation)
        {
            inAnimation = true;
            instance.anim.SetTrigger("sceneClosing");

            instance.loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);
            instance.loadingSceneOperation.allowSceneActivation = false;
        }
    }
    void Start()
    {
        instance= this;
        anim= GetComponent<Animator>();
        if (shouldPlayOpening)
        {
            anim.SetTrigger("sceneOpening");
        }
    }
    private void Update()
    {
        if (loadingSceneOperation != null)
        {
            loadingText.text = Mathf.RoundToInt(loadingSceneOperation.progress * 100) + "%";
            loadingImage.fillAmount = loadingSceneOperation.progress;
        }
        
    }
    public void OnAnimationOver() 
    {
        inAnimation = false;
        shouldPlayOpening = true;
        instance.loadingSceneOperation.allowSceneActivation = true;
    }
}
