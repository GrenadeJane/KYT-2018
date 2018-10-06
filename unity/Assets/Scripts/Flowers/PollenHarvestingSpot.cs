using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollenHarvestingSpot : MonoBehaviour {

	#region Fields



	#endregion

	#region Properties

	public FlowerBase Flower { get; private set; }
	public bool IsTargeted { get; set; }

	#endregion

	#region Methods

	public void Initialize(FlowerBase flower)
	{
		if(Flower == null)
		{
			Flower = flower;
		}

	}

	public float HarvestPollen(float wantedQuantity)
	{
		return Flower.HarvestPollen(wantedQuantity);
	}
	#endregion

}
