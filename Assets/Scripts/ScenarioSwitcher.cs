using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor.Callbacks;

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
    private Renderer screen;

    private bool _isLoading = false;
    private bool _isFading = false;

    private Scenario _scenario;
    public Scenario currentScenario {
        get { return _scenario; }
        private set { _scenario = value; }
    }
    

    public void SwitchScenario(string name) {
        Scenario nextScenario = GetScenario(name);

        // Lock interactable objects
        LockInteractables();

        // Fade in screen
        _isLoading = true;
        StartCoroutine("FadeScreenCoroutine");

        // Call all scenario switches

        // Switch lighting scenario
        GetComponent<LevelLightmapData>().LoadLightingScenario(nextScenario.lightingScenario);
        Camera.main.GetComponent<Volume>().profile = nextScenario.postProcessProfile;

        // Fade out screen
        _isLoading = false;

        // Unlock interactable objects
        StartCoroutine("UnlockInteractablesCoroutine");
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
            screen.material.color = Color.Lerp(a, b, 1f - (timer / fadeInTime));
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
            screen.material.color = Color.Lerp(b, a, 1f - (timer / fadeOutTime));
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
    [PostProcessScene(0)]
    public static void OnPostProcessScene() {
        ScenarioSwitcher manager;
        
        try {
            manager = GameObject.Find("GameManager").GetComponent<ScenarioSwitcher>();
        } catch {
            Debug.Log("No ScenarioSwitcher in scene, skipping");
            return;
        }

        // Add FadeScreen to camera
        GameObject screen = new GameObject("FadeScreen");
        screen.transform.parent = Camera.main.transform;
        screen.transform.localPosition = new Vector3(0f, 0f, 0.5f);
        screen.transform.localScale = new Vector3(100f, 100f, 1f);

        MeshFilter screenMesh = screen.AddComponent<MeshFilter>();
        screenMesh.mesh = AssetDatabaseHelper.LoadAssetFromUniqueAssetPath<Mesh>("Library/unity default resources::Quad");

        MeshRenderer screenRenderer = screen.AddComponent<MeshRenderer>();
        screenRenderer.material = manager.screenMaterial;

        screen.SetActive(false);

        // Set FadeScreen in ScenarioSwitcher
        manager.screen = screenRenderer;
    }
    
}
