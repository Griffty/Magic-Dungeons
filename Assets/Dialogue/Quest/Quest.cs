using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Quest
{
    public readonly int Index;
    public string name;
    public string desc;
    public string obj;
    public int amount;
    public int currentAmount = 0;
    public bool talkToComplete;
    

    protected Quest(int index, string name, string desc, string obj, int amount, bool talkToComplete)
    {
        Index = index;
        this.name = name;
        this.desc = desc;
        this.obj = obj;
        this.amount = amount;
        this.talkToComplete = talkToComplete;
    }

    public virtual void OnAdd()
    {
        
    }

    protected void BaseOnAction()
    {
        PlayerQuestManager.StaticUpdateQuestDetails(Index);
        if (currentAmount >= amount)
        {
            if (!talkToComplete)
            {
                OnComplete();
            }
        }
    }

    public void OnComplete()
    {
        Debug.Log("Hooray");
    }
}