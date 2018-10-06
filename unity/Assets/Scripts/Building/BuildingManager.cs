﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] GameObject copZonePrefab;
    [SerializeField] GameObject copHive;


    List<GameObject> copZoneList = new List<GameObject>();


    GameObject currentObject;
    BuildingBase currentBuilding;

    private void Awake()
    {
        BuildingBase.OnBuildingClick += OnBuildClick;
    }

    public void CreateCopZone()
    {
        GameObject obj = Instantiate(copZonePrefab);
        copZoneList.Add(obj);
        currentObject = obj;
        currentBuilding = currentObject.GetComponent<BuildingBase>();
        currentBuilding.ChangePosition.Invoke();
    }

    public void CreateCopHive()
    {
        GameObject obj = Instantiate(copHive);
        copZoneList.Add(obj);
        currentObject = obj;
        currentBuilding = currentObject.GetComponent<BuildingBase>();
        currentBuilding.ChangePosition.Invoke();
    }

    public void OnBuildClick(GameObject obj)
    {
        if ( currentObject == null )
        {
            currentObject = obj;
            currentBuilding = currentObject.GetComponent<BuildingBase>();
            currentBuilding.ChangePosition.Invoke();
        }
        else
        {
            currentBuilding.FixPosition.Invoke();
            currentObject = null;
            currentBuilding = null;
        }
    }


    void Update()
    {
        if (currentObject != null )
        {
            Vector3 cursorPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
            Vector3 pos = Camera.main.ScreenToWorldPoint(cursorPos);
            currentBuilding.CheckPosition.Invoke(pos);
        }
    }

}
