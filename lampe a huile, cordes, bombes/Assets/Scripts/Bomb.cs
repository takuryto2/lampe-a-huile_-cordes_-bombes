using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Bomb : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    private PlayerMovement player;
    public List<Cell> explodingCells;
    private Cell cellOn;
    public GameObject explosionPrefab;
    public LayerMask levelMask;
    [SerializeField] private float tickBoom;

    private void Start()
    {
        player = PlayerMovement.instance;
        cellOn = player.grid.GetCell(_transform.position.x.ConvertTo<int>(), _transform.position.z.ConvertTo<int>());
        //explodingCells = player.grid.GetNeighbors(cellOn);
    }
    void Update()
    {
        tickBoom -= Time.deltaTime;
        if (tickBoom <= 0f)
        {
            BOOM();
        }
    }

    private void BOOM()
    {
        /*for (int i = 0; i < explodingCells.Count; i++)
        {
            Debug.Log("caca");
            player.grid.GetCell(explodingCells[i].gridPos.Item1, explodingCells[i].gridPos.Item2).ExplodeCell();
        }*/
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        StartCoroutine(CreateExplosions(Vector3.forward * player.grid.cellSpacement));
        Debug.Log("next");
        StartCoroutine(CreateExplosions(Vector3.right * player.grid.cellSpacement));
        Debug.Log("next");
        StartCoroutine(CreateExplosions(Vector3.back * player.grid.cellSpacement));
        Debug.Log("next");
        StartCoroutine(CreateExplosions(Vector3.left * player.grid.cellSpacement));
        Destroy(this.gameObject);
    }

    private IEnumerator CreateExplosions(Vector3 direction)
    {
        //1
        for (int i = 1; i < player.radius + 1; i++)
        {
            //2
            RaycastHit hit;
            //3
            Physics.Raycast(_transform.position + new Vector3(0, 0.5f, 0), direction, out hit,
              i, levelMask);

            //4
            if (!hit.collider)
            {
                Instantiate(explosionPrefab, _transform.position + (i * direction),
                  //5 
                  explosionPrefab.transform.rotation);
                //6
            }
            else
            { //7
                break;
            }

            //8
            yield return new WaitForSeconds(0.05f);
        }
    }


}
