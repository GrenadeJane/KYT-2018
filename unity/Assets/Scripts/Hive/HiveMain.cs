using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveMain : HiveBase
{
    public static HiveMain m_Instance;
     List<FestBeeSwarm> m_festbeeSwarmList = new List<FestBeeSwarm>();

    private void Awake()
    {
        if (m_Instance == null)
            m_Instance = this;
    }
}
