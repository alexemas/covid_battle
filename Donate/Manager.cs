using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] private GameObject _panelIAP;

    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    private void Start()
    {
        _panelIAP.SetActive(false);
    }

    public void ControlIAP(bool status)
    {
        _panelIAP.SetActive(status);
    }
}
