using System.Collections;
using System.Collections.Generic;
using Core;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NextZoneInfoController : BaseMonoBehaviour
{
    #region Variable Field
    
    [Header("TEXTS")]
    [SerializeField] private TMP_Text _nextSafeZoneText;
    [SerializeField] private TMP_Text _nextSuperZoneText;
    
    private List<int> _nextSafeZoneList = new List<int>();
    private List<int> _nextSuperZoneList = new List<int>();
    
    private int _totalZoneCount;
    private int _everySafeZoneFactor;
    private int _everySuperZoneFactor;
    
    #endregion // Variable Field

    public override void Initialize(params object[] list)
    {
        base.Initialize(list);

        _totalZoneCount = (int) list[0];
        _everySafeZoneFactor = (int) list[1];
        _everySuperZoneFactor = (int) list[2];
        
        ListNextZones();

        _nextSafeZoneText.text = _nextSafeZoneList[0].ToString();
        _nextSuperZoneText.text = _nextSuperZoneList[0].ToString();
    }

    public override void End()
    {
        base.End();
        
        _nextSafeZoneList.Clear();
        _nextSuperZoneList.Clear();
    }

    /// <summary>
    /// Creates lists of next zones based on specific conditions.
    /// </summary>
    private void ListNextZones()
    {
        for (int i = 1; i < _totalZoneCount+1; i++)
        {
            if (i % _everySuperZoneFactor == 0)
            {
                _nextSuperZoneList.Add(i);
            }
            else if (i % _everySafeZoneFactor == 0)
            {
                _nextSafeZoneList.Add(i);
            }
        }
    }

    /// <summary>
    /// Handles the change in the current game zone index. Updates the lists of next safe and super zones
    /// based on the current zone index and triggers the display of relevant texts.
    /// </summary>
    /// <param name="currentZoneIndex">The index of the current game zone.</param>
    public void OnChangeCurrentZoneIndex(int currentZoneIndex)
    {
        // Update next safe zone info
        if (_nextSafeZoneList.Count != 0)
        {
            if (currentZoneIndex == _nextSafeZoneList[0])
            {
                _nextSafeZoneList.RemoveAt(0);
            }
        }
        
        // Update next super zone info
        if (_nextSuperZoneList.Count != 0)
        {
            if (currentZoneIndex == _nextSuperZoneList[0])
            {
                _nextSuperZoneList.RemoveAt(0);
            }
        }
        
        DisplayTexts();
    }

    /// <summary>
    /// Displays the next safe and super zone indices on the UI based on the current state of their respective lists.
    /// </summary>
    private void DisplayTexts()
    {
        if (_nextSafeZoneList.Count != 0)
            _nextSafeZoneText.text = _nextSafeZoneList[0].ToString();
        else
            _nextSafeZoneText.text = "X"; // If there are no more safe zones, display "X".
        
        if (_nextSuperZoneList.Count != 0)
        {
            _nextSuperZoneText.text = _nextSuperZoneList[0].ToString();
        }
        else
            _nextSuperZoneText.text = "X"; // If there are no more super zones, display "X".
    }
    
}
