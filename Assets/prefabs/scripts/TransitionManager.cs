using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public GameObject _background;
    public GameObject _levelLoadingScreen;

    private GameObject blackscreen;
    public GameObject Exit;
    private Animator _animatorCamera;
    private Animator _animatorFade;
    private CinemachineVirtualCamera _camera;
    private Transform _player;

    private LevelManager _levelManager;

    public static TransitionManager instance;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        if (_levelManager == null)
        {
            _levelLoadingScreen = GameObject.Find("LevelLoadingScreen");
        }

        _camera = FindObjectOfType<CinemachineVirtualCamera>();
        _background = _levelLoadingScreen.transform.GetChild(0).gameObject;
        blackscreen = _levelLoadingScreen.transform.GetChild(1).gameObject;

        _player = _camera.m_Follow;

        _animatorFade = _background.GetComponent<Animator>();
        _animatorCamera = _camera.GetComponent<Animator>();

        _levelManager = FindObjectOfType<LevelManager>();

        blackscreen.SetActive(false);
    }

    public void InTransition()
    {
        StartCoroutine(RoutineInTransition());

    }


    public void OutTransition()
    {
        StartCoroutine(RoutineOutTransition());

    }


    IEnumerator RoutineInTransition()
    {
        _camera.Follow = _player;
        _animatorFade.SetTrigger("loaded");
        _animatorCamera.SetTrigger("loaded");

        yield return new WaitForSeconds(2f);

        _levelLoadingScreen.SetActive(false);
    }
    IEnumerator RoutineOutTransition()
    {
        _levelLoadingScreen.SetActive(true);

        _camera.Follow = Exit.transform;
        _animatorFade.SetTrigger("transition");
        _animatorCamera.SetTrigger("transition");

        yield return new WaitForSeconds(2f);
        
        blackscreen.SetActive(true);
        NextLevel();
    }


    public void NextLevel()
    {

        if (SceneManager.GetActiveScene().buildIndex != _levelManager.HubIndex)
        {
            _levelManager.LoadHub();
        }
        else
        {
            _levelManager.LoadLevel(_levelManager.gameData.levelToLoad());
        }

    }
}
