using System.Collections.Generic;
using UnityEngine;
using static SimpleVisualiser;

public class RoadVisualiser : MonoBehaviour
{
    public LSystemStringBuilder lSystemStringBuilder;
    List<Vector3> positions = new List<Vector3>();
    //public Vector3 cityOffset;

    public RoadHelper roadHelper;
    public StructureHelper structureHelper;

    private int length = 10;
    private float angle = 90;

    public int Length
    {
        get
        {
            if (length > 0)
            {
                return length;
            }
            else
            {
                return 1;
            }
        }
        set => length = value;
    }

    private void Start()
    {
        var sequence = lSystemStringBuilder.GenerateSentence();
        VisualiseSequence(sequence);
        //transform.position += cityOffset;
    }

    private void VisualiseSequence(string sequence)
    {
        Stack<RoadAgentParameters> savePoints = new Stack<RoadAgentParameters>();
        var currentPosition = Vector3.zero;

        Vector3 direction = Vector3.forward;
        Vector3 tempPosition = Vector3.zero;

        positions.Add(currentPosition);

        foreach (var letter in sequence)
        {
            EncodingLetters encoding = (EncodingLetters)letter;

            switch (encoding)
            {
                case EncodingLetters.save:
                    savePoints.Push(new RoadAgentParameters
                    {
                        position = currentPosition,
                        direction = direction,
                        length = Length
                    });
                    break;
                case EncodingLetters.load:
                    if (savePoints.Count > 0)
                    {
                        var agentParameter = savePoints.Pop();
                        currentPosition = agentParameter.position;
                        direction = agentParameter.direction;
                        Length = agentParameter.length;
                    }
                    else
                    {
                        throw new System.Exception("No saved point in stack");
                    }
                    break;
                case EncodingLetters.draw:
                    tempPosition = currentPosition;
                    currentPosition += direction * length;
                    roadHelper.PlaceStreetPositions(tempPosition, Vector3Int.RoundToInt(direction), length);
                    //Length -= 2; Decrease size of city as it gets generated
                    positions.Add(currentPosition);
                    break;
                case EncodingLetters.turnRight:
                    direction = Quaternion.AngleAxis(angle, Vector3.up) * direction;
                    break;
                case EncodingLetters.turnLeft:
                    direction = Quaternion.AngleAxis(-angle, Vector3.up) * direction;
                    break;
                default:
                    break;
            }
        }
        roadHelper.FixRoad();
        structureHelper.PlaceStruturesAroundRoad(roadHelper.GetRoadPositions());
    }
}
