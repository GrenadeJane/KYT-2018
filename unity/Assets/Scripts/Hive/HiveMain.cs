using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HiveMain : HiveBase, IPointerClickHandler
{

    public static HiveMain m_Instance;

    UIManager uiManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        uiManager.ShowButtonsHive();
    }

    private void Awake()
    {
        if (m_Instance == null)
            m_Instance = this;

        uiManager = GameObject.FindObjectOfType<UIManager>();
    }
}
