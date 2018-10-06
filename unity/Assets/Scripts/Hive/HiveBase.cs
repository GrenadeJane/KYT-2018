﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveBase : MonoBehaviour {

    #region Constants
    [SerializeField] float SPAWN_RATE = 15.0f;
	#endregion

	#region Fields


	private float m_SpawnRate;

	[SerializeField]
	private FestBeeSwarm m_BeeSwarmPrefab;

	[SerializeField]
	private BeeBase m_BeePrefab;
	[SerializeField]
	private MayaBee m_MayaBeePrefab;

	[SerializeField]
	private List<Transform> m_SwarmExits = new List<Transform>();

    public List<FestBeeSwarm> m_festbeeSwarmList = new List<FestBeeSwarm>();

    private float m_NextSpawnTime = 0.0f;
	#endregion

	#region Properties

	#endregion

	#region Methods
	void Awake()
	{
		m_SpawnRate = SPAWN_RATE;
	}

	protected virtual void  Update()
	{
		if (m_NextSpawnTime <= 0.0f)
		{
			GenerateBeeSwarm(3);
			m_NextSpawnTime = SPAWN_RATE;
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
		GenerateMayaBeeSwarm(beeNumber);

		//if (Random.Range(0,4) == 1)
		//{
		//	GenerateMayaBeeSwarm(beeNumber);
		//}
		//else
		//{
		//	GenerateFestBeeSwarm(beeNumber);
		//}
	}

	public void GenerateFestBeeSwarm(int beeNumber)
	{
		Transform swarmExit = m_SwarmExits[Random.Range(0, m_SwarmExits.Count)];
		FestBeeSwarm swarm = Instantiate(m_BeeSwarmPrefab, swarmExit.position, Quaternion.identity);

		m_festbeeSwarmList.Add(swarm);

		for (int i = 0; i < beeNumber; i++)
		{
			BeeBase bee = Instantiate(m_BeePrefab, swarm.transform.position, Quaternion.identity);
			swarm.AddSwarmObject(bee);
		}
	}

	public void GenerateMayaBeeSwarm(int beeNumber)
	{
		Transform swarmExit = m_SwarmExits[Random.Range(0, m_SwarmExits.Count)];
		FestBeeSwarm swarm = Instantiate(m_BeeSwarmPrefab, swarmExit.position, Quaternion.identity);
		swarm.ComposedOfAMaya = true;

		m_festbeeSwarmList.Add(swarm);

		MayaBee mayaBee = Instantiate(m_MayaBeePrefab, swarm.transform.position, Quaternion.identity);
		swarm.AddSwarmObject(mayaBee);

		for (int i = 0; i < beeNumber - 1; i++)
		{
			BeeBase bee = Instantiate(m_BeePrefab, swarm.transform.position, Quaternion.identity);
			swarm.AddSwarmObject(bee);
		}
	}

	public void BackToHive(FestBeeSwarm swarm)
	{
		m_festbeeSwarmList.Remove(swarm);
		Destroy(swarm.gameObject);
	}
	#endregion
}
