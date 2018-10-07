using System;

using UnityEngine;
using UnityEngine.Events;

public class BuildingBase : MonoBehaviour
{
    [Serializable]
    public class UnityEvent_Ray : UnityEvent<Ray> { };

    public float heightCollider;
    public static Action<GameObject> OnBuildingClick;
    public UnityEvent ChangePosition;
    public UnityEvent FixPosition;
    public UnityEvent_Ray CheckPosition;
}
