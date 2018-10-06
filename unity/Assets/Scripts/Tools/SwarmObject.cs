using UnityEngine;
using UnityEngine.Events;

public class SwarmObject : MonoBehaviour
{
	#region Constant 

	protected const float MOVEMENT_SPEED = 1.0f;
	protected const float ACCELERATION = 0.2f;

	#endregion

	#region Fields
	private bool m_ReachedTargetPosition = false;
	private Vector3 m_Velocity = Vector3.zero;
	#endregion

	#region Properties
	public SwarmBase<SwarmObject> Swarm { get; set; }
	public bool ReachedTargetPosition { get; private set; }

	public Vector3 WorldTargetPosition
	{
		get { return RelativeTargetPosition + Swarm.transform.position; }
		set
		{
			RelativeTargetPosition = value - Swarm.transform.position;
		}
	}

	/// <summary>
	/// Target position relative to the swarm it comes from
	/// </summary>
	public Vector3 RelativeTargetPosition { get; set; }

	public bool DrivenBySwarmMovement { get; set; }

    public UnityAction<SwarmObject> OnTargetReached;

	#endregion

	#region Methods
	public void UpdatePosition()
	{
		if(Swarm != null)
		{
			if(DrivenBySwarmMovement)
			{
				SwarmMovement();
			}
			else
			{
				MoveToPosition();
			}
			
		}

	}

	public void SwarmMovement()
	{
		if (Vector3.Distance(transform.position, WorldTargetPosition) < MOVEMENT_SPEED * Time.deltaTime)
		{
			transform.position = WorldTargetPosition;
			ReachedTargetPosition = true;

			if (OnTargetReached != null)
			{
				OnTargetReached.Invoke(this);
				OnTargetReached = null;
			}
		}
		else
		{
			MoveToPosition();
		}
	}

	public void MoveToPosition()
	{
		ReachedTargetPosition = false;
		float movementAcceleration = ACCELERATION * Time.deltaTime;
		Vector3 movementVector = Vector3.Normalize(WorldTargetPosition - transform.position);
		m_Velocity += (movementVector * movementAcceleration);
		m_Velocity = Vector3.ClampMagnitude(m_Velocity, MOVEMENT_SPEED * Time.deltaTime);
		transform.position += m_Velocity;
	}

	public void StopMoving()
	{
		RelativeTargetPosition = Swarm.transform.position - transform.position;
		ReachedTargetPosition = true;
	}
	#endregion

	#region Debug

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(WorldTargetPosition, 0.1f);
	}

	#endregion
}
