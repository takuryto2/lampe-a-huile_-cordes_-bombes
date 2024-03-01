using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float actionTime;

    [SerializeField] private float moveSpeed;

    [SerializeField] public Cell cellOn;

    [SerializeField] bool startInTheMidle;

    [SerializeField] bool snapToGrid;

    [SerializeField] private Transform _transform;

    [SerializeField] public GameObject pauseCanvas;

    [SerializeField] List<MonoBehaviour> behaviours = new();

    private Vector3 lastPosition, targetPosition;

    public GameGrid grid;

    private bool isMoving;

    public bool isInAction { get => isMoving; }

    private Entity entity;

    private float timer;

    public static PlayerMovement instance;

    [SerializeField] private GameObject bombPrefab;

    //[SerializeField] private Bomb _bomb;

    public bool isOnPause;

    Vector2 movement;

    private int orientationX;
    private int orientationY;

    public int radius;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        grid = GameGrid.instance;

        radius = 1;

        //center on the grid or set manualy
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


        transform.position = cellOn.pos;

        lastPosition = cellOn.pos;

        targetPosition = cellOn.pos;


        isMoving = false;

        isOnPause = false;

        entity = Entity.instance;
        cellOn.SetEntity(entity);


    }

    public void SpeedChange(bool flash)
    {
        if (flash)
        {
            moveSpeed *= 2;
        }
        else
        {
            moveSpeed /= 2f;
        }
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
                isMoving = false;
                TryMove();
            }
        }
        if (cellOn.isExploding)
        {
            Death();
        }
    }

    public void OnMovementEnter(InputAction.CallbackContext ctx)
    {
        movement = ctx.ReadValue<Vector2>();

        if (isInAction)
        {
            return;
        }
        TryMove();
    }

    public void TryMove()
    {

        Vector3Int right = Vector3Int.RoundToInt(transform.right);
        Vector3Int forward = Vector3Int.RoundToInt(transform.forward);

        if (movement.x == 1)
        {
            MoveTo(right.x, right.z);

        }
        else if (movement.x == -1)
        {
            MoveTo(-right.x, -right.z);
        }
        else if (movement.y == 1)
        {
            MoveTo(forward.x, forward.z);
        }
        else if (movement.y == -1)
        {
            MoveTo(-forward.x, -forward.z);
        }
    }

    private void MoveTo(int x, int z)
    {
        Cell targetCell = grid.GetCell(cellOn.gridPos.Item1 + x, cellOn.gridPos.Item2 + z);

        orientationX = x;
        orientationY = z;
        if (targetCell != null && !WallDetection(cellOn.pos, targetCell.pos) && !targetCell.HasEntity())
        {
            MoveToCell(targetCell);
        }
        return;
    }

    void MoveToCell(Cell cell)
    {
        lastPosition = cellOn.pos;

        targetPosition = cell.pos;

        cellOn.DeleteEntity();


        cell.SetEntity(entity);

        cellOn = cell;

        isMoving = true;

        timer = 0f;
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

    public void OnPause(InputAction.CallbackContext ctx)
    {
        isOnPause = !isOnPause;
        if (isOnPause)
        {
            ActivateBehaviours();
            Time.timeScale = 1f;
            pauseCanvas.SetActive(false);
        }
        else
        {
            DeactivateBehaviours();
            Time.timeScale = 0f;
            pauseCanvas.SetActive(true);
        }
    }

    public void DeactivateBehaviours()
    {
        foreach (MonoBehaviour argument in behaviours)
        {
            argument.gameObject.SetActive(false);
        }
    }

    public void ActivateBehaviours()
    {
        foreach (MonoBehaviour argument in behaviours)
        {
            argument.gameObject.SetActive(true);
        }
    }

    public void OnBombEnter()
    {
        Cell bombCell = grid.GetCell(cellOn.gridPos.Item1 + orientationX, cellOn.gridPos.Item2 + orientationY);
        GameObject bomb = Instantiate(bombPrefab, bombCell.pos , Quaternion.identity);
    }

    private void Death()
    {
        moveSpeed = 0;
        gameObject.SetActive(false);
    }
}
