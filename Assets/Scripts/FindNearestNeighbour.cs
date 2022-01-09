using UnityEngine;

public class FindNearestNeighbour : SingletonMB<FindNearestNeighbour>
{
    public KdTree<CubeController> cubes = new KdTree<CubeController>();
    public bool ShowLineInGame;

    private void Update()
    {
        if (cubes.Count > 1)
        {
            cubes.UpdatePositions();

            foreach (var cube in cubes)
            {
                CubeController nearestObject = cubes.FindClosestExceptSelf(cube.transform.position, cube);

                if (ShowLineInGame)
                    cube.target = nearestObject;

                Debug.DrawLine(cube.transform.position, nearestObject.transform.position);
            }
        }

    }
}
