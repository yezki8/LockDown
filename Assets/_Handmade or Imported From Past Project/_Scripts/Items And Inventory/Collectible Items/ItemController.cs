using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public ItemSO ItemScriptableObject;
    [Min(1)] public int Amount;
}
