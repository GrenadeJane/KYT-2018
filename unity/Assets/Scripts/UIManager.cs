using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [SerializeField] GameObject buttonContainer;

    [SerializeField] Button zoneButton;
    [SerializeField] Button hiveButton;
    [SerializeField] Button politestButton;

    Transform mainHiveTransform;

    private void Awake()
    {
        buttonContainer.SetActive(false);
    }

    public void ShowButtonsHive()
    {
        buttonContainer.SetActive(true);
    }
    // Use this for initialization
    void Start ()
    {
        mainHiveTransform = HiveMain.m_Instance.gameObject.transform;
    }

    // Update is called once per frame
    void Update ()
    {
        if (buttonContainer.activeInHierarchy)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(mainHiveTransform.position);

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
