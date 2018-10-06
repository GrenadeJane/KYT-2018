using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveBase : MonoBehaviour {

	#region Constants
	private float SPAWN_RATE = 15.0f;
	#endregion

	#region Fields

	private float m_SpawnRate;

	[SerializeField]
	private FestBeeSwarm m_BeeSwarmPrefab;

	[SerializeField]
	private BeeBase m_BeePrefab;

	[SerializeField]
	private List<Transform> m_SwarmExits = new List<Transform>();


	private float m_NextSpawnTime = 0.0f;
	#endregion

	#region Properties

	#endregion

	#region Methods
	void Awake()
	{
		m_SpawnRate = SPAWN_RATE;
	}

	private void Update()
	{
		if (m_NextSpawnTime <= 0.0f)
		{
			GenerateBeeSwarm(3);
			m_NextSpawnTime = m_SpawnRate;
		}
		else
		{
			m_NextSpawnTime -= Time.deltaTime;
		}
	}

	/// <summary>
	/// Generate a bee swarm composed of a certain amount of bee
	/// </summary>
	public void GenerateBeeSwarm(int beeNumber)
	{
		
		Transform swarmExit = m_SwarmExits[Random.Range(0, m_SwarmExits.Count)];

		FestBeeSwarm swarm = Instantiate(m_BeeSwarmPrefab, swarmExit.position, Quaternion.identity);

		for (int i = 0; i < beeNumber; i++)
		{
			BeeBase bee = Instantiate(m_BeePrefab, swarm.transform.position, Quaternion.identity);
			swarm.AddSwarmObject(bee);
		}
	}
	#endregion
}
