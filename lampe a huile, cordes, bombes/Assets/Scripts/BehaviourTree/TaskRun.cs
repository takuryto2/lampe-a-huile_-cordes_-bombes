using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
public class TaskRun : BehaviourTree.Node
{
    private Transform _transform;

    public TaskRun(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (Vector3.Distance(_transform.position, target.position) > 0.01f )
        {
            _transform.position = Vector3.MoveTowards(_transform.position, target.position, MorshuBT.speed * Time.deltaTime);
            _transform.LookAt(target.position);
        }
        state = NodeState.RUNNING; 
        return state;
    }
}
