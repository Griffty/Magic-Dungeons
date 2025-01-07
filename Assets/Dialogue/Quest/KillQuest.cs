using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class KillQuest : Quest
{
    [SerializeField] private EnemyStyle mobStyle;
    [SerializeField] private EnemyType mobType;
    public KillQuest(int index, string name, string desc, string obj, int amount, bool talkToComplete, EnemyStyle mobStyle, EnemyType mobType) : base(index, name, desc, obj, amount, talkToComplete)
    {
        this.mobStyle = mobStyle;
        this.mobType = mobType;
    }

    private void OnEnemyDeath(EnemyType type, EnemyStyle style)
    {
        if (mobType == type || mobType == EnemyType.Any)
        {
            if (mobStyle == style || mobStyle == EnemyStyle.Any)
            {
                currentAmount++;
            }
        }

        BaseOnAction();
    }
    public override void OnAdd()
    {
        Enemy.OnThisEnemyDeath += OnEnemyDeath;
    }
}