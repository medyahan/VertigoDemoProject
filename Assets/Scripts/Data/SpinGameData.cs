using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SpinGameData", menuName = "Data/SpinGameData")]
public class SpinGameData : ScriptableObject
{
    public Sprite BombSprite;
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
        public Sprite WheelSprite;
        public Sprite IndicatorSprite;
        public RewardData[] Rewards;

        public RewardData[] GetRandomRewardList(int rewardCount)
        {
            // düzelt
            return Rewards;
        }
    }

    [Serializable]
    public class RewardData
    {
        public RewardType Type;
        public Sprite Sprite;
        public int Amount;
    }

    public enum SpinType
    {
        Bronze,
        Silver,
        Gold
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