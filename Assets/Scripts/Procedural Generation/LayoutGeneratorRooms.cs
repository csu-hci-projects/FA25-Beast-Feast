using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using System.Linq;
using Random = System.Random;

public class LayoutGeneratorRooms : MonoBehaviour
{
    [SerializeField] RoomLevelLayoutConfiguration levelConfig;

    [SerializeField] GameObject levelLayoutDisplay;
    [SerializeField] List<Hallway> openDoorways;

    Random random;
    Level level;
    Dictionary<RoomTemplate, int> availableRooms;


    [ContextMenu("Generate Level Layout")]
    public Level GenerateLevel()
    {
        SharedLevelData.Instance.ResetRandom();
        random = SharedLevelData.Instance.Rand;
        availableRooms = levelConfig.GetAvailableRooms();
        openDoorways = new List<Hallway>();
        level = new Level(levelConfig.Width, levelConfig.Length);

        RoomTemplate startRoomTemplate = availableRooms.Keys.ElementAt(random.Next(0, 1));
        var roomRect = GetStartRoomRect(startRoomTemplate);
        Room room = CreateNewRoom(roomRect, startRoomTemplate);
        List<Hallway> hallways = room.CalculateAllPossibleDoorways(room.Area.width, room.Area.height, levelConfig.DoorDistanceFromEdge);
        hallways.ForEach(h => h.StartRoom = room);
        hallways.ForEach(h => openDoorways.Add(h));
        level.AddRoom(room);

        Hallway selectedEntryway = openDoorways[random.Next(0, openDoorways.Count)];
        AddRooms();
        AddHallwaysToRooms();
        AssignRoomTypes();

        DrawLayout(selectedEntryway, roomRect);

        levelLayoutDisplay.SetActive(false);

        return level;
    }

    private void AssignRoomTypes()
    {
        List<Room> borderRooms = level.Rooms.Where(room => room.Connectedness == 1).ToList();
        if (borderRooms.Count < 2)
        {
            return;
        }
        int startRoomIndex = random.Next(0, borderRooms.Count);
        Room randomStartRoom = borderRooms[startRoomIndex];
        level.playerStartRoom = randomStartRoom;
        randomStartRoom.Type = RoomType.Start;
        borderRooms.Remove(randomStartRoom);

        Room farthestRoom = borderRooms
            .OrderByDescending(room => UnityEngine.Vector2.Distance(randomStartRoom.Area.center, room.Area.center))
            .FirstOrDefault();
        farthestRoom.Type = RoomType.Exit;
        borderRooms.Remove(farthestRoom);

        List<Room> treasureRooms = borderRooms.OrderBy(r => random.Next()).Take(3).ToList();
        borderRooms.RemoveAll(room => treasureRooms.Contains(room));
        treasureRooms.ForEach(room => room.Type = RoomType.Treasure);

        List<Room> emptyRooms = level.Rooms.Where(room => room.Type.HasFlag(RoomType.Default)).ToList();
        Room bossRoom = emptyRooms
            .OrderByDescending(room => UnityEngine.Vector2.Distance(randomStartRoom.Area.center, room.Area.center))
            .OrderByDescending(room => room.Connectedness)
            .OrderByDescending(room => room.Area.width * room.Area.height)
            .FirstOrDefault();
        bossRoom.Type = RoomType.Boss;
        emptyRooms.Remove(bossRoom);

        emptyRooms = emptyRooms.OrderBy(room => random.Next()).ToList();
        RoomType[] typesToAssign = { RoomType.Prison, RoomType.Library, RoomType.Kitchen };
        List<Room> roomsToAssign = emptyRooms.Take(typesToAssign.Length).ToList();
        for (int i = 0; i < roomsToAssign.Count; i++)
        {
            roomsToAssign[i].Type = typesToAssign[i];
        }
    }

    void AddHallwaysToRooms()
    {
        foreach (Room room in level.Rooms)
        {
            Hallway[] hallwaysStartingAtRoom = Array.FindAll(level.Hallways, hallway => hallway.StartRoom == room);
            Array.ForEach(hallwaysStartingAtRoom, hallway => room.AddHallway(hallway));
            Hallway[] hallwaysEndingAtRoom = Array.FindAll(level.Hallways, hallway => hallway.EndRoom == room);
            Array.ForEach(hallwaysEndingAtRoom, hallway => room.AddHallway(hallway));
        }
    }

