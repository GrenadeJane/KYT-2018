using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmObject : MonoBehaviour
{
	#region Constant 

	protected const float MOVEMENT_SPEED = 1.0f;

	#endregion

	#region Fields
	private bool m_ReachedTargetPosition = false;
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
			float movementSpeed = MOVEMENT_SPEED * Time.deltaTime;
			if (Vector3.Distance(transform.position, WorldTargetPosition) < movementSpeed)
			{
				transform.position = WorldTargetPosition;
				ReachedTargetPosition = true;
			}
			else
			{
				ReachedTargetPosition = false;
				Vector3 movementVector = Vector3.Normalize(WorldTargetPosition - transform.position);
				transform.position += (movementVector * movementSpeed);
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
