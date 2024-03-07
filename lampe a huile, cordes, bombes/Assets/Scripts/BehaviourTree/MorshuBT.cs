using BehaviourTree;
using System.Collections.Generic;

public class MorshuBT : Tree
{
    public static float fovRange = 6f;
    public static float speed = 2f;
    protected override BehaviourTree.Node SetUpTree()
    {
        BehaviourTree.Node root = new Sequence(new List<BehaviourTree.Node>
            {
                new CheckBombInFOVRange(transform),
                new TaskRun(transform),
            });
        return root;
    }
}
