using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ScreenFading : MonoBehaviour
{
    private DepthOfField dof;
    [SerializeField] private Volume volume;
    [SerializeField] private GameManager uiManager;
    void Start()
    {
        volume.profile.TryGet(out dof);
    }
    public void StartFadingCoroutine(float startValue, float maxValue)
    {
        StartCoroutine(Fading(startValue, maxValue));
    }
    private IEnumerator Fading(float startValue, float maxValue)
    {
        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            dof.focalLength.value = Mathf.Lerp(startValue, maxValue, i);
            yield return null;
        }
        dof.focalLength.value = 1;
        uiManager.FocusPanelVisible(true);
        uiManager.fightManager.BackToMap();
    }
}
