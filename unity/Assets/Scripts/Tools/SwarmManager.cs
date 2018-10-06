using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmManager : MonoBehaviour {

	[SerializeField]
	private SwarmBase m_SwarmBasePrefab;
	[SerializeField]
	private SwarmObject m_SwarmObjectPrefab;

	private void Start()
	{
		SwarmBase swarm = Instantiate(m_SwarmBasePrefab);

		for(int i = 0; i < 15; i++)
		{
			swarm.AddSwarmObject(Instantiate(m_SwarmObjectPrefab));
		}

		swarm.TargetPosition = Vector3.left * 100;
	}
}
