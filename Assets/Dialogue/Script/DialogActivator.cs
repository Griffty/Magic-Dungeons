using System;
using UnityEngine;

public class DialogActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private GameObject dialogueMarkPref;
    private void Start()
    {
        var tr = transform;
        Vector3 pos = tr.position;
        pos.y += 1.5f;
        Instantiate(dialogueMarkPref, pos, tr.rotation, tr);
    }

    public void UpdateDialogueObject(DialogueObject dialogueObject)
    {
        this.dialogueObject = dialogueObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
        {
            player.playerEventHandler.Interactable = this;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
        {
            if (player.playerEventHandler.Interactable is DialogActivator dialogActivator && dialogActivator == this)
            {
                player.playerEventHandler.Interactable = null;
            }
        }
    }

    public void Interact(Player player)
    {
        foreach (DialogResponseEvents responseEvents in GetComponents<DialogResponseEvents>())
        {
            if (responseEvents.DialogueObject == dialogueObject)
            {
                player.playerEventHandler.AddResponseEvents(responseEvents.Events);
                break;
            }
        }
        player.playerEventHandler.ShowDialogue(dialogueObject);
    }
}
