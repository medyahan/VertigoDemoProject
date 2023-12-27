using Core;
using TMPro;
using UnityEngine;

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
        
        #endregion // Variable Field
    
        public override void Initialize(params object[] list)
        {
            base.Initialize(list);

            _zoneIndex = (int)list[0];

            Prepare();
        }

        private void Prepare()
        {
            _zoneText.text = _zoneIndex.ToString();

            //TODO düzelt
            if (_zoneIndex % 30 == 0)
            {
                _zoneText.color = _superZoneColor;
            }
            else if(_zoneIndex % 5 == 0)
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
