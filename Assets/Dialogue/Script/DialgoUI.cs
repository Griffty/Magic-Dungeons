using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialgoUI : MonoBehaviour, IDisplayable
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;

    private ResponceHandler _responceHandler;
    private TypeWriterEffect _typeWriterEffect;
    private Player _player;

    private void Start()
    {
        _typeWriterEffect = GetComponent<TypeWriterEffect>();
        _responceHandler = GetComponent<ResponceHandler>();
        _player = FindObjectOfType<Player>();
        _player.playerEventHandler.CloseDialogue();
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        StartCoroutine(StopThroughDialogue(dialogueObject));
    }

    public void AddResponseEvents(ResponseEvent[] responseEvents)
    {
        _responceHandler.AddResponseEvents(responseEvents);
    }

    private IEnumerator StopThroughDialogue(DialogueObject dialogueObject)
    {
        for (int i = 0; i < dialogueObject.getDialogue().Length; i++)
        {
            string dialogue = dialogueObject.getDialogue()[i];
            yield return _typeWriterEffect.Run(dialogue, textLabel);
            textLabel.text = dialogue;
            if (i == dialogueObject.getDialogue().Length - 1 && dialogueObject.HasResponses)
            {
                break;
            }

            yield return null;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        if (dialogueObject.HasResponses)
        {
            _responceHandler.ShowResponses(dialogueObject.getResponces);
        }
        else
        {
            _player.playerEventHandler.CloseDialogue();
        }
    }

    public bool OpenDisplay()
    {
        if (dialogueBox.activeInHierarchy)
        {
            return false;
        }

        dialogueBox.SetActive(true);
        return true;
    }

    public bool CloseDisplay()
    {
        if (!dialogueBox.activeInHierarchy)
        {
            return false;
        }
        if (_typeWriterEffect.isRunning)
        {
            _typeWriterEffect.Stop();
        }
        _responceHandler.ClearResponse();
        textLabel.text = string.Empty;
        dialogueBox.SetActive(false);
        return true;
    }
}
