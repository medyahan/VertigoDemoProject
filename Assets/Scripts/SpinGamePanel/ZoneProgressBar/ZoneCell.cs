using Core;
using TMPro;
using UnityEngine;
using ZoneType = SpinGameData.ZoneType;

namespace SpinGamePanel.ZoneProgressBar
{
    public class ZoneCell : BaseMonoBehaviour
    {
        #region Variable Field
        
        [Header("CELL COMPONENTS")]
        [SerializeField] private TMP_Text _zoneText;
        [SerializeField] private CanvasGroup _canvasGroup;
        
        [Header("ZONE COLORS")]
        [SerializeField] private Color _defaultZoneColor;
        [SerializeField] private Color _safeZoneColor;
        [SerializeField] private Color _superZoneColor;

        [Header("VALUES")] 
        [SerializeField] private float _inactivateAlphaValue;

        private int _zoneIndex;
        private ZoneType _zoneType;
        
        #endregion // Variable Field
    
        public override void Initialize(params object[] list)
        {
            base.Initialize(list);

            _zoneIndex = (int)list[0];
            _zoneType = SpinGameEventLib.Instance.GetZoneTypeOfZoneIndex(_zoneIndex);

            PrepareUI();
        }

        /// <summary>
        /// Prepares the UI elements for the current zone, updating the zone text and color based on the current zone index and type.
        /// </summary>
        private void PrepareUI()
        {
            _zoneText.text = _zoneIndex.ToString();

            // Determine the color of the zone text based on the current zone type.
            if (_zoneType == ZoneType.SuperZone)
            {
                _zoneText.color = _superZoneColor;
            }
            else if(_zoneType == ZoneType.SafeZone)
            {
                _zoneText.color = _safeZoneColor;
            }
            else
            {
                _zoneText.color = _defaultZoneColor;
            }
        }

        public void Inactivate()
        {
            _canvasGroup.alpha = _inactivateAlphaValue;
        }
    }
}
