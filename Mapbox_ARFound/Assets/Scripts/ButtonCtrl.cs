using System.Collections;
using UnityEngine.SceneManagement;
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

    public void SwitchScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
