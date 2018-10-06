using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopsSwarmManager : MonoBehaviour {


    [SerializeField]
    private SwarmBaseCop m_SwarmBasePrefab;
    [SerializeField]
    private SwarmObjectCop m_SwarmObjectPrefab;
    [SerializeField]
    int _amountObject = 2;


    #region Runtime data

    SwarmBaseCop swarm;
    public SwarmBaseCop Swarm
    { get { return swarm; } }

    #endregion


    public void SetTargetPosition(Vector3 position)
    {
        swarm.TargetPosition = position;
    }

    private void Awake()
    {
        swarm = Instantiate(m_SwarmBasePrefab, transform.position, Quaternion.identity, transform);
    }

    private void Start()
    {
        swarm.TargetPosition = transform.position;

        for (int i = 0; i < _amountObject; i++)
        {
            swarm.AddSwarmObject(Instantiate(m_SwarmObjectPrefab));
        }
    }
}


