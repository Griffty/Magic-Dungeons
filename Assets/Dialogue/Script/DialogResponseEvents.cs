using System;
using UnityEngine;
using UnityEngine.Serialization;

public class DialogResponseEvents : MonoBehaviour
{
    [FormerlySerializedAs("_dialogObject")] [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private ResponseEvent[] _events;

    public DialogueObject DialogueObject => dialogueObject;
    
    public ResponseEvent[] Events => _events;

    public void OnValidate()
    {
        if (dialogueObject == null)
        {
            return;
        }

        if (dialogueObject.getResponces == null)
        {
            return;
        }

        if (_events != null && _events.Length == dialogueObject.getResponces.Length)
        {
            return;
        }

        if (_events == null)
        {
            _events = new ResponseEvent[dialogueObject.getResponces.Length];
        }
        else
        {
            Array.Resize(ref _events, dialogueObject.getResponces.Length);
        }

        for (int i = 0; i < dialogueObject.getResponces.Length; i++)
        {
            Response response = dialogueObject.getResponces[i];

            if (_events[i] != null)
            {
                _events[i].name = response.getResponse;
                continue;
            }

            _events[i] = new ResponseEvent() { name = response.getResponse };
        }
    }
}
