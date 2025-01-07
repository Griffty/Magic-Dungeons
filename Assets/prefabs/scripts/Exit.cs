using Cinemachine;
using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections;

public class Exit : InteractableStaticObject
{
    private TransitionManager _transitionManager;
    public Room _room;
    public LevelManager _levelmanager;

    private void Start()
    {
        interactableStaticObject = this;

        TransitionManager.instance.Exit = gameObject;

        _transitionManager= FindObjectOfType<TransitionManager>();
        _levelmanager = FindObjectOfType<LevelManager>();

    }

    public override void Interact(Player player)
    {
        if (SceneManager.GetActiveScene().buildIndex == _levelmanager.HubIndex || _room is { IsCleared: true })
        {
            _transitionManager.OutTransition();
        }
    }
}
