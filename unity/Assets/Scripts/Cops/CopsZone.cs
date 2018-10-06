using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

[Serializable]
public class CopsZone : MonoBehaviour, IPointerClickHandler
{

    #region Types
    enum CopState
    {
        None = -1,
        Idle = 0,
        GoToHive,
        Placement,
        JustPlaced
    }

    #endregion
    
    #region Events



    #endregion

    #region Parameters

    [SerializeField] float startRange;
    [SerializeField] float timeCheck;
    [SerializeField] float timeToReload;
    [SerializeField] int baseCountCops;
    [SerializeField] int basePoliTest = 10;
    [SerializeField] float legalAlcoholAmount;


    [SerializeField] SphereCollider collider;
    [SerializeField] SwarmBaseCop copSwarmPrefab;
    [SerializeField] SwarmObjectCop copPrefab;

    #endregion

    #region RuntimeData

    List<FestBeeSwarm> beeListChecking = new List<FestBeeSwarm>();
    SwarmBaseCop currentSwarm;
    CopState state = CopState.None;
    int _countPoliTest;

    #endregion

    #region Events
 
    [SerializeField] public static Action<GameObject> OnBuildingClick;

    #endregion



    /// <summary>
    ///  Check If a bee // swarmBase enter into the range of the cops
    /// </summary>
    void CheckEnterZone()
    {
        float rangeSqr = startRange;
        foreach (FestBeeSwarm swarm in HiveMain.m_Instance.m_festbeeSwarmList)
        {
            if (swarm.State == FestBeesSwarmState.BeenChecked)
                continue;

            float rangeSqrSwarm = swarm.m_SwarmRadius ;
            Vector3 beePos = swarm.transform.position;
            Vector3 dir = (beePos -currentSwarm.transform.position);

            float dis = dir.magnitude;
            bool ischecked = beeListChecking.Contains(swarm);

            if (dis < ( rangeSqr + rangeSqrSwarm ))
            {
                if ( !ischecked && _countPoliTest > 0)
                {
                    beeListChecking.Add(swarm);
                    swarm.IsChecked();

                    // :: lost time when go checked to a maya even if we don't loose a polinotest
                    if (!swarm.ComposedOfAMaya)
                        _countPoliTest--;

                    SendCopToPlace(swarm, currentSwarm.transform.position + dir.normalized *(dis - rangeSqrSwarm));
                }
               

            } else if (ischecked)
            {
                beeListChecking.Remove(swarm);
            }
        }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="swarm"></param>
    /// <param name="targetPos"></param>
    void SendCopToPlace(FestBeeSwarm swarm, Vector3 targetPos)
    {
        SwarmObjectCop cop = currentSwarm.GetRandomnlyObject();
        if ( cop != null )
        {
            cop.AssignNewTarget(targetPos);
            cop.SwarmTarget = swarm;

            cop.OnTargetReached = null;
            cop.OnTargetReached += ProceedToCheck;
        }
    }




    /// <summary>
    /// Get the amount of alcool in the swarm + stop them to move
    /// </summary>
    void ProceedToCheck(SwarmObject swarmObject) // param current swarm
    {

        SwarmObjectCop cop = swarmObject as SwarmObjectCop;

        if (cop.SwarmTarget.ComposedOfAMaya)
        {
            cop.GoesToUniqueTarget = false;
            cop.SwarmTarget.EndChecked(); 
        }
        else 
            CoroutineUtils.ExecuteWhenFinished(this, new WaitForSeconds(timeCheck), () =>
            {
                cop.GoesToUniqueTarget = false;

                float alcoholAmount = cop.SwarmTarget.TotalSwarmPollenAmount;

                if (alcoholAmount >= legalAlcoholAmount)
                    cop.SwarmTarget.GoToHive();
                else
                    cop.SwarmTarget.EndChecked();

                // send back to home if needed
                if (_countPoliTest <= 0 && state != CopState.GoToHive)
                    GoToHive();
            });
    }




    /// <summary>
    /// Send the swarmbase to the main hive
    /// </summary>
    void GoToHive()
    {
        state = CopState.GoToHive;
        currentSwarm.TargetPosition =  HiveMain.m_Instance.gameObject.transform.position;
        currentSwarm.OnTargetReached = null;
        currentSwarm.OnTargetReached = ReturnFromHive;
    }


    /// <summary>
    /// 
    /// </summary>
    void ReturnFromHive()
    {
        CoroutineUtils.ExecuteWhenFinished(this, new WaitForSeconds(timeToReload), () =>
        {
            _countPoliTest = basePoliTest;
            currentSwarm.TargetPosition = transform.position;
            currentSwarm.OnTargetReached = null;
            currentSwarm.OnTargetReached = () => { state = CopState.Idle; };
        });
    }


    #region MonoBehaviour

    void Start ()
    {
        _countPoliTest = basePoliTest;
        collider.radius = startRange;

        GenerateCopSwarm();
    }
    

    void Update ()
    {
        switch( state )
        {
            case CopState.Idle :
                {
                    CheckEnterZone();
                }
                break;
            case CopState.GoToHive:
                {

                }
                break;
        }
    }

    void GenerateCopSwarm()
    {
        Transform swarmExit = HiveMain.m_Instance.gameObject.transform;

        currentSwarm = Instantiate(copSwarmPrefab, swarmExit.position, Quaternion.identity);
        currentSwarm.m_SwarmRadius = startRange;

        for (int i = 0; i < baseCountCops; i++)
        {
            SwarmObjectCop cop = Instantiate(copPrefab, currentSwarm.transform, false);
            cop.transform.position = swarmExit.position;

            currentSwarm.AddSwarmObject(cop);
        }

        currentSwarm.TargetPosition = HiveMain.m_Instance.gameObject.transform.position;
    }

    public void FixPosition()
    {
        currentSwarm.TargetPosition = transform.position;
        state = CopState.JustPlaced;
        currentSwarm.OnTargetReached += () =>
        {
            state = CopState.Idle;
            collider.enabled = true;
        };
    }

    public void ChangePosition()
    {
        state = CopState.Placement;
        collider.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if ( currentSwarm.ReachedTarget )
            BuildingBase.OnBuildingClick.Invoke(gameObject);
    }

    #endregion
}
