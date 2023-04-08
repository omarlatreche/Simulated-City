using System;
using System.Collections.Generic;
using UnityEngine;

public static class PlacementHelper
{
    public static List<RoadDirection> FindNeighbour(Vector3Int position, ICollection<Vector3Int> collection)
    {
        List<RoadDirection> neighbourDirections = new List<RoadDirection>();
        if (collection.Contains(position + Vector3Int.right))
        {
            neighbourDirections.Add(RoadDirection.Right);
        }
        if (collection.Contains(position - Vector3Int.right))
        {
            neighbourDirections.Add(RoadDirection.Left);
        }
        if (collection.Contains(position + new Vector3Int(0, 0, 1)))
        {
            neighbourDirections.Add(RoadDirection.Up);
        }
        if (collection.Contains(position - new Vector3Int(0, 0, 1)))
        {
            neighbourDirections.Add(RoadDirection.Down);
        }
        return neighbourDirections;
    }

    internal static Vector3Int GetOffsetFromDirection(RoadDirection direction)
    {
        switch (direction)
        {
            case RoadDirection.Up:
                return new Vector3Int(0, 0, 1);
            case RoadDirection.Down:
                return new Vector3Int(0, 0, -1);
            case RoadDirection.Left:
                return Vector3Int.left;
            case RoadDirection.Right:
                return Vector3Int.right;
            default:
                break;
        }
        throw new Exception(direction + " does not exist");
    }

    public static RoadDirection GetReverseDirection(RoadDirection direction)
    {
        switch (direction)
        {
            case RoadDirection.Up:
                return RoadDirection.Down;
            case RoadDirection.Down:
                return RoadDirection.Up;
            case RoadDirection.Left:
                return RoadDirection.Left;
            case RoadDirection.Right:
                return RoadDirection.Right;
            default:
                break;
        }
        throw new Exception(direction + " does not exist");
    }
}
