using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResponceHandler : MonoBehaviour
{
    [SerializeField] private RectTransform responseBox;
    [SerializeField] private RectTransform responseButtonTemplate;
    [SerializeField] private RectTransform responseContainer;


    private DialgoUI _dialgoUI;
    private Player _player;

    private List<GameObject> tempRespButton = new();
    private ResponseEvent[] _responseEvents;

    private void Start()
    {
        _dialgoUI = GetComponent<DialgoUI>();
        _player = FindObjectOfType<Player>();
    }

    public void AddResponseEvents(ResponseEvent[] responseEvents)
    {
        _responseEvents = responseEvents;
    }

    public void ClearResponse()
    {
        responseBox.gameObject.SetActive(false);
        foreach (GameObject o in tempRespButton)
        {
            Destroy(o);
        }
        tempRespButton.Clear();
        _responseEvents = null;
    }
    public void ShowResponses(Response[] responses)
    {
        float responsesBoxHeight = 0;
        for (int i = 0; i < responses.Length; i++)
        {
            Response response = responses[i];
            int responsesIndex = i;
            
            GameObject responseButton = Instantiate(responseButtonTemplate.gameObject, responseContainer);
            responseButton.gameObject.SetActive(true);
            responseButton.GetComponent<TMP_Text>().text = response.getResponse;
            responseButton.GetComponent<Button>().onClick.AddListener(() => onPickedResponce(response, responsesIndex));
            tempRespButton.Add(responseButton);
            
            responsesBoxHeight += responseButtonTemplate.sizeDelta.y;
        }

        responseBox.sizeDelta = new Vector2(responseBox.sizeDelta.x, responsesBoxHeight);
        responseBox.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(tempRespButton[0]);
    }

    private void onPickedResponce(Response response, int responsesIndex)
    {
        responseBox.gameObject.SetActive(false);
        foreach (GameObject o in tempRespButton)
        {
            Destroy(o);
        }
        tempRespButton.Clear();

        if (_responseEvents !=null && responsesIndex <= _responseEvents.Length)
        {
            _responseEvents[responsesIndex].getOnPickResponse?.Invoke();
        }

        _responseEvents = null;
        if (response.GetDialogueObject)
        {
            _dialgoUI.ShowDialogue(response.GetDialogueObject);
        }
        else
        {
            _player.playerEventHandler.CloseDialogue();
        }
        
    }
}
