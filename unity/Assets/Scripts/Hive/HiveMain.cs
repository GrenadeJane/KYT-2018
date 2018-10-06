using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HiveMain : HiveBase, IPointerClickHandler
{

    [SerializeField] GameObject buttonContainer;

    [SerializeField] Button zoneButton;
    [SerializeField] Button copButton;
    [SerializeField] Button politestButton;


    public static HiveMain m_Instance;


    public void OnPointerClick(PointerEventData eventData)
    {
        buttonContainer.SetActive(true);
    }

    private void Awake()
    {
        if (m_Instance == null)
            m_Instance = this;

        buttonContainer.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();

        if (buttonContainer.activeInHierarchy)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            if (screenPos.x < Screen.currentResolution.width && screenPos.x > 0
                && screenPos.y > 0 && screenPos.y < Screen.currentResolution.height)
                buttonContainer.transform.position = screenPos;
            else
            {
                buttonContainer.SetActive(false);
            }
        }
    }
}
