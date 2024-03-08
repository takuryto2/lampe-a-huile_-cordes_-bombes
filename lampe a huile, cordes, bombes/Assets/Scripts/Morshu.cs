using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.Tilemaps.Tilemap;

public class Morshu : MonoBehaviour
{
    [SerializeField] private GameGrid grid;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Transform _transform;
    private Cell cellOn;
    private List<Node> opened = new List<Node>();
    private List<Node> closed = new List<Node>();
    private List<Node> pathToPlayerOne = new List<Node>();
    private float branchWeight = 1;
    [SerializeField] bool startInTheMidle;
    [SerializeField] bool snapToGrid;
    private Entity entity;
    private Vector3 lastPosition, targetPosition;
    private float timer;
    [SerializeField] private float moveSpeed;
    private bool isMoving;
    private bool terrorism = true;
    [SerializeField] private GameObject bombPrefab;
    private int orientationX;
    private int orientationY;
    public static Morshu instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        grid = GameGrid.instance;
        player = PlayerMovement.instance;
        if (startInTheMidle)
        {
            cellOn = grid.GetCell((grid.gridWidth - 1) / 2, (grid.gridHeight - 1) / 2);
        }
        else if (snapToGrid)
        {
            cellOn = grid.GetClosestCell(transform.position);
        }
        else
        {
            cellOn = grid.GetCell(_transform.position.x.ConvertTo<int>(), _transform.position.z.ConvertTo<int>());
        }
        entity = Entity.instance;
        cellOn.SetEntity(entity);
        transform.position = cellOn.pos;

        lastPosition = cellOn.pos;

        targetPosition = cellOn.pos;

        isMoving = true;

    }

    void Update()
    {
        if (isMoving)
        {
            timer += Time.deltaTime * moveSpeed;
            timer = Mathf.Clamp01(timer);
            transform.position = Vector3.Lerp(lastPosition, targetPosition, timer);
            if (timer == 1f)
            {
                InitPath();
                isMoving = false;
                if (pathToPlayerOne.Count - 1 <= player.radius)
                {
                    OnBombEnter();
                }
                else
                {
                    MoveToCell(pathToPlayerOne[pathToPlayerOne.Count - 1].cell);
                }
                pathToPlayerOne.Clear();
            }
        }
    }

    private void InitPath()
    {
        opened.Clear();
        closed.Clear();
        float heuristique = GetHeuristique(cellOn, player.cellOn);
        opened.Add(new Node(cellOn, heuristique, heuristique, null));

        Astar(player.cellOn);
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

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Explosion"))
        {
            SceneManager.LoadScene("Simon_Win");
        }
    }

    public void MoveToCell(Cell cell)
    {
            lastPosition = cellOn.pos;

            targetPosition = cell.pos;

        if (!WallDetection(cellOn.pos, targetPosition))
        {

            orientationX = cell.gridPos.Item1 - cellOn.gridPos.Item1;
            orientationY = cell.gridPos.Item2 - cellOn.gridPos.Item2;

            cellOn.DeleteEntity();

            cell.SetEntity(entity);

            cellOn = cell;

            isMoving = true;

            timer = 0f;
        }
        else
        {
            lastPosition = cellOn.pos;

            targetPosition = cellOn.pos;

            orientationX = cell.gridPos.Item1 - cellOn.gridPos.Item1;
            orientationY = cell.gridPos.Item2 - cellOn.gridPos.Item2;



            cellOn.DeleteEntity();

            cellOn.SetEntity(entity);

            isMoving = true;

            timer = 0f;
        }
    }

    public void OnBombEnter()
    {
        if (terrorism == true)
        {
            Cell bombCell = grid.GetCell(cellOn.gridPos.Item1 + orientationX, cellOn.gridPos.Item2 + orientationY);
            GameObject bomb = Instantiate(bombPrefab, bombCell.pos, Quaternion.identity);
            SetBool(false);
        }
    }

    public void SetBool(bool value)
    {
        terrorism = value;
    }

    public bool WallDetection(Vector3 startPos, Vector3 endPos)
    {
        Debug.DrawRay(startPos, (endPos - startPos) * Vector3.Distance(startPos, endPos), Color.red, 2);
        if (Physics.Raycast(startPos, endPos - startPos, Vector3.Distance(startPos, endPos)))
        {
            return true;
        }
        return false;
    }

    public GameGrid GetGrid() { return grid; }

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