    [ContextMenu("Generate New Seed")]
    public void GenerateNewSeed()
    {
        SharedLevelData.Instance.GenerateSeed();
    }

    [ContextMenu("Generate New Seed and Level")]
    public void GenerateNewSeedAndLevel()
    {
        GenerateNewSeed();
        GenerateLevel();
    }

    RectInt GetStartRoomRect(RoomTemplate roomTemplate)
    {
        RectInt roomSize = roomTemplate.GenerateRoomCandidateRect(random);

        int roomWidth = roomSize.width;
        int availableWidthX = level.Width / 2 - roomWidth;
        int randomX = random.Next(0, availableWidthX);
        int roomX = randomX + (level.Width / 4);

        int roomLength = roomSize.height;
        int availableLengthY = level.Length / 2 - roomLength;
        int randomY = random.Next(0, availableLengthY);
        int roomY = randomY + (level.Length / 4);

        return new RectInt(roomX, roomY, roomWidth, roomLength);
    }

    void DrawLayout(Hallway selectedEntryway = null, RectInt roomCandidateRect = new RectInt(), bool isDebug = false)
    {
        var renderer = levelLayoutDisplay.GetComponent<Renderer>();

        var layoutTexture = (Texture2D)renderer.sharedMaterial.mainTexture;

        layoutTexture.Reinitialize(level.Width, level.Length);
        int scale = SharedLevelData.Instance.Scale;
        levelLayoutDisplay.transform.localScale = new UnityEngine.Vector3(level.Width * scale, level.Length * scale, 1);
        float xPos = level.Width * scale / 2.0f - scale;
        float zPos = level.Length * scale / 2.0f - scale;
        levelLayoutDisplay.transform.position = new UnityEngine.Vector3(xPos, 0.1f, zPos);
        layoutTexture.FillWithColor(Color.black);

        foreach (Room room in level.Rooms)
        {
            if (room.LayoutTexture != null)
            {
                layoutTexture.DrawTexture(room.LayoutTexture, room.Area);
            }
            else
            {
                layoutTexture.DrawRectangle(room.Area, Color.white);
            }
            Debug.Log(room.Area + " " + room.Connectedness + " " + room.Type);
        }
        Array.ForEach(level.Hallways, hallway => layoutTexture.DrawLine(hallway.StartPositionAbsolute, hallway.EndPositionAbsolute, Color.white));
        layoutTexture.ConvertToBlackAndWhite();
        if (isDebug)
        {
            layoutTexture.DrawRectangle(roomCandidateRect, Color.blue);

            openDoorways.ForEach(hallway => layoutTexture.SetPixel(hallway.StartPositionAbsolute.x, hallway.StartPositionAbsolute.y, hallway.StartDirection.GetColor()));
        }


        if (isDebug && selectedEntryway != null)
        {
            layoutTexture.SetPixel(selectedEntryway.StartPositionAbsolute.x, selectedEntryway.StartPositionAbsolute.y, Color.red);
        }

        layoutTexture.SaveAsset();

    }

    Hallway SelectHallwayCandidate(RectInt roomCandidateRect, RoomTemplate roomTemplate, Hallway entryway)
    {
        Room room = CreateNewRoom(roomCandidateRect, roomTemplate, false);
        List<Hallway> candidates = room.CalculateAllPossibleDoorways(room.Area.width, room.Area.height, levelConfig.DoorDistanceFromEdge);
        HallwayDirection requiredDirection = entryway.StartDirection.GetOppositeDirection();
        List<Hallway> filteredHallwayCandidates = candidates.Where(hallwayCadidate => hallwayCadidate.StartDirection == requiredDirection).ToList();
        return filteredHallwayCandidates.Count > 0 ? filteredHallwayCandidates[random.Next(filteredHallwayCandidates.Count)] : null;
    }

    Vector2Int CalculateRoomPosition(Hallway entryway, int roomWidth, int roomLength, int distance, Vector2Int endPosition)
    {
        Vector2Int roomPosition = entryway.StartPositionAbsolute;
        switch (entryway.StartDirection)
        {
            case HallwayDirection.Left:
                roomPosition.x -= distance + roomWidth;
                roomPosition.y -= endPosition.y;
                break;
            case HallwayDirection.Top:
                roomPosition.x -= endPosition.x;
                roomPosition.y += distance + 1;
                break;
            case HallwayDirection.Right:
                roomPosition.x += distance + 1;
                roomPosition.y -= endPosition.y;
                break;
            case HallwayDirection.Bottom:
                roomPosition.x -= endPosition.x;
                roomPosition.y -= distance + roomLength;
                break;
        }
        return roomPosition;
    }

