using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroller : MonoBehaviour {

	#region Fields

	[SerializeField]
	private Transform m_LeftBorder;
	[SerializeField]
	private Transform m_RightBorder;

	[SerializeField]
	private float m_BorderBoundary = 50.0f;

	[SerializeField]
	private float m_ScrollSpeed = 5.0f;

	private int m_ScreenWidth;

	#endregion

	#region Properties

	#endregion

	#region Methods

	private void Start()
	{
		m_ScreenWidth = Screen.width;
	}

	private void Update()
	{
		if (Input.mousePosition.x > m_ScreenWidth - m_BorderBoundary)
		{
			transform.position += Vector3.right * m_ScrollSpeed * Time.deltaTime;
			if(transform.position.x > m_RightBorder.position.x)
			{
				transform.position = new Vector3(m_RightBorder.position.x, transform.position.y, transform.position.z);
			}
		}

		if (Input.mousePosition.x < 0 + m_BorderBoundary)
		{
			transform.position += Vector3.left * m_ScrollSpeed * Time.deltaTime;
			if (transform.position.x < m_LeftBorder.position.x)
			{
				transform.position = new Vector3(m_LeftBorder.position.x, transform.position.y, transform.position.z);
			}
		}
	}

	#endregion
}
