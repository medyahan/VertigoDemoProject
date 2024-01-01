using System;
using UnityEngine;
using ZoneType = SpinGameData.ZoneType;
using RewardData = SpinGameData.RewardData;

public class SpinGameEventLib : Singleton<SpinGameEventLib>
{
    public Action<RewardData> RewardCollected;
    public Action BombExploded;
    public Action ClickedExitButton;
    public Func<bool> GetIsWheelSpinning;
    public Func<int, ZoneType> GetZoneTypeOfZoneIndex;
    public Func<RewardData[]> GetAllRewardInInventory;

    protected override void Awake()
    {
        DestroyOnLoad = true;
        base.Awake();
    }
    
    private void OnDestroy()
    {
        RewardCollected = null;
        BombExploded = null;
        ClickedExitButton = null;
        GetIsWheelSpinning = null;
        GetZoneTypeOfZoneIndex = null;
        GetAllRewardInInventory = null;
    }
}
