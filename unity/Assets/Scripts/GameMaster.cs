using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    [SerializeField] HiveBase hiveBase;
    [SerializeField] CameraScroller cameraScroller;

    // Use this for initialization
    void Start ()
    {
        hiveBase = HiveMain.m_Instance;
        cameraScroller = Camera.main.GetComponent<CameraScroller>();

        hiveBase.enabled = false;
        cameraScroller.enabled = false;
    }
	
    public void StartGame()
    {
        hiveBase.enabled = true;
        cameraScroller.enabled = true;
    }

    public void ReloadGame()
    { }

}
