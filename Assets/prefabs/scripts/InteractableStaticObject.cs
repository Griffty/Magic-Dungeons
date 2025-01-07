using UnityEngine;

public abstract class InteractableStaticObject : MonoBehaviour, IInteractable
{
    protected InteractableStaticObject interactableStaticObject;

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
            if (ReferenceEquals(player.playerEventHandler.Interactable, interactableStaticObject))
            {
                player.playerEventHandler.Interactable = null;
            }
        }
    }

    public abstract void Interact(Player player);
}
