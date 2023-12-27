using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SpinGameData", menuName = "Data/SpinGameData")]
public class SpinGameData : ScriptableObject
{
    public Sprite BombSprite;
    
    [Header("SPIN WHEEL DATA")]
    public SpinWheelData BronzeSpinWheelData;
    public SpinWheelData SilverSpinWheelData;
    public SpinWheelData GoldSpinWheelData;

    public SpinWheelData GetSpinWheelDataByZoneIndex(int zoneIndex)
    {
        ZoneType zoneType = GetZoneTypeOfZoneIndex(zoneIndex);
        
        switch (zoneType)
        {
            case ZoneType.DefaultZone:
                return BronzeSpinWheelData;
            case ZoneType.SafeZone:
                return SilverSpinWheelData;
            default:
                return GoldSpinWheelData;
        }
    }

    public ZoneType GetZoneTypeOfZoneIndex(int zoneIndex)
    {
        if (zoneIndex % 30 == 0)
        {
            return ZoneType.SuperZone;
        }

        if (zoneIndex % 5 == 0)
        {
            return ZoneType.SafeZone;
        }

        return ZoneType.DefaultZone;
    }

    [Serializable]
    public class SpinWheelData
    {
        public ZoneType Type;
        public bool HasBomb;
        public string WheelSpriteName;
        public string IndicatorSpriteName;
        public RewardData[] Rewards;
    }

    [Serializable]
    public class RewardData
    {
        public string SpriteName;
        public int Amount;

        // public Sprite GetSprite()
        // {
        //     
        // }
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