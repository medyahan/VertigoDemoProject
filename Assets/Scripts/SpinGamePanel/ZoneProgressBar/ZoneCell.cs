using Core;
using TMPro;
using UnityEngine;

namespace SpinGamePanel.ZoneProgressBar
{
    public class ZoneCell : BaseMonoBehaviour
    {
        [Header("CELL COMPONENTS")]
        [SerializeField] private TMP_Text _zoneText;
        [SerializeField] private CanvasGroup _canvasGroup;
        
        [Header("ZONE COLORS")]
        [SerializeField] private Color _defaultZoneColor;
        [SerializeField] private Color _safeZoneColor;
        [SerializeField] private Color _superZoneColor;

        private int _zoneIndex;
    
        public override void Initialize(params object[] list)
        {
            base.Initialize(list);

            _zoneIndex = (int)list[0];

            _zoneText.text = _zoneIndex.ToString();

            //burası düzenlecek
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

        public void Inactive()
        {
            _canvasGroup.alpha = .2f;
        }
    }
}
