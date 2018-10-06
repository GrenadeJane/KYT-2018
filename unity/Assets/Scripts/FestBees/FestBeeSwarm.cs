using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FestBeeSwarm : SwarmBase<BeeBase>
{

	#region Fields

	private FlowersField m_FlowerField;

	private FlowerBase m_TargetFlower;

	#endregion

	#region Properties

	public FestBeesSwarmState State { get; private set; }
	public bool ComposedOfABob { get; set; }

	#endregion

	#region Methods

	protected override void Awake()
	{
		base.Awake();

		m_FlowerField = FindObjectOfType<FlowersField>();

		SearchTarget();
	}

	protected override void Update()
	{

		base.Update();

	}

	protected override void OnReachedTarget()
	{
		base.OnReachedTarget();
		switch (State)
		{
			case FestBeesSwarmState.Idle:
				{
					break;
				}
			case FestBeesSwarmState.GoToFlower:
				{
					break;
				}
			case FestBeesSwarmState.GoBackToHive:
				{
					break;
				}
			case FestBeesSwarmState.Harvesting:
				{
					break;
				}
		}
	}

	protected void SearchTarget()
	{
		m_TargetFlower = m_FlowerField.GetUntargetedFlower();
		if(m_TargetFlower != null)
		{
			TargetPosition = m_TargetFlower.transform.position;
			State = FestBeesSwarmState.GoToFlower;
			SetBeeState(BeeState.Moving);
		}
		else
		{
			State = FestBeesSwarmState.Idle;
			SetBeeState(BeeState.Idle);
		}

	}

	protected void HarvestCurrentTargetedFlower()
	{
		if(m_TargetFlower != null)
		{
			foreach(BeeBase bee in SwarmObjects)
			{
				bee.DrivenBySwarmMovement = false;
				bee.CurrentTargetedSpot = m_TargetFlower.GetUntargetedPollenHaverstingSpot();
			}
		}
	}

	protected void SetBeeState(BeeState state)
	{
		foreach(BeeBase bee in SwarmObjects)
		{
			bee.CurrentState = state;
		}
	}

	#endregion
}
