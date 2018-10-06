using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
		switch (State)
		{
			case FestBeesSwarmState.Idle:
				{
					break;
				}
			case FestBeesSwarmState.GoToFlower:
				{
					if(m_FlowerField != null && m_TargetFlower.IsTargeted)
					{
						SearchTarget();
					}
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

	protected override void IsOnTarget()
	{
		base.IsOnTarget();
		switch (State)
		{
			case FestBeesSwarmState.Idle:
				{
					break;
				}
			case FestBeesSwarmState.GoToFlower:
				{
					HarvestCurrentTargetedFlower();
					State = FestBeesSwarmState.Harvesting;
					break;
				}
			case FestBeesSwarmState.GoBackToHive:
				{
                  //  HiveBase.OnSwarmComeBack(this);
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
		//m_TargetFlower = m_FlowerField.GetUntargetedFlower();
		if(m_TargetFlower != null)
		{
			TargetPosition = m_TargetFlower.transform.position;
			State = FestBeesSwarmState.GoToFlower;
			SetBeeState(BeeState.Moving);
		}
		else
		{
			TargetPosition = transform.position;
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
				bee.StopMoving();
				bee.CurrentTargetedSpot = m_TargetFlower.GetUntargetedPollenHaverstingSpot();
				bee.CurrentTargetedSpot.IsTargeted = true;
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

    public void GoToHive()
    {
        State = FestBeesSwarmState.GoBackToHive;
        TargetPosition = HiveMain.m_Instance.transform.position;
        OnTargetReached = null;
    }

    public void IsChecked()
    {
        State = FestBeesSwarmState.BeenChecked;
    }

    public void EndChecked()
    {
        State = FestBeesSwarmState.Idle;
    }

    #endregion
}
