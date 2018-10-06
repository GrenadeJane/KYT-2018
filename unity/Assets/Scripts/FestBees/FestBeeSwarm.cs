using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FestBeeSwarm : SwarmBase<BeeBase>
{
	#region Constants

	private const float TIME_BEFORE_RETURN = 5.0f;

	#endregion

	#region Fields

	private FlowersField m_FlowerField;

	private FlowerBase m_TargetFlower;

	private float m_TimeBeforeReturn;

	#endregion

	#region Properties

	public FestBeesSwarmState State { get; private set; }
	public bool ComposedOfAMaya { get; set; }

	public float TotalSwarmPollenAmount
	{
		get
		{
			float totalPollenAmount = 0.0f;
			foreach(BeeBase bee in SwarmObjects)
			{
				totalPollenAmount += bee.CurrentPollenAmount;
			}
			return totalPollenAmount;
		}
	}
	public float AverageSwarmPollenAmount
	{
		get
		{
			float totalPollenAmount = 0.0f;
			foreach (BeeBase bee in SwarmObjects)
			{
				totalPollenAmount += bee.CurrentPollenAmount;
			}
			return totalPollenAmount / (SwarmObjects.Count);
		}
	}
	#endregion


	#region Methods

	protected void Start()
	{

		State = FestBeesSwarmState.Idle;
		m_FlowerField = FindObjectOfType<FlowersField>();

		if(ComposedOfAMaya)
		{
			m_TimeBeforeReturn = TIME_BEFORE_RETURN;
		}
		else
		{
			SearchTarget();
		}

	}

	protected override void Update()
	{
        if ( State != FestBeesSwarmState.BeenChecked)
		    base.Update();
		switch (State)
		{
			case FestBeesSwarmState.MoveToAPosition:
				{
					m_TimeBeforeReturn -= Time.deltaTime;
					if(m_TimeBeforeReturn <= 0.0f)
					{
						GoToHive();
					}
					break;
				}
			case FestBeesSwarmState.Idle:
				{
					if(ComposedOfAMaya)
					{
						SearchPlaceToVisit();
					}
					else
					{
						if (m_FlowerField != null && m_TargetFlower == null)
						{
							SearchTarget();
						}
					}

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
					// Checking if the flower has still pollen, if not 
					if(!m_TargetFlower.HasPollen)
					{
						foreach (BeeBase bee in SwarmObjects)
						{
							bee.DrivenBySwarmMovement = true;
							bee.CurrentTargetedSpot.IsTargeted = false;
							bee.CurrentTargetedSpot = null;
						}
						SearchTarget();
					}
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
					m_TargetFlower.IsTargeted = true;
					HarvestCurrentTargetedFlower();
					State = FestBeesSwarmState.Harvesting;
					break;
				}
			case FestBeesSwarmState.GoBackToHive:
				{
					HiveMain.m_Instance.BackToHive(this);    
					break;
				}
			case FestBeesSwarmState.Harvesting:
				{
					break;
				}

			case FestBeesSwarmState.MoveToAPosition:
				{

					SearchPlaceToVisit();

					break;
				}
		}
	}

	protected void SearchTarget()
	{

	//	m_TargetFlower = m_FlowerField.GetUntargetedFlower();
		if (m_TargetFlower != null)
		{
			TargetPosition = m_TargetFlower.transform.position + (m_TargetFlower.transform.up.normalized * 2.2f);
			State = FestBeesSwarmState.GoToFlower;
			SetBeeState(BeeState.Moving);
		}


	}

	protected void SearchPlaceToVisit()
	{
		TargetPosition = m_FlowerField.GetPositionInGarden();
		State = FestBeesSwarmState.MoveToAPosition;
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
