using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbianceManager : MonoBehaviour {

	[SerializeField]
	private AudioSource m_DiscoHiveSource;

	[SerializeField]
	private float m_DiscoHiveAudioRadius;

	private float m_DiscoHiveVolume;
	private float m_GlobalMusicVolume;

	[SerializeField]
	private AudioSource m_GlobalMusicSource;

	private void Awake()
	{
		m_DiscoHiveVolume = m_DiscoHiveSource.volume;
		m_GlobalMusicVolume = m_GlobalMusicSource.volume;
	}

	private void Update()
	{
		float distanceFromDiscoHive = Vector3.Distance(Camera.main.transform.position, m_DiscoHiveSource.transform.position);

		float volumeRatio = Mathf.Clamp01((m_DiscoHiveAudioRadius - distanceFromDiscoHive) / m_DiscoHiveAudioRadius);


		float discoHiveRatioVolume = volumeRatio;
		float globalMusicRatioVolume = 1 - volumeRatio;

		m_DiscoHiveSource.volume = m_DiscoHiveVolume * discoHiveRatioVolume;
		m_GlobalMusicSource.volume = m_GlobalMusicVolume * globalMusicRatioVolume;
	}

	#region Debug

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(m_DiscoHiveSource.transform.position, m_DiscoHiveAudioRadius);
	}

	#endregion

}
