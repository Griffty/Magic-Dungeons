using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class PlayerEventHandler : MonoBehaviour
{
    public bool hasOpenMenu;
    public string menuName;
    private Object _openMenu;
    private PlayerInventoryHandler _inventoryHandler;
    private PlayerQuestManager _questManager;
    private PlayerMagicHandler _magicHandler;
    private MovementHandler _movementHandler;
    private TradeManager _tradeManager;
    private DialgoUI _dialogUI;
    private Player _player;
    
    public IInteractable Interactable { get; set; }

    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider manaBar;

    private void Awake()
    {
        HealthHandler.OnPlayerDamageTake += UpdateHealthBar;
        _dialogUI = FindObjectOfType<DialgoUI>();
        _tradeManager = FindObjectOfType<TradeManager>();
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        _inventoryHandler = _player.playerInventoryHandler;
        _magicHandler = _player.playerMagicHandler;
        _movementHandler = _player.movementHandler;
        _questManager = _player.playerQuestManager;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tilde))
        {
            Debug.Log(Room.GetRoomByPos(new Vector2Int((int)_player.transform.position.x, (int)_player.transform.position.y)));
            Debug.Log(Interactable);
        }
        if (_openMenu == null)
        {
            menuName = "null";
        }
        else
        {
            if (_openMenu.GetType() == _inventoryHandler.GetType())
            {
                menuName = "inv";
            }
            if (_openMenu.GetType() == _magicHandler.GetType())
            {
                menuName = "mag";
            }
            if (_openMenu.GetType() == _dialogUI.GetType())
            {
                menuName = "dia";
            }
            if (_openMenu.GetType() == _questManager.GetType())
            {
                menuName = "qst";
            }
            if (_openMenu.GetType() == _tradeManager.GetType())
            {
                menuName = "trd";
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (hasOpenMenu)
            {
                Close(_magicHandler);
            }
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (hasOpenMenu)
            {
                Close(_inventoryHandler);
            }
            else
            {
                Open(_inventoryHandler); 
            }
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (hasOpenMenu)
            {
                Close(_questManager);
            }
            else
            {
                Open(_questManager); 
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!hasOpenMenu)
            {
                Interact();
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (hasOpenMenu)
            {
                Close(_magicHandler);
            }
            else
            {
                Open(_magicHandler); 
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (!hasOpenMenu)
            {
                _magicHandler.CastSpell();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (hasOpenMenu)
            {
                if (_openMenu != _dialogUI)
                {
                    Close(_openMenu);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            Time.timeScale = 0.1f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }
    }

    private void FixedUpdate()
    {
        if (hasOpenMenu)
        {
            if (menuName is "inv" or "dia" or "qst" or "trd")
            {
                _player.rb.velocity = Vector2.zero;
                return;
            }
        }
        _movementHandler.Move();
    }

    private void Open(Object obj)
    {
        if (!hasOpenMenu)
        {
            
            if (((IDisplayable)obj).OpenDisplay())
            {
                _openMenu = obj;
                hasOpenMenu = true;
            }
        }
    }

    private void Close(Object obj)
    {
        if (_openMenu.Equals(obj))
        {
            if (((IDisplayable)obj).CloseDisplay())
            {
                _openMenu = null;
                hasOpenMenu = false;
            }
        }
    }

    private void Interact()
    {
        if (Interactable != null)
        {
            Interactable.Interact(_player);
        }
    }

    public void AddResponseEvents(ResponseEvent[] responseEventsEvents)
    {
        _dialogUI.AddResponseEvents(responseEventsEvents);
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        if (_dialogUI.OpenDisplay())
        {
            hasOpenMenu = true;
            _openMenu = _dialogUI;
            _dialogUI.ShowDialogue(dialogueObject);
        }
    }

    public void StartTrade(TradeObject tradeObject)
    {
        StartCoroutine(WaitForDialogueEnd(tradeObject));
    }

    private IEnumerator WaitForDialogueEnd(TradeObject tradeObject)
    {
        yield return new WaitUntil(() => !hasOpenMenu);
        Open(_tradeManager);
        _tradeManager.ShowTrade(tradeObject);
    }
    public void CloseDialogue()
    {
        if (hasOpenMenu)
        {
            if (_openMenu == _dialogUI)
            {
                Close(_openMenu);
            }
        }
    }

    private void UpdateHealthBar()
    {
        healthBar.value = _player.healthHandler.healthPoints / _player.healthHandler.maxHealth;
    }

}

[Serializable]
public struct KeyBind
{
    public KeyCode keyCode;
    public string keyBindName;

    public void SetKeyCode()
    {
        // Event.current.keyCode;
    }
}