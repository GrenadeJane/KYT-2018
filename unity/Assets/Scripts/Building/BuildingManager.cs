using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{

    public static BuildingManager m_instance;
    [SerializeField] GameObject copZonePrefab;
    [SerializeField] GameObject copHive;


    public List<GameObject> copZoneList = new List<GameObject>();
    public List<GameObject> copHiveList = new List<GameObject> ();


    GameObject currentObject;
    BuildingBase currentBuilding;

    int _countAvailableZone = 1;
    int CountAvailableZone
    {
        get { return _countAvailableZone; }
        set
        {
            GetComponent<UIManager>().SetCountZone(value);
            _countAvailableZone = value;
        }
    }
    int _countAvailableHive = 1;
    int CountAvailableHive
    {
        get { return _countAvailableHive; }
        set
        {
            GetComponent<UIManager>().SetCountHive(value);
            _countAvailableHive = value;
        }
    }

    void Start()
    {
        CountAvailableHive = _countAvailableHive;
        CountAvailableZone = _countAvailableZone;
    }

    private void Awake()
    {
        if (m_instance == null)
            m_instance = this;

        BuildingBase.OnBuildingClick += OnBuildClick;

    }

    public void CreateCopZone()
    {
        GameObject obj = Instantiate(copZonePrefab);
        copZoneList.Add(obj);
        currentObject = obj;
        currentBuilding = currentObject.GetComponent<BuildingBase>();
        currentBuilding.ChangePosition.Invoke();
        CountAvailableZone--;
    }

    public void CreateCopHive()
    {
        GameObject obj = Instantiate(copHive);
        copZoneList.Add(obj);
        currentObject = obj;
        currentBuilding = currentObject.GetComponent<BuildingBase>();
        currentBuilding.ChangePosition.Invoke();
        copHiveList.Add(currentObject);

        CountAvailableHive--;
    }

    public void OnBuildClick(GameObject obj)
    {
        if (currentObject == null)
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
        if (currentObject != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            currentBuilding.CheckPosition.Invoke(ray);


        }
    }

}
