using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using BehaviourTree;
public class CheckBombInFOVRange : BehaviourTree.Node
{

    private Transform _transform;

    public CheckBombInFOVRange(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");
        if (t== null)
        {
            Collider[] colliders = Physics.OverlapSphere(_transform.position, MorshuBT.fovRange);
            if (colliders.Length > 0)
            {
                parent.parent.SetData("target", colliders[0].transform);
                state = NodeState.SUCCESS;
                return state;
            }
            state = NodeState.FAILURE; 
            return state;
        }
        state = NodeState.SUCCESS;
        return state;
    }
}
