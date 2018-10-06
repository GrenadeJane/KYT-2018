using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BeeBase : SwarmObject {

	#region Constant

	protected float HARVEST_PER_SECOND = 0.2f;

	protected float CRITICAL_THRESHOLD = 4.0f;

	#endregion

	#region Fields

	PollenHarvestingSpot m_CurrentTargetedSpot;

	#endregion

	#region Properties

	public float CurrentPollenAmount { get; set; }

	public BeeState CurrentState { get; set; }

	public PollenHarvestingSpot CurrentTargetedSpot
	{
		get { return m_CurrentTargetedSpot; }
		set
		{
			if(m_CurrentTargetedSpot != value)
			{
				m_CurrentTargetedSpot = value;
				WorldTargetPosition = m_CurrentTargetedSpot.transform.position;
				CurrentState = BeeState.GoToPollenSpot;
			}
		}
	}

	#endregion

	#region Methods

	private void Update()
	{
		switch(CurrentState)
		{
			case BeeState.Idle:
				{
					break;
				}
			case BeeState.Moving:
				{
					break;
				}
			case BeeState.GoToPollenSpot:
				{
					CurrentState = BeeState.Harvesting;
					break;
				}
			case BeeState.Harvesting:
				{
					break;
				}
		}
	}

	protected void HarvestSpot()
	{
		if(CurrentTargetedSpot != null)
		{
			var pollenAmountAvailable = CurrentTargetedSpot.HarvestPollen(HARVEST_PER_SECOND * Time.deltaTime);
			if(pollenAmountAvailable > 0.0f)
			{
				CurrentPollenAmount += pollenAmountAvailable;
			}
			else
			{
				CurrentState = BeeState.Idle;
				CurrentTargetedSpot.IsTargeted = false;
			}
		}
	}

    #endregion
}
