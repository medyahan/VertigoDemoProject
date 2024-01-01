using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZoneType = SpinGameData.ZoneType;

public class ZoneIndicator : BaseMonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField] private Image _zoneIndicatorImage;
    [SerializeField] private TMP_Text _zoneIndicatorText;

    [Header("SPRITES")]
    [SerializeField] private Sprite _defaultZoneSprite;
    [SerializeField] private Sprite _safeZoneSprite;
    [SerializeField] private Sprite _superZoneSprite;
    
    /// <summary>
    /// Updates the UI elements of the zone indicator based on the current game zone index.
    /// </summary>
    /// <param name="currentZoneIndex">The index of the current game zone.</param>
    public void UpdateZoneIndicatorUI(int currentZoneIndex)
    {
        _zoneIndicatorText.text = currentZoneIndex.ToString();

        ZoneType currentZoneType = SpinGameEventLib.Instance.GetZoneTypeOfZoneIndex(currentZoneIndex);

        // Set the zone indicator sprite based on the current zone type.
        switch (currentZoneType)
        {
            case ZoneType.DefaultZone:
                _zoneIndicatorImage.sprite = _defaultZoneSprite;
                break;
            case ZoneType.SafeZone:
                _zoneIndicatorImage.sprite = _safeZoneSprite;
                break;
            default:
                _zoneIndicatorImage.sprite = _superZoneSprite;
                break;
        }
    }
}
