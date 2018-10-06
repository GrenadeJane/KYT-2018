using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class SwarmBase<T> : MonoBehaviour where T : SwarmObject
{
	#region Constant 

	protected const float MOVEMENT_SPEED = 3.0f;

	#endregion

	#region Fields
	[SerializeField]
	public float m_SwarmRadius = 2.0f;

	protected bool m_Moving = false;

	#endregion

	#region Properties
	public List<T> SwarmObjects { get; set; }

	public Vector3 TargetPosition { get; set; }
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
		if(ReachedTarget())
		{
			m_Moving = false;
            if ( OnTargetReached != null )
            {
                OnTargetReached.Invoke();
                OnTargetReached = null;
            }
        }
		else
		{
			m_Moving = true;

			MovingSwarm();
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
			OnReachedTarget();
		}
		else
		{
			Vector3 movementVector = Vector3.Normalize(TargetPosition - transform.position);
			transform.position += (movementVector * movementSpeed);
		}
	}

	protected virtual void UpdateSwarmObjects()
	{
		// Updating the swarm objects
		foreach (SwarmObject swarmObject in SwarmObjects)
		{
			if (swarmObject.ReachedTargetPosition)
			{
				swarmObject.RelativeTargetPosition = UnityEngine.Random.insideUnitSphere * m_SwarmRadius;
			}
			swarmObject.UpdatePosition();
		}
	}

	private bool ReachedTarget()
	{
		return Vector3.Distance(transform.position, TargetPosition) < 0.1f;
	}

	public void AddSwarmObject(T swarmObject)
	{
		SwarmObjects.Add(swarmObject);
		swarmObject.transform.SetParent(transform);
		swarmObject.Swarm = this as SwarmBase<SwarmObject>;
		swarmObject.DrivenBySwarmMovement = true;
	}

	public void RemoveSwarmObject(T swarmObject)
	{
		if (SwarmObjects.Contains(swarmObject))
		{
			swarmObject.Swarm = null;
			swarmObject.transform.SetParent(null);
			SwarmObjects.Remove(swarmObject);
		}

	}

	#endregion

	protected virtual void OnReachedTarget()
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
