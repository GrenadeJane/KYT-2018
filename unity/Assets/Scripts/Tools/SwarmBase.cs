using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwarmBase : MonoBehaviour
{
	#region Constant 

	protected const float MOVEMENT_SPEED = 3.0f;

	#endregion

	#region Fields
	[SerializeField]
	protected float m_SwarmRadius = 2.0f;

	protected bool m_Moving = false;
	#endregion

	#region Properties
	public List<SwarmObject> SwarmObjects { get; set; }

	public Vector3 TargetPosition { get; set; }
    #endregion

    #region Events 

    public UnityAction OnTargetReached;

    #endregion
    
    
    #region Methods
    protected void Awake()
	{
		SwarmObjects = new List<SwarmObject>();
	}

	protected void Update()
	{
		if(ReachedTarget())
		{
			m_Moving = false;
            if (OnTargetReached != null)
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
				swarmObject.RelativeTargetPosition = Random.insideUnitSphere * m_SwarmRadius;
			}
			swarmObject.UpdatePosition();
		}
	}

	private bool ReachedTarget()
	{
		return Vector3.Distance(transform.position, TargetPosition) < 0.1f;
	}

	public void AddSwarmObject(SwarmObject swarmObject)
	{
		SwarmObjects.Add(swarmObject);
		swarmObject.transform.SetParent(transform);
		swarmObject.Swarm = this;
	}

	public void RemoveSwarmObject(SwarmObject swarmObject)
	{
		if (SwarmObjects.Contains(swarmObject))
		{
			swarmObject.Swarm = null;
			swarmObject.transform.SetParent(null);
			SwarmObjects.Remove(swarmObject);
		}

	}

	#endregion

	#region Debug

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, m_SwarmRadius);
	}

	#endregion

}
