using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeBase : SwarmObject {

	#region Constant

	protected float HARVEST_PER_SECOND = 0.2f;

	#endregion

	#region Fields



	#endregion

	#region Properties

	public float CurrentPollenAmount { get; set; }
	public PollenHarvestingSpot CurrentTargetedSpot { get; set; }

	#endregion

	#region Methods

	private void Update()
	{
		
	}

	protected void HarvestSpot()
	{
		if(CurrentTargetedSpot != null)
		{
			CurrentPollenAmount += CurrentTargetedSpot.HarvestPollen(HARVEST_PER_SECOND * Time.deltaTime);
		}
	}

	#endregion
}
