using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SpinGameData", menuName = "Data/SpinGame/SpinGameData")]
public class SpinGameData : ScriptableObject
{
    public Sprite BombSprite;
    
    [Header("ZONE FACTOR VALUES")]
    public int EverySafeZoneFactor;
    public int EverySuperZoneFactor;

    [Header("ZONE FACTOR VALUES")] 
    public int ReviveCurrencyValue;

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

    public enum RewardType
    {
        Bomb,
        Cash,
        Weapon,
        Gold,
        Knife
    }
}