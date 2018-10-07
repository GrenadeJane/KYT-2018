using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour {

    public void LoadGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void LoadMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
