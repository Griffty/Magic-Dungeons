using System;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Sprite doorTopLeft, doorTopRight, doorBottomLeft, doorBottomRight, doorLeftLeft, doorLeftRight, doorRightLeft, doorRightRight;
    [SerializeField] private Sprite doorTopOpened, doorRightOpened, doorBottomOpened, doorLeftOpened;
    [SerializeField] private RoomManager roomManager;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _collider2D;
    private int _state;
    private bool _opened;
    private Sprite _sprite;
    
    private Room _room;
    public void Setup(Room room, int dState)
    {
        roomManager = FindObjectOfType<RoomManager>();
        _room = room;
        _state = dState;
        
        SetupSprite();
        SetupCollider();
        SetOpen();
    }

    public void SetOpen()
    {
        ChangeDoorState(true);
    }

    public void SetClose()
    {
        ChangeDoorState(false);
    }
    

    public void ChangeDoorState(bool open)
    {
        _opened = open;
        _collider2D.isTrigger = open;
        UpdateSprite();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
        {
            foreach (var door in Room.Doors)
            {
                if (door.GetComponent<Collider2D>() == null)
                {
                    throw new Exception("MotherFuckerWTF");
                }
                if (other.IsTouching(door.GetComponent<Collider2D>()))
                {
                    return;
                }
            }
            if (IsACloser(new Vector2(player.transform.position.x, player.transform.position.y), new Vector2(transform.position.x, transform.position.y), new Vector2(_room.RoomCenter.x, _room.RoomCenter.y)))
            {
                roomManager.OnEnteringRoom(_room);
            }
            else
            {
                roomManager.OnExitingRoom(_room);
            }
        }
    }

    private bool IsACloser(Vector2 a, Vector2 b, Vector2 c)
    {
        double dA = Math.Sqrt((a.x - c.x) * (a.x - c.x) + (a.y - c.y) * (a.y - c.y));
        double dB = Math.Sqrt((b.x - c.x) * (b.x - c.x) + (b.y - c.y) * (b.y - c.y));

        return dA < dB;
    }

    private void UpdateSprite()
    {
        if (_opened)
        {
            int s = (_state > 3) ? _state - 4: _state;
            switch (s)
            {
                case 0: _sprite = doorTopOpened;
                    break;
                case 1: _sprite = doorRightOpened;
                    break;
                case 2: _sprite = doorBottomOpened;
                    break;
                case 3: _sprite = doorLeftOpened;
                    break;
            }
        }
        else
        {
            switch (_state)
            {
                case 0: _sprite = doorTopLeft;
                    break;
                case 1: _sprite = doorRightLeft;
                    break;
                case 2: _sprite = doorBottomLeft;
                    break;
                case 3: _sprite = doorLeftLeft;
                    break;
                case 4: _sprite = doorTopRight;
                    break;
                case 5: _sprite = doorRightRight;
                    break;
                case 6: _sprite = doorBottomRight;
                    break;
                case 7: _sprite = doorLeftRight;
                    break;
                default:
                    Debug.Log("WTF");
                    Debug.Log(_state);
                    break;
            }
        }

        _spriteRenderer.sprite = _sprite;
    }
    
    private void SetupSprite()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            throw new Exception("No render");
        }
        UpdateSprite();
    }
    
    private void SetupCollider()
    {
        _collider2D = GetComponent<BoxCollider2D>();
        if (_collider2D == null)
        {
            Debug.Log("No collision");
        }
        
        switch (_state % 4)
        {
            case 0: case 2: // Up, Down
                _collider2D.size = new Vector2(1, 1);
                _collider2D.offset = new Vector2(0, 0);
                break;
            case 1: // Right
                _collider2D.size = new Vector2(0.375f, 1);
                _collider2D.offset = new Vector2(-0.313f, 0);
                break;
            case 3: // Left
                _collider2D.size = new Vector2(0.375f, 1);
                _collider2D.offset = new Vector2(0.313f, 0);
                break;
        }
    }
}
