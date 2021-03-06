﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UI;

public class FestBeeSwarm : SwarmBase<BeeBase>
{
	#region Constants

	private const float TIME_BEFORE_RETURN = 5.0f;
	private static float CRITICAL_POLLEN_AMOUNT = 9.0f;
	public static float DRUNK_POLLEN_AMOUNT = CRITICAL_POLLEN_AMOUNT / 100 * 20;

    #endregion

    #region Fields

    private FlowersField m_FlowerField;

	private FlowerBase m_TargetFlower;

	private float m_TimeBeforeReturn;

	private BeeDyingStyle m_DyingStyle;

	protected AudioSource m_AudioSource;

	[SerializeField]
	private Sprite m_SuckImage;

	[SerializeField]
	private Sprite m_BreathOutImage;

	[SerializeField]
	private Sprite m_SuccessImage;

	[SerializeField]
	private Sprite m_FailedImage;

    [SerializeField]
    private Sprite m_DrunkImage;

    private GameObject m_UiCanvas;

	[SerializeField]
	private Image m_ActionImagePrefab;
	private Image m_ActionImage;
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

	public bool IsLost { get; private set; }
	#endregion


	#region Methods

	protected void Start()
	{
		IsLost = false;

		m_UiCanvas = GameObject.Find("UICanvas");

		if(m_UiCanvas != null)
		{
			m_ActionImage = Instantiate(m_ActionImagePrefab);
			m_ActionImage.transform.SetParent(m_UiCanvas.transform);
			m_ActionImage.enabled = false;
		}

		m_AudioSource = GetComponent<AudioSource>();

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

		m_ActionImage.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up);

		base.Update();
        switch (State)
		{
			case FestBeesSwarmState.MoveToAPosition:
				{
					if(ComposedOfAMaya)
					{
						m_TimeBeforeReturn -= Time.deltaTime;
						if (m_TimeBeforeReturn <= 0.0f)
						{
							GoToHive();
						}
					}
					else
					{
						SearchTarget();
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
					if(m_FlowerField != null && (m_TargetFlower == null || m_TargetFlower.IsTargeted))
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
					if(m_TargetFlower == null || !m_TargetFlower.HasPollen)
					{
						foreach (BeeBase bee in SwarmObjects)
						{
							bee.DrivenBySwarmMovement = true;
							if(bee.CurrentTargetedSpot != null)
							{
								bee.CurrentTargetedSpot.IsTargeted = false;
							}
							bee.CurrentTargetedSpot = null;
						}

                        if (TotalSwarmPollenAmount > DRUNK_POLLEN_AMOUNT && TotalSwarmPollenAmount <= CRITICAL_POLLEN_AMOUNT)
                        {
                            SetSwarmDrunk();
                        }

                        if (TotalSwarmPollenAmount > CRITICAL_POLLEN_AMOUNT)
						{
							SetupDeath();
						}
						else
						{
							SearchTarget();
						}

					}
					break;
				}
			case FestBeesSwarmState.GoingToDie:
				{

					break;
				}
		}
	}

	protected virtual void SetupDeath()
	{
		State = FestBeesSwarmState.GoingToDie;
		m_DyingStyle = (BeeDyingStyle)Random.Range(0, 2);
		
		if(m_DyingStyle == BeeDyingStyle.Misplace)
		{
			TargetPosition = m_FlowerField.GetPositionInGarden() + Vector3.up * 8.0f;
        }
		else if(m_DyingStyle == BeeDyingStyle.Overdose)
		{
			TargetPosition = m_FlowerField.GetPositionInGarden();
        }
    }

    protected virtual void SetSwarmDrunk()
    {
        SetBeeAnimatorDrunk();

        m_ActionImage.sprite = m_DrunkImage;
        m_ActionImage.enabled = true;
    }

    protected virtual void UpdateDeath()
	{

	}

