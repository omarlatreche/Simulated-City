using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureHelper : MonoBehaviour
{
    public GameObject prefab;
    public Dictionary<Vector3Int, GameObject> structureDictionary = new Dictionary<Vector3Int, GameObject>();
    [SerializeField] private Vector3 curbOffset;

    public void PlaceStruturesAroundRoad(List<Vector3Int> roadPositions)
    {
        Dictionary<Vector3Int, RoadDirection> freeEstateSpots = FindFreeSpotsAroundRoad(roadPositions);

        foreach (var freeSpot in freeEstateSpots)
        {
            var rotation = Quaternion.identity;
            switch (freeSpot.Value)
            {
                case RoadDirection.Up:
                    rotation = Quaternion.Euler(0, 90, 0);
                    break;
                case RoadDirection.Down:
                    rotation = Quaternion.Euler(0, -90, 0);
                    break;
                case RoadDirection.Left:
                    rotation = Quaternion.Euler(0, 180, 0);
                    break;
                default:
                    break;
            }

            Instantiate(prefab, freeSpot.Key + curbOffset, rotation, transform);
        }
    }

    private Dictionary<Vector3Int, RoadDirection> FindFreeSpotsAroundRoad(List<Vector3Int> roadPositions)
    {
        Dictionary<Vector3Int, RoadDirection> freeSpaces = new Dictionary<Vector3Int, RoadDirection>();

        foreach (var position in roadPositions)
        {
            var neighbourDirections = PlacementHelper.FindNeighbour(position, roadPositions);

            foreach (RoadDirection direction in Enum.GetValues(typeof(RoadDirection)))
            {
                if(neighbourDirections.Contains(direction) == false)
                {
                    var newPosition = position + PlacementHelper.GetOffsetFromDirection(direction);
                    if (freeSpaces.ContainsKey(newPosition))
                    {
                        continue;
                    }
                    freeSpaces.Add(newPosition, PlacementHelper.GetReverseDirection(direction));
                }
            }
        }
        return freeSpaces;
    }
}
