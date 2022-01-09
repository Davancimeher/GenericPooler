using System;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    private FindNearestNeighbour FindNearestNeighbour;
    [SerializeField] private LineRenderer lineRenderer;

    public CubeController target;

    private float speed;
    private Vector3 position;
    private float timer;


    public void SetLineRendrer()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, target.transform.position);
    }

    private void Update()
    {

        if (target != null && FindNearestNeighbour.ShowLineInGame)
            SetLineRendrer();


        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            UpdatePosition();
        }

    }
    private void GetRandomPosition()
    {
        rand = Random.Range(-4.5f, 4.5f);
        rand = Random.Range(-4.5f, 4.5f);
        rand = Random.Range(-4.5f, 4.5f);

    }
    private void UpdatePosition()
    {
        Ra

    }

    private void OnEnable()
    {
        FindNearestNeighbour = FindNearestNeighbour.Instance;

        FindNearestNeighbour.cubes.Add(this);
    }

    private void OnDisable()
    {
        int index = FindNearestNeighbour.cubes.ToList().FindIndex(a => a == this);

        if (index != -1)
            FindNearestNeighbour.cubes.RemoveAt(index);
    }
}
