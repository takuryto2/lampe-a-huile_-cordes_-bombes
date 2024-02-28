using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[System.Serializable]
public class Cell
{
    public Vector3 pos { get; private set; } = Vector3.zero;
    public Entity entity;
    public bool isExploding = false;
    [HideInInspector] public (int, int) gridPos;


    public Cell(Vector3 pos, (int, int) gridPos)
    {
        this.pos = pos;
        this.gridPos = gridPos;
    }

    public void DeleteEntity()
    {
        this.entity = null;
    }

    public void SetEntity(Entity entity)
    {
        this.entity = entity;
    }

    public bool HasEntity()
    {
        if (entity == null) return false;
        return true;
    }

    public void ExplodeCell()
    {
        Debug.Log("TAMERE");
        if (entity == null) return;
        isExploding = true;
        DeleteEntity();
        isExploding = false;
    }

}
