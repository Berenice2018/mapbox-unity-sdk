using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCtrl : MonoBehaviour
{
    public GameObject customMap, defaultMap;

    private bool _showCustom;

    public void ToggleDataLayer()
    {
        _showCustom = !_showCustom;
        customMap.SetActive(_showCustom);
        defaultMap.SetActive(!_showCustom);
    }


}
