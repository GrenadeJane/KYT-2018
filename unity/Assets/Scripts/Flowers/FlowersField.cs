using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FlowersField : MonoBehaviour {


	#region Fields

	[SerializeField]
	private List<FlowerBase> m_FlowerPrefabs = new List<FlowerBase>();

	[SerializeField]
	private List<FlowerSpawnSpot> m_FlowerSpawnSpots = new List<FlowerSpawnSpot>();
	#endregion

	#region Properties
	public List<FlowerBase> SpawnedFlowers { get; private set; }
	#endregion

	#region Methods


	private void Start()
	{
		m_FlowerSpawnSpots = FindObjectsOfType<FlowerSpawnSpot>().ToList();

		SpawnedFlowers = new List<FlowerBase>();

		for(int i = 0; i < 3; i++)
		{
			SpawnAFlower();
		}
	}

	private void Update()
	{
		for(int i = 0; i < SpawnedFlowers.Count; i++)
		{
			var flower = SpawnedFlowers[i];
			if (!flower.HasPollen)
			{
				var flowerSpawnSpots = m_FlowerSpawnSpots.Find(fss => fss.Flower == flower);
				flowerSpawnSpots.Flower = null;
				SpawnedFlowers.Remove(flower);

				Destroy(flower.gameObject);

				i--;
			}
		}
	}

	private void SpawnAFlower()
	{
		var freeFlowerSpawnSpots = m_FlowerSpawnSpots.Where(fss => fss.OccupiedByFlower == false);

		if(freeFlowerSpawnSpots.Count() > 0)
		{
			 var selectedFlowerSpawnSpot = freeFlowerSpawnSpots.ToList()[Random.Range(0, freeFlowerSpawnSpots.Count())];
			selectedFlowerSpawnSpot.OccupiedByFlower = true;

			selectedFlowerSpawnSpot.Flower = GetRandomFlowerInstance();
			SpawnedFlowers.Add(selectedFlowerSpawnSpot.Flower);
		}
	}

	private FlowerBase GetRandomFlowerInstance()
	{
		if (m_FlowerPrefabs.Count() > 0)
		{
			return Instantiate(m_FlowerPrefabs[Random.Range(0, m_FlowerPrefabs.Count())]);
		}
		else return null;
	}

	/// <summary>
	/// Returns a flower which is not the target of any swarm
	/// </summary>
	/// <returns>returns a random not target flower or null if there are no flowers available</returns>
	public FlowerBase GetUntargetedFlower()
	{
		FlowerBase flower = null;

		var untargetdFlowerList = SpawnedFlowers.FindAll(fss => fss.IsTargeted == false);

		if(untargetdFlowerList.Count > 0)
		{
			flower = untargetdFlowerList[Random.Range(0, untargetdFlowerList.Count)];
		}

		return flower;
		
	}

	#endregion

}
