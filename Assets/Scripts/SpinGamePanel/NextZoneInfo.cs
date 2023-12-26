using System.Collections;
using System.Collections.Generic;
using Core;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NextZoneInfo : BaseMonoBehaviour
{
    [SerializeField] private TMP_Text _nextSafeZoneText;
    [SerializeField] private TMP_Text _nextSuperZoneText;
    
    private int _totalZoneCount;

    private List<int> _nextSafeZoneList = new List<int>();
    private List<int> _nextSuperZoneList = new List<int>();

    public override void Initialize(params object[] list)
    {
        base.Initialize(list);

        _totalZoneCount = (int) list[0];
        
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

    private void ListNextZones()
    {
        for (int i = 1; i < _totalZoneCount+1; i++)
        {
            if (i % 30 == 0)
            {
                _nextSuperZoneList.Add(i);
            }
            else if (i % 5 == 0)
            {
                _nextSafeZoneList.Add(i);
            }
        }
    }

    public void OnChangeCurrentZoneIndex(int currentZoneIndex)
    {
        if (currentZoneIndex == _nextSafeZoneList[0])
        {
            _nextSafeZoneList.RemoveAt(0);
        }

        if (currentZoneIndex == _nextSuperZoneList[0])
        {
            _nextSuperZoneList.RemoveAt(0);
        }
        
        _nextSafeZoneText.text = _nextSafeZoneList[0].ToString();
        _nextSuperZoneText.text = _nextSuperZoneList[0].ToString();
    }
    
}
