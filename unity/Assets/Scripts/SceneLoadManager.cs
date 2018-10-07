using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour {

    static SceneLoadManager m_instance;
    public static SceneLoadManager m_Instance
    {
        get {
                if ( m_instance != null )
                    return m_instance;
                else
            {
                GameObject obj = new GameObject();
                obj.name = "sceneloadermanager";
                return obj.AddComponent<SceneLoadManager>();
            }
        } 
    }

    public void LoadGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void LoadMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
    private void Awake()
    {
        if (m_instance == null)
            m_instance = this;
        else if (m_instance != this)
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start ()
    {
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
