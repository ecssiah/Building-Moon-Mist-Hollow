using System;
using UnityEngine;

[Serializable]
public struct CitizenData
{
    public GameObject Entity;

    public GroupData GroupData;
    public InventoryData InventoryData;

    public int CitizenNumber;
    public string Name;
}