	protected override void IsOnTarget()
	{
		base.IsOnTarget();
		switch (State)
		{
			case FestBeesSwarmState.Idle:
				{
					m_AudioSource.Play();
					break;
				}
			case FestBeesSwarmState.GoToFlower:
				{
					m_AudioSource.Play();
					m_TargetFlower.IsTargeted = true;
					HarvestCurrentTargetedFlower();
					State = FestBeesSwarmState.Harvesting;
					break;
				}
			case FestBeesSwarmState.GoBackToHive:
				{
					m_AudioSource.Play();
					HiveMain.m_Instance.BackToHive(this);    
					break;
				}
			case FestBeesSwarmState.Harvesting:
				{
					m_AudioSource.Stop();
					break;
				}

			case FestBeesSwarmState.MoveToAPosition:
				{
					m_AudioSource.Play();
					SearchPlaceToVisit();

					break;
				}

			case FestBeesSwarmState.GoingToDie:
				{
					m_AudioSource.Stop();

					HiveMain.m_Instance.TotalNumberOfBeeStillAlive -= SwarmObjects.Count;
					if (m_DyingStyle == BeeDyingStyle.Misplace)
					{
						IsLost = true;
					}
					else if(m_DyingStyle == BeeDyingStyle.Overdose)
					{
						while (SwarmObjects.Count > 0)
						{
							BeeBase beeBase = SwarmObjects[0];
							Rigidbody rigidbody = SwarmObjects[0].GetComponent<Rigidbody>();

							rigidbody.isKinematic = false;
							rigidbody.useGravity = true;

                            SetBeeAnimatorDeath();

                            RemoveSwarmObject(SwarmObjects[0]);

							Destroy(beeBase);
						}

						IsLost = true;
					}
					break;
				}
		}
	}

	protected void SearchTarget()
	{

		m_TargetFlower = m_FlowerField.GetUntargetedFlower();
		if (m_TargetFlower != null)
		{
			TargetPosition = m_TargetFlower.transform.position + (m_TargetFlower.transform.up.normalized * 2.2f);
			State = FestBeesSwarmState.GoToFlower;
			SetBeeState(BeeState.Moving);
		}
		else
		{
			State = FestBeesSwarmState.Idle;
			SetBeeState(BeeState.Idle);
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

    void SetBeeAnimatorDeath()
    {
        foreach (BeeBase bee in SwarmObjects)
        {
            bee.beeAnimator.Death();
        }

    }

    void SetBeeAnimatorDrunk()
    {
        foreach (BeeBase bee in SwarmObjects)
        {
            bee.beeAnimator.Drunk();
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
        TargetPosition = transform.position;
        m_ActionImage.sprite = m_SuckImage;
		m_ActionImage.enabled = true;

        CoroutineUtils.ExecuteWhenFinished(this, new WaitForSeconds(2.0f), () =>
		{
			m_ActionImage.sprite = m_BreathOutImage;
		});
	}

	public void SuccessPolitest()
	{
		m_ActionImage.sprite = m_SuccessImage;

		CoroutineUtils.ExecuteWhenFinished(this, new WaitForSeconds(3.0f), () =>
		{
			EndChecked();
			m_ActionImage.enabled = false;
		});
	}

	public void FailedPolitest()
	{
		m_ActionImage.sprite = m_FailedImage;

		CoroutineUtils.ExecuteWhenFinished(this, new WaitForSeconds(3.0f), () =>
		{
			GoToHive();
			m_ActionImage.enabled = false;
		});
	}
    public void EndChecked()
    {
        State = FestBeesSwarmState.Idle;
        m_TargetFlower = null;

    }


	public void PrepareToDestroy()
	{
		Destroy(m_ActionImage.gameObject);
	}
	#endregion

	#region Debug
	private void OnDrawGizmos()
	{
#if UNITY_EDITOR
        Handles.Label(transform.position, "Average Pollen	:" + TotalSwarmPollenAmount);
#endif
    }
#endregion
}
