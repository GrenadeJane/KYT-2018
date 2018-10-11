using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmObjectCop : SwarmObject
{
    enum StateCops
    {
        None =-1,
        Alert
    }

    #region Properties

    bool _goesToUniqueTarget;
    public bool GoesToUniqueTarget
    {
       get { return _goesToUniqueTarget;  }     
       set
        {
            bool previousState = _goesToUniqueTarget;
            _goesToUniqueTarget = value;
            if (previousState != value)
            {
                RotationDrivenBySwarm = !value;
                DrivenBySwarmMovement = !value;
            }
            if (!value)
                animator.SetTrigger("Idle");
        }
    }

    public FestBeeSwarm SwarmTarget;
    public AudioSource audio;
    public Animator animator;
    [SerializeField] GameObject alarmObject;

    StateCops state;
    #endregion

    private void Start()
    {
        GoesToUniqueTarget = false;
    }

    public void Alert()
    {
        audio.Play();
        animator.SetTrigger("Alert");

        state = StateCops.Alert;
    }

    public void EndAlert()
    {
        state = StateCops.None;

        audio.Stop();
    }

    public void AssignNewTarget(Vector3 position, Vector3 dir )
    {

        WorldTargetPosition = position;
        GoesToUniqueTarget = true;

        transform.forward = dir;
    }
    float x = 0;

    public void Update()
    {
        switch (state) 
        {
            case StateCops.Alert:
                x += Time.deltaTime * 2;
                alarmObject.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2( x, 0));
                break;

            default:
                break;
        }
    }
}
