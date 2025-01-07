using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerQuestManager : MonoBehaviour, IDisplayable
{
    private static PlayerQuestManager _this;
    [SerializeField] private GameObject questDisplay;
    [SerializeField] private RectTransform questTemplate;
    [SerializeField] private RectTransform content;

    [SerializeField] private TextMeshProUGUI questTitleText;
    [SerializeField] private TextMeshProUGUI questDescText;
    [SerializeField] private TextMeshProUGUI questObjectiveText;
    [SerializeField] private TextMeshProUGUI questProgressText;
    
    [SerializeField] public List<Quest> activeQuests = new();

    private void Awake()
    {
        _this = this;
    }

    public void AddNewQuest(int questIndex)
    {
        Quest q = QuestHandler.GetQuestByIndex(questIndex);
        if (activeQuests.Contains(q))
        {
            if (q.talkToComplete && q.currentAmount >= q.amount) q.OnComplete();
            else
            {
                throw new Exception("You already have this snus"); 
            }
        }
        q.OnAdd();
        activeQuests.Add(q);
        UpdateListDisplay();
    }

    private void UpdateListDisplay()
    {
        ClearQuests();
        int contentHeight = 0;
        for (int i = 0; i < activeQuests.Count; i++)
        {
            int index = i;
            var q = activeQuests[i];
            GameObject quest = Instantiate(questTemplate.gameObject, content);
            quest.GetComponent<Button>().onClick.AddListener(() => UpdateQuestDetails(index));
            foreach (Transform text in quest.transform)
            {
                if (text.name == "Title")
                {
                    text.GetComponent<TextMeshProUGUI>().text = q.name;
                    continue;
                }

                if (text.name == "Desc")
                {
                    text.GetComponent<TextMeshProUGUI>().text = q.desc;
                }
            }

            quest.SetActive(true);
            contentHeight += (int)questTemplate.sizeDelta.y;
        }

        content.sizeDelta = new Vector2(content.sizeDelta.x, contentHeight);
    }

    private void UpdateQuestDetails(int questIndex)
    {
        questTitleText.text = activeQuests[questIndex].name;
        questDescText.text = activeQuests[questIndex].desc;
        questObjectiveText.text = activeQuests[questIndex].obj;
        questProgressText.text = activeQuests[questIndex].currentAmount + "/" + activeQuests[questIndex].amount;
    }

    public static void StaticUpdateQuestDetails(int questIndex)
    {
        _this.UpdateQuestDetails(questIndex);
    }

    private void ClearQuests()
    {
        foreach (Transform child in content) {
            if (child.gameObject.activeSelf)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public bool OpenDisplay()
    {
        if (questDisplay.activeInHierarchy)
        {
            return false;
        }
        questDisplay.SetActive(true);
        return true;
    }

    public bool CloseDisplay()
    {
        if (!questDisplay.activeInHierarchy)
        {
            return false;
        }
        questDisplay.SetActive(false);
        return true;
    }
}
