using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

#if UNITY_EDITOR
using UnityEditor.Callbacks;
#endif

[Serializable]
public struct Scenario {
    public string name;
    public VolumeProfile postProcessProfile;
    public string lightingScenario;
    public string backgroundMusic;
    public string fadeInSound;
    public string fadeWaitSound;
    public string fadeOutSound;
    public Color fadeInColor;
}

public class ScenarioSwitcher : MonoBehaviour
{
    public List<Scenario> scenarioList;

    [Header("Switch settings")]
    public float fadeInTime = 0.5f;
    public float fadeWaitTime = 0.5f;
    public float fadeOutTime = 2f;


    [Header("Internals")]
    public Material screenMaterial;
    private UnityEngine.UI.Image screen;

    private bool _isLoading = false;
    private bool _isFading = false;

    private Scenario _scenario;
    public Scenario currentScenario {
        get { return _scenario; }
        private set { _scenario = value; }
    }
    

    public void SwitchScenario(string name, bool animateFade = true) {
        try {
            Debug.Log("Loading scenario: " + name);
            Scenario nextScenario = GetScenario(name);

            // Lock interactable objects
            LockInteractables();

            // Fade in screen
            if(animateFade) {
                _isLoading = true;
                StartCoroutine("FadeScreenCoroutine");
            }

            // Call all scenario switches

            // Switch lighting scenario
            GetComponent<LevelLightmapData>().LoadLightingScenario(nextScenario.lightingScenario);
            Debug.Log("Loaded light scenario: " + nextScenario.lightingScenario);
            Camera.main.GetComponent<Volume>().profile = nextScenario.postProcessProfile;
            Debug.Log("Loaded profile: " + nextScenario.postProcessProfile.name);

            // Fade out screen
            _isLoading = false;
            if(!animateFade) _isFading = false;

            // Unlock interactable objects
            StartCoroutine("UnlockInteractablesCoroutine");
        } catch (Exception e) {
            Debug.LogError("Failed to load scenario");
            Debug.LogError(e);
        }
    }

    public Scenario GetScenario(string name) {
        for(int i = 0; i < scenarioList.Count; i++) {
            if(scenarioList[i].name.Equals(name)) {
                return scenarioList[i];
            }
        }

        throw new ArgumentOutOfRangeException("Specified scenario does not exist");
    }

    private void LockInteractables() {}

    private void UnlockInteractables() {}

    private IEnumerator FadeScreenCoroutine() {
        float timer;

        Color b = currentScenario.fadeInColor;
        Color a = new Color(b.r, b.g, b.b, 0f);

        // Fade in
        _isFading = true;
        timer = fadeInTime;
        screen.gameObject.SetActive(true);
        while(timer > 0f) {
            screen.color = Color.Lerp(a, b, 1f - (timer / fadeInTime));
            yield return null; //new WaitForEndOfFrame();
        }

        // Wait
        yield return new WaitForSeconds(fadeWaitTime);
        while(_isLoading) {
            yield return new WaitForSeconds(0.1f);
        }

        // Fade out
        timer = fadeOutTime;
        while(timer > 0f) {
            screen.color = Color.Lerp(b, a, 1f - (timer / fadeOutTime));
            yield return null; //new WaitForEndOfFrame();
        }
        screen.gameObject.SetActive(false);
        _isFading = false;
    }

    private IEnumerator UnlockInteractablesCoroutine() {
        while(_isFading) {
            yield return new WaitForSeconds(0.1f);
        }

        UnlockInteractables();
    }

    /*
     *  Build-time optimization
     */
    #if UNITY_EDITOR
    [PostProcessScene(0)]
    public static void OnPostProcessScene() {
        ScenarioSwitcher manager;
        
        try {
            manager = GameObject.Find("GameManager").GetComponent<ScenarioSwitcher>();
        } catch {
            Debug.Log("No ScenarioSwitcher in scene, skipping");
            return;
        }
    }
    #endif
}
