using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMaster : MonoBehaviour {

	public void LoadGame()
    {
        SceneLoadManager.m_Instance.LoadGame();
    }
}