    Room ConstructAdjacetRoom(Hallway selectedEntryway)
    {
        RoomTemplate roomTemplate = availableRooms.Keys.ElementAt(random.Next(0, availableRooms.Count));
        RectInt roomCandidateRect = roomTemplate.GenerateRoomCandidateRect(random);
        Hallway selectedExit = SelectHallwayCandidate(roomCandidateRect, roomTemplate, selectedEntryway);
        if (selectedExit == null) { return null; }
        int distance = random.Next(levelConfig.HallwayLengthMin, levelConfig.HallwayLengthMax + 1);
        Vector2Int roomCandidatePosition = CalculateRoomPosition(selectedEntryway, roomCandidateRect.width, roomCandidateRect.height, distance, selectedExit.StartPosition);
        roomCandidateRect.position = roomCandidatePosition;

        if (!IsRoomCandidateValid(roomCandidateRect))
        {
            return null;
        }

        Room newRoom = CreateNewRoom(roomCandidateRect, roomTemplate);
        selectedEntryway.EndRoom = newRoom;
        selectedEntryway.EndPosition = selectedExit.StartPosition;
        return newRoom;
    }

    void AddRooms()
    {
        while (openDoorways.Count > 0 && level.Rooms.Length < levelConfig.MaxRoomCount && availableRooms.Count > 0)
        {
            Hallway selectedEntryWay = openDoorways[random.Next(0, openDoorways.Count)];
            Room newRoom = ConstructAdjacetRoom(selectedEntryWay);

            if (newRoom == null)
            {
                openDoorways.Remove(selectedEntryWay);
                continue;
            }

            level.AddRoom(newRoom);
            level.AddHallway(selectedEntryWay);

            selectedEntryWay.EndRoom = newRoom;
            List<Hallway> newOpenHallways = newRoom.CalculateAllPossibleDoorways(newRoom.Area.width, newRoom.Area.height, levelConfig.DoorDistanceFromEdge);
            newOpenHallways.ForEach(hallway => hallway.StartRoom = newRoom);

            openDoorways.Remove(selectedEntryWay);
            openDoorways.AddRange(newOpenHallways);
        }
    }

    private void UseUpRoomTemplate(RoomTemplate roomTemplate)
    {
        availableRooms[roomTemplate] -= 1;
        if (availableRooms[roomTemplate] == 0)
        {
            availableRooms.Remove(roomTemplate);
        }
    }

    bool IsRoomCandidateValid(RectInt roomCandidateRect)
    {
        RectInt levelRect = new RectInt(1, 1, level.Width - 2, level.Length - 2);
        return levelRect.Contains(roomCandidateRect) && !CheckRoomOverlap(roomCandidateRect, level.Rooms, level.Hallways, levelConfig.MinRoomDistance);
    }

    bool CheckRoomOverlap(RectInt roomCandidateRect, Room[] rooms, Hallway[] hallways, int minRoomDistance)
    {
        RectInt paddedRoomRect = new RectInt
        {
            x = roomCandidateRect.x - minRoomDistance,
            y = roomCandidateRect.y - minRoomDistance,
            width = roomCandidateRect.width + 2 * minRoomDistance,
            height = roomCandidateRect.height + 2 * minRoomDistance
        };

        foreach (Room room in rooms)
        {
            if (paddedRoomRect.Overlaps(room.Area))
            {
                return true;
            }
        }

        foreach (Hallway hallway in hallways)
        {
            if (paddedRoomRect.Overlaps(hallway.Area))
            {
                return true;
            }
        }

        return false;
    }

    Room CreateNewRoom(RectInt roomCandidateRect, RoomTemplate roomTemplate, bool useUp = true)
    {
        if (useUp)
        {
            UseUpRoomTemplate(roomTemplate);
        }
        if (roomTemplate.LayoutTexture == null)
        {
            return new Room(roomCandidateRect);
        }
        else
        {
            return new Room(roomCandidateRect.x, roomCandidateRect.y, roomTemplate.LayoutTexture);
        }
    }
}
