using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class Response
{
    [SerializeField] private string responseText;
    [SerializeField] private DialogueObject dialogueObject;

    public string getResponse => responseText;

    public DialogueObject GetDialogueObject => dialogueObject;
}
