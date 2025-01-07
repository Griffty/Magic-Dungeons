
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Custom/PlayerData")]
public class PlayerData : ScriptableObject
{
    //Basic
    public readonly float _baseHealth = 10;
    public readonly float _baseMoveSpeed = 10;
    
    public float maxHealth = 10;
    public float armor;
    public float moveSpeed = 10;
    
    // Magic
    public List<String> learnedSpells;
    public float maxManaAmp;
    public float passiveManaRegenAmp;
    
    
    public SpellDamageData spellDamageData;
    public ResistanceData resistanceData;

    //Level:
    public int skillPoints;
    public int level;
    public int usedSkillPoints;
    
    //Items:
}