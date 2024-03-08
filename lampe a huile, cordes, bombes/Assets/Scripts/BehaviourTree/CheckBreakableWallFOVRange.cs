using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class CheckBreakableWallFOVRange : BehaviourTree.Node
{
    private static int _wallLayerMask = 1 << 6;
    private Transform _transform;

    public CheckBreakableWallFOVRange(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");
        if (t == null)
        {
            Collider[] colliders = Physics.OverlapSphere(_transform.position, MorshuBT.fovRange, _wallLayerMask);
            if (colliders.Length > 0)
            {
                parent.SetData("target", colliders[0].transform);
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
