using System;
using System.Collections;
using UnityEngine;

public class HealthHandler : MonoBehaviour
{
    public float healthPoints;
    public float maxHealth;
    public bool onHitCd;
    public ResistanceData resistanceData;
    private static PlayerData _playerData;
    private bool _isAttachedToPlayer;

    public delegate void OnDamageTake();
    public static event OnDamageTake OnPlayerDamageTake;

    private void Awake()
    {
        _playerData = FindObjectOfType<Player>().playerData;
    }

    private void Start()
    {
        if (TryGetComponent(out Player _))
        {
            maxHealth = _playerData.maxHealth;
            _isAttachedToPlayer = true;
            OnPlayerDamageTake?.Invoke();
        }
        healthPoints = maxHealth;
    }

    private void Update()
    {
        if (_isAttachedToPlayer)
        {
            maxHealth = _playerData.maxHealth;
        }
        if (healthPoints <= 0)
        {
            if (gameObject.TryGetComponent(out IDestroyable destroyable))
            {
                destroyable.At0Hp();
            }
            else
            {
                throw new Exception("Cannot get IDestroyable from Object with attached health handler");
            }
        }
    }

    public void TakeDamage(float amount)
    {
        if (onHitCd)
        {
            return;
        }
        healthPoints -= amount;
        if (_isAttachedToPlayer)
        {
            OnPlayerDamageTake?.Invoke();
        }
        StartCoroutine(StartHitCd(this));
    }

    private static IEnumerator StartHitCd(HealthHandler healthHandler)
    {
        healthHandler.onHitCd = true;
        if (healthHandler.gameObject.TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            for (int i = 0; i < 2; i++)
            {
                spriteRenderer.enabled = false;
                yield return new WaitForSeconds(0.1f);
                spriteRenderer.enabled = true;
                yield return new WaitForSeconds(0.2f);
            }
            yield return new WaitForSeconds(0.2f);
            healthHandler.onHitCd = false;
            yield break;
        }
        yield return new WaitForSeconds(0.8f);
        healthHandler.onHitCd = false;
    }

    public void TakeDamage(float amount, DamageType damageType, bool usePlayerSpellDamage)
    {
        if (!resistanceData)
        {
            return;
        }
        if (damageType != DamageType.Physic)
        {
            float damageAfterAmpl = amount;
            
            if (usePlayerSpellDamage)
            { 
                damageAfterAmpl = amount * (_playerData.spellDamageData.GetAmp(damageType) + 1);
            }
            
            float damageReduction = resistanceData.GetResist(damageType);
            float finalDamage = damageAfterAmpl * (1 - damageReduction);
            TakeDamage(finalDamage);
        }
        else
        {
            float damageReduction = _playerData.armor/(_playerData.armor + 40) * 0.9f;
            float finalDamage = amount * (1 - damageReduction);
            TakeDamage(finalDamage);
        }
    }
    
    
    public void TakeDamage(float amount, DamageType damageType, bool useResistance, bool usePlayerSpellDamage)
    {
        if (!resistanceData)
        {
            return;
        }
        if (damageType != DamageType.Physic)
        {
            float damageAfterAmpl = amount;
            
            if (usePlayerSpellDamage)
            { 
                damageAfterAmpl = amount * (_playerData.spellDamageData.GetAmp(damageType) + 1);
            }

            float damageReduction = _playerData.resistanceData.GetResist(damageType);
            
            if (!useResistance)
            {
                damageReduction = 0;
            }
           
            float finalDamage = damageAfterAmpl * (1 - damageReduction);
            TakeDamage(finalDamage);
        }
        else
        {
            float damageReduction = _playerData.armor/(_playerData.armor + 40) * 0.9f;
            if (!useResistance)
            {
                damageReduction = 0;
            }
            
            float finalDamage = amount * (1 - damageReduction);
            TakeDamage(finalDamage);
        }
    }

    public void Heal(float amount)
    {
        healthPoints += amount;
        if (healthPoints > maxHealth)
        {
            healthPoints = maxHealth;
        }
    }

    public void SetMaxHealth(float amount, bool restoreCurrentHp)
    {
        maxHealth = amount;
        if (restoreCurrentHp)
        {
            Heal(amount);
        }
    }
}

public enum DamageType
{
    Fire,
    Water,
    Lightning,
    Wind,
    Earth,
    Holy,
    Dark,
    Physic,
}
