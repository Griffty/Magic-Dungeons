using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMagicHandler : MonoBehaviour, IDisplayable
{
    [SerializeField] private GameObject drawingDisplay;
    private NewNetworkConfidenceDisplay _newNetworkConfidenceDisplay;
    private NewDrawingController _drawingController;
    private bool _spellOnCd;
    private bool _isDrawing;
    private bool _drawingOnCd;
    private Player _player;
    
    [SerializeField] private Spell selectedSpell;
    [SerializeField] private Transform spellParent;
    
    [SerializeField] private Slider manaBar;
    [SerializeField] private RectTransform minMana;
    [SerializeField] private UnityEngine.UI.Image spellImage;
    private void UpdateManaBar()
    {
        manaBar.value = selectedSpell.mana / selectedSpell.spellData.manaPool;
    }
    private void Awake()
    {
        _player = GetComponent<Player>();
        _player.playerData.resistanceData.RecalculateAllResistance();
        SetUpSpells();
    }

    private void SetUpSpells()
    {
        foreach (Transform spellChild in spellParent)
        {
            if (spellChild.TryGetComponent(out Spell spell))
            {
                if (_player.playerData.learnedSpells.Contains(spell.name.ToLower()))
                {
                    spellChild.gameObject.SetActive(true);
                }
                else
                {
                    spellChild.gameObject.SetActive(false);
                }
            }
            else
            {
                throw new Exception("Spell " + spellChild.name + " without SpellScript Component");
            }
        }
    }

    private void Start()
    {
        _newNetworkConfidenceDisplay = FindObjectOfType<NewNetworkConfidenceDisplay>();
        _drawingController = FindObjectOfType<NewDrawingController>();
    }
    
    private float _targetTimeScale = 1;
    private const float Speed = 0.5f;

    public void CastSpell()
    {
        if (selectedSpell != null && !_spellOnCd && !_isDrawing)
        {
            float t = selectedSpell.CastSpell();
            UpdateManaBar();
            StartCoroutine(CdTimer(t, 1));
        }
    }
    
    void Update()
    {
        if (!_isDrawing)
        {
            return;
        }
        
        Time.timeScale = Mathf.MoveTowards(Time.timeScale, _targetTimeScale, 
            Time.unscaledDeltaTime * Speed);
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
    
    private void SetNewSpell(string predictedLabel)
    {
        foreach (Transform spellChild in spellParent)
        {
            if (!spellChild.gameObject.activeSelf)
            {
                continue;
            }
            if (spellChild.GetComponent<Spell>().spellData.spellName == predictedLabel.ToLower())
            {
                if (selectedSpell!= null)
                {
                    selectedSpell.isSelected = false;  
                }

                selectedSpell = spellChild.GetComponent<Spell>();
                selectedSpell.isSelected = true;
                
                spellImage.sprite = selectedSpell.spellData.spellIcon;
                UpdateManaBar();
                return;
            }
        }
        Debug.Log("Cannot select new Spell");
    }

    private void SetTimeScale(float timeScale)
    {
        _targetTimeScale = timeScale;
    }
    
    private IEnumerator SlowMotionTimer(float duration, float slowestTime)
    {
        if (_isDrawing)
        {
            SetTimeScale(slowestTime);
            yield return new WaitForSeconds(duration);
            SetTimeScale(1f);
        }
    }
    private IEnumerator CdTimer(float duration, int cd)
    {
        if (cd == 0)
        {
            _drawingOnCd = true;
        }else if (cd == 1)
        {
            _spellOnCd = true;
        }
        
        yield return new WaitForSeconds(duration);
        
        if (cd == 0)
        {
            _drawingOnCd = false;
        }else if (cd == 1)
        {
            _spellOnCd = false;
        }
    }

    public bool OpenDisplay()
    {
        if (!_drawingOnCd)
        {
            drawingDisplay.SetActive(true);
            _isDrawing = true;
            StartCoroutine(SlowMotionTimer(2, 0.3f));
            return true;
        }

        return false;
    }

    public bool CloseDisplay()
    {
        if (_newNetworkConfidenceDisplay.predictedLabel != "Wrong Symbol")
        {
            SetNewSpell(_newNetworkConfidenceDisplay.predictedLabel);
        }
        _drawingController.Clear();
        StartCoroutine(CdTimer(0.5f, 0));
        _isDrawing = false;
        drawingDisplay.SetActive(false);
        _targetTimeScale = 1;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        return true;
    }
}
