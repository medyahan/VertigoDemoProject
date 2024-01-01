using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SpinGameData", menuName = "Data/SpinGame/SpinGameData")]
public class SpinGameData : ScriptableObject
{
    public int TotalZoneCount;
    
    [Header("ZONE FACTOR VALUES")]
    public int EverySafeZoneFactor;
    public int EverySuperZoneFactor;

    [Header("CURRENCY VALUES")] 
    public CurrencyController.CurrencyType ReviveCurrencyType;
    public int ReviveCurrencyValue;
    
    [Header("BOMB")] 
    public Sprite BombSprite;

    public ZoneType GetZoneTypeOfZoneIndex(int zoneIndex)
    {
        if (zoneIndex % EverySuperZoneFactor == 0)
        {
            return ZoneType.SuperZone;
        }

        if (zoneIndex % EverySafeZoneFactor == 0)
        {
            return ZoneType.SafeZone;
        }

        return ZoneType.DefaultZone;
    }

    [Serializable]
    public class RewardData
    {
        public Sprite Sprite;
        public int Amount;
    }
    
    public enum ZoneType
    {
        DefaultZone,
        SafeZone,
        SuperZone
    }
}