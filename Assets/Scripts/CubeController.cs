using UnityEngine;

public class CubeController : MonoBehaviour
{
    private FindNearestNeighbour FindNearestNeighbour;

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
