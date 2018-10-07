using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BeeBase : SwarmObject {

	#region Constant

	protected float HARVEST_PER_SECOND = 0.05f;

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
				if(m_CurrentTargetedSpot != null)
				{
					WorldTargetPosition = m_CurrentTargetedSpot.transform.position;
					CurrentState = BeeState.GoToPollenSpot;
				}
				else
				{
					CurrentState = BeeState.Idle;
				}
			}
		}
	}

    public BeeAnimator beeAnimator;

	#endregion

	#region Methods

	private void Update()
	{
		switch(CurrentState)
		{
			case BeeState.Idle:
				{
					RotationDrivenBySwarm = true;
					break;
				}
			case BeeState.Moving:
				{
					RotationDrivenBySwarm = true;
					break;
				}
			case BeeState.GoToPollenSpot:
				{

					RotationDrivenBySwarm = false;


					transform.forward = m_Velocity.normalized;

					CurrentState = BeeState.Harvesting;
                    beeAnimator.Harvesting();

                    break;
				}
			case BeeState.Harvesting:
				{
					RotationDrivenBySwarm = false;
					transform.forward = m_Velocity.normalized;

					HarvestSpot();
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
				DrivenBySwarmMovement = true;

                beeAnimator.Idle();
            }
		}
	}

    #endregion
}
