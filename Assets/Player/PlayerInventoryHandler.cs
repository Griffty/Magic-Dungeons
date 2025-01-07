using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventoryHandler : MonoBehaviour, IDisplayable, IItemContainer // add Item/Effect list, that will calculate states based on effects(items datas)
{
    public EquipmentData clearEquipmentData;
    
    public List<ItemPlaceScript> itemPlaceHolders = new();

    private ItemData _selectedItem;
    public GameObject inventoryDisplay;
    private Player _player;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI statsText;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private UnityEngine.UI.Image icon;

    [SerializeField] private int money;

    public ItemPlaceScript helmetItemPlace;
    public ItemPlaceScript chestPlateItemPlace;
    public ItemPlaceScript bootsItemPlace;
    public ItemPlaceScript wandItemPlace;

    public int GetMoney()
    { 
        UpdateMoneyDisplay();
        return money;
    }

    public void GiveMoney(int amount)
    {
        money += amount;
        UpdateMoneyDisplay();
    }

    public void TakeMoney(int amount)
    {
        money -= amount;
        UpdateMoneyDisplay();
    }

    private void UpdateMoneyDisplay()
    {
        moneyText.text = money.ToString();
    }

    private void Start()
    {
        _player = GetComponent<Player>();
        RecalculateStats();
        foreach (var placeHolder in itemPlaceHolders)
        {
            placeHolder.UpdateImage();
        }
        helmetItemPlace.UpdateImage();
        chestPlateItemPlace.UpdateImage();
        bootsItemPlace.UpdateImage();
        wandItemPlace.UpdateImage();
    }
    public void RecalculateStats()
    {
        PlayerData data = _player.playerData;
        EquipmentData helmet = (EquipmentData)helmetItemPlace.GetItem();
        EquipmentData chestPlate = (EquipmentData)chestPlateItemPlace.GetItem();
        EquipmentData boots = (EquipmentData)bootsItemPlace.GetItem();
        EquipmentData wand = (EquipmentData)wandItemPlace.GetItem();
        if (helmet == null)
        {
            helmet = clearEquipmentData;
        }
        if (chestPlate == null)
        {
            chestPlate = clearEquipmentData;
        }if (boots == null)
        {
            boots = clearEquipmentData;
        }if (wand == null)
        {
            wand = clearEquipmentData;
        }
        data.armor = helmet.armor + chestPlate.armor + boots.armor + wand.armor;
        data.maxHealth = data._baseHealth + helmet.healthPoints + chestPlate.healthPoints + boots.healthPoints + wand.healthPoints;
        data.moveSpeed = data._baseHealth + helmet.moveSpeed + chestPlate.moveSpeed + boots.moveSpeed + wand.moveSpeed;
        data.maxManaAmp = helmet.manaAmp + chestPlate.manaAmp + boots.manaAmp + wand.manaAmp;
        data.passiveManaRegenAmp = helmet.passiveManaRegenAmp + chestPlate.passiveManaRegenAmp + boots.passiveManaRegenAmp + wand.passiveManaRegenAmp;

        data.resistanceData.darkArmor = helmet.resistanceData.darkArmor + chestPlate.resistanceData.darkArmor +
                                        boots.resistanceData.darkArmor + wand.resistanceData.darkArmor;
        data.resistanceData.earthArmor = helmet.resistanceData.earthArmor + chestPlate.resistanceData.earthArmor +
                                        boots.resistanceData.earthArmor + wand.resistanceData.earthArmor;
        data.resistanceData.fireArmor = helmet.resistanceData.fireArmor + chestPlate.resistanceData.fireArmor +
                                        boots.resistanceData.fireArmor + wand.resistanceData.fireArmor;
        data.resistanceData.holyArmor = helmet.resistanceData.holyArmor + chestPlate.resistanceData.holyArmor +
                                        boots.resistanceData.holyArmor + wand.resistanceData.holyArmor;
        data.resistanceData.lightningArmor = helmet.resistanceData.lightningArmor + chestPlate.resistanceData.lightningArmor +
                                        boots.resistanceData.lightningArmor + wand.resistanceData.lightningArmor;
        data.resistanceData.waterArmor = helmet.resistanceData.waterArmor + chestPlate.resistanceData.waterArmor +
                                        boots.resistanceData.waterArmor + wand.resistanceData.waterArmor;
        data.resistanceData.windArmor = helmet.resistanceData.windArmor + chestPlate.resistanceData.windArmor +
                                        boots.resistanceData.windArmor + wand.resistanceData.windArmor;

        data.spellDamageData.darkDamage = helmet.damageData.darkDamage + chestPlate.damageData.darkDamage +
                                          boots.damageData.darkDamage + wand.damageData.darkDamage;
        data.spellDamageData.earthDamage = helmet.damageData.earthDamage + chestPlate.damageData.earthDamage +
                                          boots.damageData.earthDamage + wand.damageData.earthDamage;
        data.spellDamageData.fireDamage = helmet.damageData.fireDamage + chestPlate.damageData.fireDamage +
                                          boots.damageData.fireDamage + wand.damageData.fireDamage;
        data.spellDamageData.holyDamage = helmet.damageData.holyDamage + chestPlate.damageData.holyDamage +
                                          boots.damageData.holyDamage + wand.damageData.holyDamage;
        data.spellDamageData.lightningDamage = helmet.damageData.lightningDamage + chestPlate.damageData.lightningDamage +
                                          boots.damageData.lightningDamage + wand.damageData.lightningDamage;
        data.spellDamageData.waterDamage = helmet.damageData.waterDamage + chestPlate.damageData.waterDamage +
                                          boots.damageData.waterDamage + wand.damageData.waterDamage;
        data.spellDamageData.windDamage = helmet.damageData.windDamage + chestPlate.damageData.windDamage +
                                           boots.damageData.windDamage + wand.damageData.windDamage;
    }

    public bool TryUseHere(ItemPlaceScript itemPlaceScript)
    {
        return itemPlaceScript.placeHolderType == PlaceHolderType.Any ? TryEquipItem(itemPlaceScript) : TryUnEquipItem(itemPlaceScript);
    }

    private bool TryEquipItem(ItemPlaceScript itemPlaceScript)
    {
        ItemData itemToWear = itemPlaceScript.GetItem();
        switch (itemToWear.itemType)
        {
            case PlaceHolderType.Helmet:
                // if (helmetItemPlace.GetItem() != null)
                // {
                //     if (AddItem(helmetItemPlace.GetItem()))
                //     {
                //         return false;
                //     }
                // }
                if (helmetItemPlace.GetItem() == null)
                {
                    if (helmetItemPlace.PutItem((EquipmentData)itemToWear))
                    {
                        itemPlaceScript.ClearItem();
                        break;
                    }
                }
                return false;
                
            case PlaceHolderType.ChestPlate:
                if (chestPlateItemPlace.GetItem() == null)
                {
                    if (chestPlateItemPlace.PutItem((EquipmentData)itemToWear))
                    {
                        itemPlaceScript.ClearItem();
                        break;
                    }
                }
                return false;
            case PlaceHolderType.Boots:
                if (bootsItemPlace.GetItem() == null)
                {
                    if (bootsItemPlace.PutItem((EquipmentData)itemToWear))
                    {
                        itemPlaceScript.ClearItem();
                        break;
                    }
                }
                return false;
            case PlaceHolderType.Wand:
                if (wandItemPlace.GetItem() == null)
                {
                    if (wandItemPlace.PutItem((EquipmentData)itemToWear))
                    {
                        itemPlaceScript.ClearItem();
                        break;
                    }
                }
                return false;
            default:
                return false;
        }
        RecalculateStats();
        return true;
    }

    public void SetSelectedItem(ItemData itemData)
    {
        _selectedItem = itemData;
        UpdateDescriptionDisplay(_selectedItem);
    }
    
    private void UpdateDescriptionDisplay(ItemData itemData)
    {
        if (itemData == null)
        {
            nameText.text = "";
            descText.text = "";
            statsText.text = "";
            icon.color = new Color(0,0,0,0);
            icon.sprite = null;
            return;
        }

        if (itemData.GetType() == typeof(EquipmentData))
        {
            statsText.text = GetStats((EquipmentData)itemData);
        }
        nameText.text = itemData.itemName;
        descText.text = itemData.itemDesc;
        icon.sprite = itemData.icon;
        icon.color = Color.white;
        
    }

    private string GetStats(EquipmentData equipmentData) // add icons 
    {
        string s = "";
        if (Math.Abs(equipmentData.armor - clearEquipmentData.armor) > 0.1f)
        {
            s += $"Armor: {equipmentData.armor} \n";
        }
        if (Math.Abs(equipmentData.healthPoints - clearEquipmentData.healthPoints) > 0.1f)
        {
            s += $"Health Points: {equipmentData.healthPoints} \n";
        }
        if (Math.Abs(equipmentData.manaAmp - clearEquipmentData.manaAmp) > 0.1f)
        {
            s += $"Mana: {equipmentData.manaAmp} \n";
        }
        if (Math.Abs(equipmentData.passiveManaRegenAmp - clearEquipmentData.passiveManaRegenAmp) > 0.1f)
        {
            s += $"Mana Regen: {equipmentData.passiveManaRegenAmp} \n";
        }
        if (Math.Abs(equipmentData.moveSpeed - clearEquipmentData.moveSpeed) > 0.1f)
        {
            s += $"Move Speed: {equipmentData.moveSpeed} \n";
        }

        ResistanceData resData = equipmentData.resistanceData;
        ResistanceData clearResData = clearEquipmentData.resistanceData;
        
        if (Math.Abs(resData.fireArmor - clearResData.fireArmor) > 0.1f)
        {
            s += $"Fire armor: {resData.fireArmor} \n";
        }

        if (Math.Abs(resData.waterArmor - clearResData.waterArmor) > 0.1f)
        {
            s += $"Water armor: {resData.waterArmor} \n";
        }
        
        if (Math.Abs(resData.windArmor - clearResData.windArmor) > 0.1f)
        {
            s += $"Wind armor: {resData.windArmor} \n";
        }

        if (Math.Abs(resData.earthArmor - clearResData.earthArmor) > 0.1f)
        {
            s += $"Earth armor: {resData.earthArmor} \n";
        }
        
        SpellDamageData damageData = equipmentData.damageData;
        SpellDamageData clearDamageData = clearEquipmentData.damageData;
        
        if (Math.Abs(damageData.fireDamage - clearDamageData.fireDamage) > 0.1f)
        {
            s += $"Fire armor: {damageData.fireDamage} \n";
        }

        if (Math.Abs(damageData.waterDamage - clearDamageData.waterDamage) > 0.1f)
        {
            s += $"Water armor: {damageData.waterDamage} \n";
        }
        
        if (Math.Abs(damageData.windDamage - clearDamageData.windDamage) > 0.1f)
        {
            s += $"Wind armor: {damageData.windDamage} \n";
        }

        if (Math.Abs(damageData.earthDamage - clearDamageData.earthDamage) > 0.1f)
        {
            s += $"Earth armor: {damageData.earthDamage} \n";
        }
        
        if (Math.Abs(damageData.lightningDamage - clearDamageData.lightningDamage) > 0.1f)
        {
            s += $"Water armor: {damageData.lightningDamage} \n";
        }
        
        if (Math.Abs(damageData.darkDamage - clearDamageData.darkDamage) > 0.1f)
        {
            s += $"Wind armor: {damageData.darkDamage} \n";
        }

        if (Math.Abs(damageData.holyDamage - clearDamageData.holyDamage) > 0.1f)
        {
            s += $"Earth armor: {damageData.holyDamage} \n";
        }
        return s;
    }

    public bool AddItem(ItemData item)
    {
        if (item == null)
        {
            return false;
        }
        for (int i = 0; i < itemPlaceHolders.Count; i++)
        {
            if (itemPlaceHolders[i].IsEmpty())
            {
                return itemPlaceHolders[i].PutItem(item);
            }
        }
        return false;
    }

    public bool OpenDisplay()
    {

        if (inventoryDisplay.activeInHierarchy)
        {
            return false;
        }

        inventoryDisplay.SetActive(true);
        return true;
    }

    public bool CloseDisplay()
    {
        if (!inventoryDisplay.activeInHierarchy)
        {
            return false;
        }
        SetSelectedItem(null);
        inventoryDisplay.SetActive(false);
        return true;
    }

    public ItemData GetSelectedItem()
    {
        return _selectedItem;
    }

    public bool TryUnEquipItem(ItemPlaceScript itemPlace)
    {
        if (itemPlace == helmetItemPlace)
        {
            AddItem(helmetItemPlace.GetItem());
            helmetItemPlace.ClearItem();
            RecalculateStats();
            return true;
        }
        
        if (itemPlace == chestPlateItemPlace)
        {
            AddItem(chestPlateItemPlace.GetItem());
            chestPlateItemPlace.ClearItem();
            RecalculateStats();
            return true;
        }
        
        if (itemPlace == bootsItemPlace)
        {
            AddItem(bootsItemPlace.GetItem());
            bootsItemPlace.ClearItem();
            RecalculateStats();
            return true;
        }
        
        if (itemPlace == wandItemPlace)
        {
            AddItem(wandItemPlace.GetItem());
            wandItemPlace.ClearItem();
            RecalculateStats();
            return true;
        }
        return false;
    }

    public bool HasEmptySlot()
    {
        return itemPlaceHolders.Any(holder => holder.IsEmpty());
    }
}