using UnityEngine;
using ZoneType = SpinGameData.ZoneType;
using RewardData = SpinGameData.RewardData;

[CreateAssetMenu(fileName = "SpinWheelData", menuName = "Data/SpinGame/SpinWheelData")]
public class SpinWheelData : ScriptableObject
{
    public ZoneType ZoneType;
    public bool HasBomb;
    public Sprite WheelSprite;
    public Sprite IndicatorSprite;
    public RewardData[] Rewards;
}
