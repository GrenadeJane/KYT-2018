using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreatorManager : MonoBehaviour
{
    [SerializeField] GameObject copZonePrefab;


    List<GameObject> copZoneList = new List<GameObject>();


    GameObject currentObject;

    public void CreateCopZone()
    {
        GameObject obj = Instantiate(copZonePrefab);
        copZoneList.Add(obj);
        currentObject = obj;
      //   copZonePrefab

    }


    void Update()
    {
        if (currentObject != null )
        {
            if ( Input.GetMouseButtonDown(0))
            {
                currentObject.GetComponent<IBuilding>().FixPosition();
                currentObject = null;
            }
            else
            {
                Vector3 cursorPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
                Vector3 pos = Camera.main.ScreenToWorldPoint(cursorPos);

                currentObject.transform.position = pos;
            }
        }
    }
}
