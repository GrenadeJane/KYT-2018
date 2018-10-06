using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerBase : MonoBehaviour {

	#region Constants

	protected float POLLEN_QUANTITY = 100.0f;

	#endregion

	#region Fields

	[SerializeField]
	private List<PollenHarvestingSpot> m_PollenHarvestingSpot = new List<PollenHarvestingSpot>();

	private float m_PollenQuantity;

	#endregion

	#region Properties

	public FestBeeSwarm HaversterBees { get; set; }

	public bool IsTargeted { get; set; }
	public bool HasPollen
	{
		get { return PollenQuantity > 0.0f; }
	}

	public float PollenQuantity
	{
		get { return m_PollenQuantity; }
	}

	#endregion

	#region Methods

	private void Awake()
	{
		m_PollenQuantity = POLLEN_QUANTITY;

		foreach (PollenHarvestingSpot pollenHarvestingSpot in m_PollenHarvestingSpot)
		{
			pollenHarvestingSpot.Initialize(this);
		}
	}


	/// <summary>
	/// Returns a pollen harvesting spot which is not the target of any bee
	/// </summary>
	/// <returns>returns a random not targeted pollen harvesting spot or null if there are no spot available</returns>
	public PollenHarvestingSpot GetUntargetedPollenHaverstingSpot()
	{
		PollenHarvestingSpot pollenHaverstingSpot = null;

		var untargetdSpotList = m_PollenHarvestingSpot.FindAll(f => f.IsTargeted == false);

		if (untargetdSpotList.Count > 0)
		{
			pollenHaverstingSpot = untargetdSpotList[Random.Range(0, untargetdSpotList.Count)];
		}

		return pollenHaverstingSpot;

	}

	public float HarvestPollen(float wantedQuantity)
	{
		float possiblePollenQuantity = 0.0f;
		if (m_PollenQuantity < wantedQuantity)
		{
			possiblePollenQuantity = m_PollenQuantity;
			m_PollenQuantity = 0;
		}
		else
		{
			possiblePollenQuantity = wantedQuantity;
		}

		return possiblePollenQuantity;
	}

	#endregion
}
