using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ITEMSO_", menuName = "ScriptableObjects/ItemScriptableObject", order = 1)]
public class ItemSO : ScriptableObject
{
    public enum ItemType        //to determine
    {
        Weapon,
        Consumable
    }
    public ItemType TypeOfItem;

    public string ItemName;
    public string ItemDescription;
    public Sprite ItemSprite;

    [Header("Parameters if this was a weapon")]
    public GameObject ProjectileObject;
    public HitType TypeOfHit = HitType.melle;

    [Header("Parameters if this was a consumable")]
    public int HealthRestore;
}
