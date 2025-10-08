using System;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField] LayoutGeneratorRooms layoutGeneratorRooms;
    [SerializeField] MarchingSquares marchingSquares;
    [SerializeField] NavMeshSurface navMeshSurface;
    [SerializeField] RoomDecorator roomDecorator;

    private void Start()
    {
        GenerateRandom();
    }

    [ContextMenu("Generate Random")]
    public void GenerateRandom()
    {
        SharedLevelData.Instance.GenerateSeed();
        Generate();
    }

    [ContextMenu("Generate")]
    public void Generate()
    {
        Level level = layoutGeneratorRooms.GenerateLevel();
        marchingSquares.CreateLevelGeometry();
        roomDecorator.PlaceItems(level);
        navMeshSurface.BuildNavMesh();

        Room startRoom = level.playerStartRoom;
        Vector2 roomCenter = startRoom.Area.center;
        Vector3 playerPosition = LevelPositionToWorldPosition(roomCenter);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        NavMeshAgent playerNavMeshAgent = player.GetComponent<NavMeshAgent>();
        if (playerNavMeshAgent == null)
        {
            player.transform.position = playerPosition;
        }
        else
        {
            playerNavMeshAgent.Warp(playerPosition);
        }

    }

    Vector3 LevelPositionToWorldPosition(Vector2 levelPosition)
    {
        int scale = SharedLevelData.Instance.Scale;
        return new Vector3((levelPosition.x - 1) * scale, 0, levelPosition.y * scale);
    }
}
