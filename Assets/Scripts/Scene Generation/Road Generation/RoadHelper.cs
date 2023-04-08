using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoadHelper : MonoBehaviour
{
	public GameObject roadStraight, roadCorner, road3WayIntersecton, road4WayIntersection, roadEnd;
	Dictionary<Vector3Int, GameObject> roadDictionary = new Dictionary<Vector3Int, GameObject>();
	HashSet<Vector3Int> fixRoadCandidates = new HashSet<Vector3Int>();

	public List<Vector3Int> GetRoadPositions()
    {
		return roadDictionary.Keys.ToList();
    }

	public void PlaceStreetPositions(Vector3 startPosition, Vector3Int direction, int length)
	{
		var rotation = Quaternion.identity;
		if (direction.x == 0)
		{
			rotation = Quaternion.Euler(0, 90, 0);
		}
		for (int i = 0; i < length; i++)
		{
			var position = Vector3Int.RoundToInt(startPosition + direction * i);
			if (roadDictionary.ContainsKey(position))
			{
				continue;
			}
			var road = Instantiate(roadStraight, position, rotation, transform);
			roadDictionary.Add(position, road);
			if (i == 0 || i == length - 1)
			{
				fixRoadCandidates.Add(position);
			}
		}
	}

	public void FixRoad()
	{
		foreach (var position in fixRoadCandidates)
		{
			List<RoadDirection> neighbourDirections = PlacementHelper.FindNeighbour(position, roadDictionary.Keys);

			Quaternion rotation = Quaternion.identity;

			if (neighbourDirections.Count == 1)
			{
				Destroy(roadDictionary[position]);
				if (neighbourDirections.Contains(RoadDirection.Down))
				{
					rotation = Quaternion.Euler(0, 90, 0);
				}
				else if (neighbourDirections.Contains(RoadDirection.Left))
				{
					rotation = Quaternion.Euler(0, 180, 0);
				}
				else if (neighbourDirections.Contains(RoadDirection.Up))
				{
					rotation = Quaternion.Euler(0, -90, 0);
				}
				roadDictionary[position] = Instantiate(roadEnd, position, rotation, transform);
			}
			else if (neighbourDirections.Count == 2)
			{
				if (
					neighbourDirections.Contains(RoadDirection.Up) && neighbourDirections.Contains(RoadDirection.Down)
					|| neighbourDirections.Contains(RoadDirection.Right) && neighbourDirections.Contains(RoadDirection.Left)
					)
				{
					continue;
				}
				Destroy(roadDictionary[position]);
				if (neighbourDirections.Contains(RoadDirection.Up) && neighbourDirections.Contains(RoadDirection.Right))
				{
					rotation = Quaternion.Euler(0, 90, 0);
				}
				else if (neighbourDirections.Contains(RoadDirection.Right) && neighbourDirections.Contains(RoadDirection.Down))
				{
					rotation = Quaternion.Euler(0, 180, 0);
				}
				else if (neighbourDirections.Contains(RoadDirection.Down) && neighbourDirections.Contains(RoadDirection.Left))
				{
					rotation = Quaternion.Euler(0, -90, 0);
				}
				roadDictionary[position] = Instantiate(roadCorner, position, rotation, transform);
			}
			else if (neighbourDirections.Count == 3)
			{
				Destroy(roadDictionary[position]);
				if (neighbourDirections.Contains(RoadDirection.Right)
					&& neighbourDirections.Contains(RoadDirection.Down)
					&& neighbourDirections.Contains(RoadDirection.Left)
					)
				{
					rotation = Quaternion.Euler(0, 90, 0);
				}
				else if (neighbourDirections.Contains(RoadDirection.Down)
					&& neighbourDirections.Contains(RoadDirection.Left)
					&& neighbourDirections.Contains(RoadDirection.Up))
				{
					rotation = Quaternion.Euler(0, 180, 0);
				}
				else if (neighbourDirections.Contains(RoadDirection.Left)
					&& neighbourDirections.Contains(RoadDirection.Up)
					&& neighbourDirections.Contains(RoadDirection.Right))
				{
					rotation = Quaternion.Euler(0, -90, 0);
				}
				roadDictionary[position] = Instantiate(road3WayIntersecton, position, rotation, transform);
			}
			else
			{
				Destroy(roadDictionary[position]);
				roadDictionary[position] = Instantiate(road4WayIntersection, position, rotation, transform);
			}
		}
	}
}


	/*
	public void FixRoad()
    {
        foreach (var position in fixRoadCandidates)
        {
            List<RoadDirection> neighbourDirections = PlacementHelper.FindNeighbour(position, roadDictionary.Keys);

            Quaternion rotation = Quaternion.identity;

            if (neighbourDirections.Count == 1)
            {
                Destroy(roadDictionary[position]);

                if (neighbourDirections.Contains(RoadDirection.Down))
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (neighbourDirections.Contains(RoadDirection.Left))
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (neighbourDirections.Contains(RoadDirection.Up))
                {
                    rotation = Quaternion.Euler(0, -90, 0);
                }
				roadDictionary[position] = Instantiate(roadEnd, position, rotation, transform);
            }
            else if (neighbourDirections.Count == 2)
            {
                if (neighbourDirections.Contains(RoadDirection.Up) && neighbourDirections.Contains(RoadDirection.Down)
                    || neighbourDirections.Contains(RoadDirection.Right) && neighbourDirections.Contains(RoadDirection.Left))
                {
                    continue;
                }
            }
            else if (neighbourDirections.Count == 3)
            {

            }
            else
            {
                Destroy(roadDictionary[position]);
                roadDictionary[position] = Instantiate(road4WayIntersection, position, rotation, transform);
            }
        }
    }
}*/