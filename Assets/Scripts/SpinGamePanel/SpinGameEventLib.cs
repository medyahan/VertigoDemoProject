using System;
using UnityEngine;

public class SpinGameEventLib : Singleton<SpinGameEventLib>
{
    public Action<SpinGameData.RewardData> RewardCollected;
    public Action BombExploded;
    public Action ClickedExitButton;
    public Func<bool> GetIsWheelSpinning;
    public Func<int, SpinGameData.ZoneType> GetZoneTypeOfZoneIndex;
    public Func<SpinGameData.RewardData[]> GetAllRewardInInventory;

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
