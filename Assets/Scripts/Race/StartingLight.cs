using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingLight : MonoBehaviour
{
    [SerializeField] Renderer[] lightRenderers;
    [SerializeField] Material offMaterial;
    [SerializeField] Material readyMaterial;
    [SerializeField] Material goMaterial;
    [SerializeField] float countdownInterval = 1f;

    private void Start()
    {
        for (int i = 0; i < lightRenderers.Length; i++)
        {
            lightRenderers[i].material = offMaterial;
        }
        Invoke(nameof(StartCountdown), 2);
    }

    public void StartCountdown()
    {
        StartCoroutine(CountdownCoroutine());
    }

    IEnumerator CountdownCoroutine()
    {
        lightRenderers[0].material = readyMaterial;
        SoundManager.Instance.PlaySoundGlobal("StartLightReady");
        yield return new WaitForSeconds(countdownInterval);
        lightRenderers[1].material = readyMaterial;
        SoundManager.Instance.PlaySoundGlobal("StartLightReady");
        yield return new WaitForSeconds(countdownInterval);
        lightRenderers[2].material = readyMaterial;
        SoundManager.Instance.PlaySoundGlobal("StartLightReady");
        yield return new WaitForSeconds(countdownInterval);
        for (int i = 0; i < lightRenderers.Length; i++)
        {
            lightRenderers[i].material = goMaterial;
        }
        SoundManager.Instance.PlaySoundGlobal("StartLightGo");
    }
}
