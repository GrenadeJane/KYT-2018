using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmObject : MonoBehaviour
{
	#region Constant 

	protected const float MOVEMENT_SPEED = 1.0f;
	protected const float ACCELERATION = 0.1f;

	#endregion

	#region Fields
	private bool m_ReachedTargetPosition = false;
	private Vector3 m_Velocity = Vector3.zero;
	#endregion

	#region Properties
	public SwarmBase Swarm { get; set; }
	public bool ReachedTargetPosition { get; private set; }

	public Vector3 WorldTargetPosition
	{
		get { return RelativeTargetPosition + Swarm.transform.position; }
	}

	/// <summary>
	/// Target position relative to the swarm it comes from
	/// </summary>
	public Vector3 RelativeTargetPosition { get; set; }
	#endregion

	#region Methods
	public void UpdatePosition()
	{
		if(Swarm != null)
		{

			if (Vector3.Distance(transform.position, WorldTargetPosition) < MOVEMENT_SPEED * Time.deltaTime)
			{
				transform.position = WorldTargetPosition;
				ReachedTargetPosition = true;
			}
			else
			{
				ReachedTargetPosition = false;
				float movementAcceleration = ACCELERATION * Time.deltaTime;
				Vector3 movementVector = Vector3.Normalize(WorldTargetPosition - transform.position);
				m_Velocity += (movementVector * movementAcceleration);
				m_Velocity = Vector3.ClampMagnitude(m_Velocity, MOVEMENT_SPEED * Time.deltaTime);
				transform.position += m_Velocity;
			}
		}

	}

	public void StopMoving()
	{
		RelativeTargetPosition = Swarm.transform.position - transform.position;
		ReachedTargetPosition = true;
	}
	#endregion
}
