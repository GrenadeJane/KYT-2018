﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

#region Types
public struct SwarmBaseData
{
    public Transform transform;
    public float radius;
	public Vector3 swarmDirection;
}

#endregion

public class SwarmBase<T> : MonoBehaviour where T : SwarmObject
{

    #region Constant 

    protected const float MOVEMENT_SPEED = 3.0f;
	public static float SWARM_RADIUS = 2.0f;
	#endregion

	#region Fields
	[SerializeField]
	public float m_SwarmRadius = 2.0f;

	protected bool m_Moving = false;

	protected Vector3 m_MovementVector;

	#endregion

	#region Properties
	public List<T> SwarmObjects { get; set; }

	public Vector3 TargetPosition { get; set; }

	public bool ReachedTarget { get; set; }
    #endregion

    #region Events
    public UnityAction OnTargetReached;

    #endregion
    #region Methods
    protected virtual void Awake()
	{
		SwarmObjects = new List<T>();
	}

	protected virtual void Update()
	{
		if(TargetPosition != transform.position)
		{
			ReachedTarget = false;
			MovingSwarm();
		}
		else
		{
			ReachedTarget = true;
		}


		UpdateSwarmObjects();

	}

	protected virtual void MovingSwarm()
	{
		// Moving the Swarm
		float movementSpeed = MOVEMENT_SPEED * Time.deltaTime;

		if (Vector3.Distance(transform.position, TargetPosition) < movementSpeed)
		{
			transform.position = TargetPosition;
			foreach (SwarmObject swarmObject in SwarmObjects)
			{
				swarmObject.StopMoving();
			}

			IsOnTarget();
			if (OnTargetReached != null)
			{
				OnTargetReached.Invoke();
				OnTargetReached = null;
			}

			ReachedTarget = true;
		}
		else
		{
			m_MovementVector = Vector3.Normalize(TargetPosition - transform.position);

			transform.position += (m_MovementVector * movementSpeed);
		}
	}

	protected virtual void UpdateSwarmObjects()
	{
		// Updating the swarm objects
		foreach (SwarmObject swarmObject in SwarmObjects)
		{
			swarmObject.UpdatePosition();
			swarmObject.swarmBaseData.swarmDirection = m_MovementVector.normalized;
		}
	}


	public  void AddSwarmObject(T swarmObject)
	{
		SwarmObjects.Add(swarmObject);
		swarmObject.transform.SetParent(transform, false);
		swarmObject.transform.localPosition = Vector3.zero;
		swarmObject.DrivenBySwarmMovement = true;
        swarmObject.swarmBaseData = new SwarmBaseData
        {
            transform = this.transform,
            radius = m_SwarmRadius
        };
    }


	public void RemoveSwarmObject(T swarmObject)
	{
		if (SwarmObjects.Contains(swarmObject))
		{
			swarmObject.DrivenBySwarmMovement = false;
			swarmObject.swarmBaseData.transform = null;
			swarmObject.transform.SetParent(null);
			SwarmObjects.Remove(swarmObject);
		}

	}

	#endregion

	protected virtual void IsOnTarget()
	{

	}

	#region Debug

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, m_SwarmRadius);
	}

	#endregion

}
