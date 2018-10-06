using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveMain : HiveBase
{
    public static HiveMain m_Instance;


    private void Awake()
    {
        if (m_Instance == null)
            m_Instance = this;
    }


}
