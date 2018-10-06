using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] GameObject copZonePrefab;


    List<GameObject> copZoneList = new List<GameObject>();


    GameObject currentObject;

    private void Awake()
    {
        CopsZone.OnBuildingClick += OnBuildClick;
    }

    public void CreateCopZone()
    {
        GameObject obj = Instantiate(copZonePrefab);
        copZoneList.Add(obj);
        currentObject = obj;
        currentObject.GetComponent<IBuilding>().ChangePosition();
    }

    public void OnBuildClick(GameObject obj)
    {
        if ( currentObject == null )
        {
            currentObject = obj;
            currentObject.GetComponent<IBuilding>().ChangePosition();
        }
        else
        {
            currentObject.GetComponent<IBuilding>().FixPosition();
            currentObject = null;
        }
    }


    void Update()
    {
        if (currentObject != null )
        {
            Vector3 cursorPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
            Vector3 pos = Camera.main.ScreenToWorldPoint(cursorPos);

            currentObject.transform.position = pos;
        }
    }

}
