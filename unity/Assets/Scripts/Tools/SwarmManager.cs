using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmManager : MonoBehaviour {
   
    [SerializeField]
	private SwarmBase<SwarmObject> m_SwarmBasePrefab;
	[SerializeField]
	private SwarmObject m_SwarmObjectPrefab;
    [SerializeField]
    int _amountObject = 2;


    #region Runtime data

    SwarmBase<SwarmObject> swarm;
    public SwarmBase<SwarmObject> Swarm
    { get { return swarm; } }

    #endregion


    public void SetTargetPosition(Vector3 position)
    {
        swarm.TargetPosition = position;
    }
  

    private void Start()
	{
		swarm = Instantiate(m_SwarmBasePrefab, transform.position, Quaternion.identity, transform);
        swarm.TargetPosition = transform.position;
        
        for (int i = 0; i < _amountObject; i++)
        {
            swarm.AddSwarmObject(Instantiate(m_SwarmObjectPrefab));
        }
	}
}
