using System.Collections.Generic;
using Core;
using DG.Tweening;
using UnityEngine;

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
        [SerializeField] private ZoneIndicator _zoneIndicator;

        [Header("VALUES")] 
        [SerializeField] private float _scrollAnimateDuration;
        [SerializeField] private int _cellWidth;
        
        [SerializeField] private float _startContentParentPosX;
        private int _totalZoneCount;
        
        #endregion // Variable Field

        public override void Initialize(params object[] list)
        {
            base.Initialize(list);

            _totalZoneCount = (int) list[0];
            
            ResetContentPosition();
            CreateZoneCells();
            _zoneIndicator.UpdateZoneIndicatorUI(1);
        }

        public override void End()
        {
            base.End();
            ClearAllZoneCell();
        }

        /// <summary>
        /// Instantiates and initializes ZoneCell prefabs for each game zone, adding them to the zone cell list.
        /// </summary>
        private void CreateZoneCells()
        {
            for (int i = 0; i < _totalZoneCount; i++)
            {
                ZoneCell zoneCell = Instantiate(_zoneCellPrefab, _contentParent);

                zoneCell.Initialize(i + 1);
            
                _zoneCellList.Add(zoneCell);
            }
        }

        /// <summary>
        /// Clears and removes all zone cells from the list and scene.
        /// </summary>
        private void ClearAllZoneCell()
        {
            foreach (ZoneCell zoneCell in _zoneCellList){
                
                if(zoneCell == null)
                    continue;
               
                zoneCell.End();
                Destroy(zoneCell.gameObject);
            }
            _zoneCellList.Clear();
        }

        private void ResetContentPosition()
        {
            Vector3 contentPosition = _contentParent.localPosition;
            contentPosition.x = _startContentParentPosX;
            _contentParent.localPosition = contentPosition;
        }

        /// <summary>
        /// Handles the visual transition to the next game zone, deactivating the previous zone cell
        /// and animating the scroll content to reveal the next zone.
        /// </summary>
        /// <param name="currentZoneIndex">The index of the current game zone.</param>
        public void OnChangeCurrentZoneIndex(int currentZoneIndex)
        {
            // Inactivate previous zone cell
            _zoneCellList[currentZoneIndex - 2].Inactivate();
            
            // Animate scroll content for the next zone
            Vector3 newContentPosition = _contentParent.localPosition;
            newContentPosition.x = newContentPosition.x - _cellWidth;
            
            _contentParent.DOLocalMove(newContentPosition, _scrollAnimateDuration).OnComplete(() =>
            {
                _zoneIndicator.UpdateZoneIndicatorUI(currentZoneIndex);
            });
        }
    }
}
