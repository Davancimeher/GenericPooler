using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CubeController : MonoBehaviour
{
    private FindNearestNeighbour FindNearestNeighbour;
    private PoolSpawner poolSpawner;

    [SerializeField] private LineRenderer lineRenderer;

    public CubeController target;

    [SerializeField] private float Speed;
    [SerializeField] private Vector3 PositionTarget;
    [SerializeField] private float timer;

    public void GetNewStats()
    {
        Speed = Random.Range(0.5f, 1.5f);
        PositionTarget = PositionTarget.Random(poolSpawner.m_MinPosition, poolSpawner.m_MaxPosition);
        timer = Random.Range(3f,5f);
    }
    public void UpdatePosition()
    {
        timer -= Time.deltaTime;

        transform.position = Vector3.Lerp(transform.position, PositionTarget, Time.deltaTime * Speed);

        if (timer <= 0)
        {
            GetNewStats();
        }
    }
    public void SetLineRendrer()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, target.transform.position);
    }

    private void Update()
    {
        if (target != null && FindNearestNeighbour.ShowLineInGame)
            SetLineRendrer();

        UpdatePosition();
    }

    private void OnEnable()
    {
        FindNearestNeighbour = FindNearestNeighbour.Instance;
        poolSpawner = PoolSpawner.Instance;

        FindNearestNeighbour.cubes.Add(this);
    }

    private void OnDisable()
    {
        int index = FindNearestNeighbour.cubes.ToList().FindIndex(a => a == this);

        if (index != -1)
            FindNearestNeighbour.cubes.RemoveAt(index);
    }
}
