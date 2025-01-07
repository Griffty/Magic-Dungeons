using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject levelLoadingScreen;
    [SerializeField] public GameData gameData;
    [SerializeField] private TransitionManager TransitionManager;

    [SerializeField] private int hubIndex;
    public int HubIndex
    {
        get => hubIndex;
        private set => hubIndex = value;
    }

    private void Start()
    {
        TransitionManager = GetComponentInChildren<TransitionManager>();

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (SceneManager.GetActiveScene().buildIndex != gameData.currentScene)
            {
                if (gameData.currentScene == 16)
                {
                    LoadHub();
                }
                else
                {
                    LoadLevel(gameData.currentScene);
                }
            }
        }

        
    }

    public void LoadHub()
    {
        StartCoroutine(LoadLevelAsynchronously(hubIndex));
        gameData.currentScene = hubIndex;
    }
    public void LoadLevel(int levelIndex)
    {
        StartCoroutine(LoadLevelAsynchronously(levelIndex));
        gameData.currentScene = levelIndex;
        gameData.lastLevel = levelIndex;
    }
    
    public void Exit()
    {
        Application.Quit();
    }
    
    private IEnumerator LoadLevelAsynchronously(int levelIndex)
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync(levelIndex);

        yield return null;
    }
    
    private IEnumerator LoadLevelAsynchronously(String levelName)
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync(levelName);

        yield return null;
    }

}