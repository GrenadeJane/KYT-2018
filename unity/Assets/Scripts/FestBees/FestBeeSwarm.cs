using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FestBeeSwarm : SwarmBase
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
		TargetPosition = m_TargetFlower.transform.position;
	}

	protected void HarvestCurrentTargetedFlower()
	{
		if(m_TargetFlower != null)
		{
			foreach(SwarmObject bee in SwarmObjects)
			{
				m_TargetFlower.GetUntargetedPollenHaverstingSpot();
			}
		}
	}

	#endregion
}
