using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmObjectCop : SwarmObject
{


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
    #endregion

    private void Start()
    {
        GoesToUniqueTarget = false;
    }

    public void Alert()
    {
        audio.Play();
    }

    public void EndAlert()
    {
        audio.Stop();
    }

    public void AssignNewTarget(Vector3 position, Vector3 dir )
    {
        animator.SetTrigger("Alert");

        WorldTargetPosition = position;
        GoesToUniqueTarget = true;

        transform.forward = dir;
    }
}
