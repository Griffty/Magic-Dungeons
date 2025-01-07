
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestHandler: MonoBehaviour
{
    [SerializeField] private List<Quest> allQuests = new();
    private static List<Quest> _allQuests = new();
    private void Start()
    {
        allQuests.Add(new KillQuest(0, "New Beginning", "You, brave hero, need to clean this castle from evil undead monsters. Kill them and talk to trader to complete Quest", "Kill any 5 mobs", 5, true, EnemyStyle.Any, EnemyType.Any));
        allQuests.Add(new KillQuest(1, "Big Cleaning", "You, brave hero, need to clean this castle from terrible ranged monsters. Kill them to complete Quest", "Kill 10 ranged mobs", 10, false, EnemyStyle.Any, EnemyType.Range));
        allQuests.Add(new GiveQuest(2, "Gift to friend", "You, brave hero, need to Find ang give an ultimate chest plate to trader. Talk to him to complete the quest", "Find and give Ultimate ChestPlate to Trader", 1, true, ItemHandler.FindItemByName("Divine Chestplate")));
        
        _allQuests = allQuests;
    }

    public static Quest GetQuestByIndex(int index)
    {
        return _allQuests.FirstOrDefault(quest => quest.Index == index);
    }
}
