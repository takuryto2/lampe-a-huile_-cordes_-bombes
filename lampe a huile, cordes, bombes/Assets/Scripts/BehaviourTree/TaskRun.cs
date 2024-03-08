using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
public class TaskRun : BehaviourTree.Node
{
    private Transform _transform;
    private Morshu morshu = Morshu.instance;

    public TaskRun(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target == null)
        {
            state = NodeState.FAILURE;
            return state;
        }
        if (Vector3.Distance(_transform.position, target.position) > 0.01f )
        {
            Cell targetCell = morshu.GetGrid().GetClosestCell(-target.position);
            morshu.MoveToCell(targetCell);
        }
        state = NodeState.RUNNING; 
        return state;
    }
}
