using System.Collections.Generic;
using Core;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZoneType = SpinGameData.ZoneType;

namespace SpinGamePanel.ZoneProgressBar
{
    public class ZoneProgressBarController : BaseMonoBehaviour
    {
        #region Variable Field
        
        private List<ZoneCell> _zoneCellList = new List<ZoneCell>();
    
        [Header("ZONE CELL")]
        [SerializeField] private ZoneCell _zoneCellPrefab;
        [SerializeField] private Transform _contentParent;

        [Header("ZONE INDICATOR")]
        [SerializeField] private Image _zoneIndicatorImage;
        [SerializeField] private TMP_Text _zoneIndicatorText;

        [Header("ZONE INDICATOR SPRITES")]
        [SerializeField] private Sprite _defaultZoneSprite;
        [SerializeField] private Sprite _safeZoneSprite;
        [SerializeField] private Sprite _superZoneSprite;

        [Header("VALUES")] 
        [SerializeField] private float _scrollAnimateDuration;
        [SerializeField] private int _cellWidth;
        
        private int _totalZoneCount;
        private float _startContentParentPosX;
        
        #endregion // Variable Field

        public override void Initialize(params object[] list)
        {
            base.Initialize(list);

            _totalZoneCount = (int) list[0];

            _startContentParentPosX = _contentParent.localPosition.x;
            
            CreateZoneCells();
        
            UpdateZoneIndicator(1, ZoneType.DefaultZone); //TODO Düzelt
        }

        public override void End()
        {
            base.End();
               
           Vector3 contentPosition = _contentParent.localPosition;
           contentPosition.x = _startContentParentPosX;

           _contentParent.localPosition = contentPosition;

           foreach (var zoneCell in _zoneCellList)
           {
               zoneCell.End();
               Destroy(zoneCell.gameObject);
           }
            _zoneCellList.Clear();
        }

        private void CreateZoneCells()
        {
            for (int i = 0; i < _totalZoneCount; i++)
            {
                ZoneCell zoneCell = Instantiate(_zoneCellPrefab, _contentParent);
            
                zoneCell.Initialize(i+1);
            
                _zoneCellList.Add(zoneCell);
            }
        }

        public void OnChangeCurrentZoneIndex(int currentZoneIndex, ZoneType currentZoneType)
        {
            // Inactivate previous zone cell
            _zoneCellList[currentZoneIndex - 1].Inactivate();
            
            // Animate scroll content for the next zone
            Vector3 newContentPosition = _contentParent.localPosition;
            newContentPosition.x = newContentPosition.x - _cellWidth;
            
            _contentParent.DOLocalMove(newContentPosition, _scrollAnimateDuration).OnComplete(() =>
            {
                UpdateZoneIndicator(currentZoneIndex, currentZoneType);
            });
        }

        private void UpdateZoneIndicator(int currentZoneIndex, ZoneType currentZoneType)
        {
            _zoneIndicatorText.text = currentZoneIndex.ToString();

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
}
