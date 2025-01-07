using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Custom/DialogueObject")]
public class DialogueObject: ScriptableObject
{
    [SerializeField] [TextArea] private string[] Dialogue;
    [SerializeField] private Response[] _responses;

    public bool HasResponses => _responses != null && _responses.Length > 0;

    public string[] getDialogue() => Dialogue;
    public Response[] getResponces => _responses;
}
