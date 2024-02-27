using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class Morshu : MonoBehaviour
{
    [SerializeField] private GameGrid grid;
    [SerializeField] private Player player;
    [SerializeField] private Transform position;
    [SerializeField] private float cellSize;
    private Cell cellOn;
    private List<Node> opened = new List<Node>();
    private List<Node> closed = new List<Node>();
    private List<Node> pathToPlayerOne = new List<Node>();
    private float branchWeight = 1;
    void Start()
    {
        InitPath();
        cellOn = pathToPlayerOne[pathToPlayerOne.Count - 1].cell;
        position.position = pathToPlayerOne[pathToPlayerOne.Count - 1].cell.pos;
        pathToPlayerOne.RemoveAt(pathToPlayerOne.Count - 1);
        pathToPlayerOne.Clear();
    }

    void Update()
    {
        
    }

    private void InitPath()
    {
        opened.Clear();
        closed.Clear();
        float heuristique = GetHeuristique(cellOn, player.GetCellOn());
        opened.Add(new Node(cellOn, heuristique, heuristique, null));

        Astar(player.GetCellOn());
        pathToPlayerOne.Clear();
        GetPathFromClosedList(closed[closed.Count - 1], pathToPlayerOne);
        pathToPlayerOne.RemoveAt(pathToPlayerOne.Count - 1);
    }

    private void Astar(Cell finish)
    {

        int bestIndexInOpen = FindBestPotentialNodeIndex();

        Node currentNode = opened[bestIndexInOpen];

        opened.RemoveAt(bestIndexInOpen);

        if (currentNode.cell != finish)
        {
            foreach (Cell neighbor in grid.GetNeighbors(currentNode.cell))
            {
                if (ReevaluateOpen(neighbor, currentNode))
                {
                }
                else if (ReevaluateClosed(neighbor, currentNode))
                {
                }
                else
                {
                    opened.Add(new Node(neighbor, currentNode.weight - currentNode.euristic + branchWeight + GetHeuristique(neighbor, finish), GetHeuristique(neighbor, finish), currentNode));
                }
            }
            closed.Add(currentNode);
            Astar(finish);
        }
        else
        {
            closed.Add(currentNode);
        }
    }

    private bool ReevaluateOpen(Cell value, Node currentStudy)
    {
        for (int i = 0; i < opened.Count; i++)
        {
            if (value == opened[i].cell)
            {
                if (currentStudy.weight - currentStudy.euristic + branchWeight + opened[i].euristic < opened[i].weight)
                {
                    opened[i].weight = currentStudy.weight - currentStudy.euristic + branchWeight + opened[i].euristic;
                    opened[i].parent = currentStudy;

                }
                return true;
            }
        }
        return false;
    }

    private bool ReevaluateClosed(Cell value, Node currentStudy)
    {
        for (int i = 0; i < closed.Count; i++)
        {
            if (value == closed[i].cell)
            {
                if (currentStudy.weight - currentStudy.euristic + branchWeight + closed[i].euristic < closed[i].weight)
                {
                    closed[i].weight = currentStudy.weight - currentStudy.euristic + branchWeight + closed[i].euristic;
                    closed[i].parent = currentStudy;

                    opened.Add(closed[i]);
                    closed.RemoveAt(i);
                }
                return true;
            }
        }
        return false;
    }

    private int FindBestPotentialNodeIndex()
    {
        int result = 0;
        float bestPotential = opened[0].weight;

        for (int i = 0; i < opened.Count; i++)
        {
            if (opened[i].weight < bestPotential)
            {
                bestPotential = opened[i].weight;
                result = i;
            }
        }
        return result;
    }

    private float GetHeuristique(Cell start, Cell finish)
    {
        return Vector3.Distance(start.pos, finish.pos);
    }

    private void GetPathFromClosedList(Node _node, List<Node> pathToFill)
    {
        Node value = new Node();
        value = _node;

        if (_node.parent == null)
        {
            pathToFill.Add(value);
        }
        else
        {

            pathToFill.Add(value);
            GetPathFromClosedList(value.parent, pathToFill);
        }
    }

}

public class Node
{
    public Cell cell;
    public float weight;
    public float euristic;
    public Node parent;

    public Node(Cell myName, float _weight, float _euristic, Node _parent)
    {
        cell = myName;
        weight = _weight;
        euristic = _euristic;
        parent = _parent;
    }
    public Node()
    {

    }
}
