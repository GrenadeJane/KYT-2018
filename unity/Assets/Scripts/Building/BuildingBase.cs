using System;

using UnityEngine;
using UnityEngine.Events;

public class BuildingBase : MonoBehaviour
{
    public static Action<GameObject> OnBuildingClick;
    public UnityEvent ChangePosition;
    public UnityEvent FixPosition;
}
