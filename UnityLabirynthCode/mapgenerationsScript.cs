using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapgenerationsScript : MonoBehaviour {

    public int radius=1;
    public int width;
    public int height;
    public int wallTresholdSize=40;
    public int roomTresholdSize=30;
    public GameObject plane;
    public bool isManual=false;

    public string seed;
    public bool useRandomSeed=false;

    [Range(0, 1000)]
    public int randomFillPercent=521;

    int[,] map;

    void Start()
    {
        if (isManual)
        {
            GenerateMap();
            GameObject ground = Instantiate(plane, new Vector3(transform.position.x, -5, transform.position.z), Quaternion.identity);
            ground.transform.localScale = new Vector3((float)width / 10, 1, (float)height / 10);
        }
    }

    void getValuesAndMakeMap(int val)
    {
        seed = val.ToString();
        //transform.position = new Vector3(val[3], 0, val[4]);//instancjonowac w odpowiednim mijscu
        GenerateMap();
        GameObject ground = Instantiate(plane, new Vector3(transform.position.x, -5, transform.position.z), Quaternion.identity);
        ground.transform.localScale = new Vector3((float)width / 10, 1, (float)height / 10);
    }

    void GenerateMap()
    {
        map = new int[width, height];
        RandomFillMap();

        for(int i =0; i < 5; i++)
        {
            SmoothMap();
        }

        procesMap();

        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        meshGen.GenerateMesh(map, 1);
    }


    void RandomFillMap()
    {
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = (pseudoRandom.Next(0, 1000) < randomFillPercent) ? 1 : 0;
                }
            }
        }
    }

    void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);

                if (neighbourWallTiles > 4)
                    map[x, y] = 1;
                else if (neighbourWallTiles <4)
                    map[x, y] = 0;

            }
        }
    }

    List<cord> getRegionTiles(int startX, int startY)
    {
        List<cord> tiles = new List<cord>();
        int[,] mapFlogs = new int[width, height];
        int tiletype = map[startX, startY];

        Queue<cord> queue = new Queue<cord>();
        queue.Enqueue(new cord(startX, startY));
        mapFlogs[startX, startY] = 1;

        while (queue.Count > 0)
        {
            cord tile = queue.Dequeue();
            tiles.Add(tile);

            for(int x = tile.tileX-1; x <= tile.tileX + 1; x++)
            {
                for (int y = tile.tileY-1; y <= tile.tileY + 1; y++)
                {
                    if(isInMapRabge(x,y) && (y==tile.tileY || x == tile.tileX))
                    {
                        if(mapFlogs[x,y] == 0 && map[x,y] == tiletype)
                        {
                            mapFlogs[x, y] = 1;
                            queue.Enqueue(new cord(x, y));
                        }
                    }
                }
            }
        }

        return tiles;
    }

    void procesMap()
    {
        List<List<cord>> wallRegions = getRegion(1);
        foreach(List<cord> walRegion in wallRegions)
        {
            if (walRegion.Count < wallTresholdSize)
            {
                foreach(cord tile in walRegion)
                {
                    map[tile.tileX, tile.tileY] = 0;
                }
            }
        }

        List<Room> survivingRooms = new List<Room>();

        List<List<cord>> roomRegions = getRegion(0);
        foreach (List<cord> roomRegion in roomRegions)
        {
            if (roomRegion.Count < roomTresholdSize)
            {
                foreach (cord tile in roomRegion)
                {
                    map[tile.tileX, tile.tileY] = 1;
                }
            }
            else
            {
                survivingRooms.Add(new Room(roomRegion, map));
            }
        }

        survivingRooms.Sort();
        survivingRooms[0].isMainRoom = true;
        survivingRooms[0].isAssessableFromMainRoom = true;
        connectClosestRooms(survivingRooms);
    }

    void connectClosestRooms(List<Room> allRooms,bool forceAccessibilityFromMainRoom = false)
    {
        List<Room> roomListA = new List<Room>();
        List<Room> roomListB = new List<Room>();

        if (forceAccessibilityFromMainRoom)
        {
            foreach(Room room in allRooms)
            {
                if (room.isAssessableFromMainRoom)
                {
                    roomListB.Add(room);
                }
                else
                {
                    roomListA.Add(room);
                }
            }
        }
        else
        {
            roomListA = allRooms;
            roomListB = allRooms;
        }

        int bestDistance = 0;
        cord bestTileA = new cord();
        cord bestTileB = new cord();
        Room bestRoomA = new Room();
        Room bestRoomB = new Room();
        bool possibleConectionFound = false;

        foreach(Room roomA in roomListA)
        {
            if (!forceAccessibilityFromMainRoom)
            {
                possibleConectionFound = false;
                if (roomA.connectedRoom.Count > 0)
                {
                    continue;
                }
            }
            foreach(Room roomB in roomListB)
            {
                if (roomA == roomB || roomA.isConnected(roomB))
                    continue;
                        
                for(int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA++)
                {
                    for (int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB++)
                    {
                        cord tileA = roomA.edgeTiles[tileIndexA];
                        cord tileB = roomB.edgeTiles[tileIndexB];
                        int distanceBetweenRooms = (int)(Mathf.Pow(tileA.tileX - tileB.tileX, 2) + Mathf.Pow(tileA.tileY- tileB.tileY, 2));

                        if (distanceBetweenRooms < bestDistance || !possibleConectionFound)
                        {
                            bestDistance = distanceBetweenRooms;
                            possibleConectionFound = true;
                            bestTileA = tileA;
                            bestTileB = tileB;
                            bestRoomA = roomA;
                            bestRoomB = roomB;
                        }
                    }
                }
            }

            if (possibleConectionFound && !forceAccessibilityFromMainRoom)
            {
                createPassage(bestRoomA, bestRoomB,bestTileA,bestTileB);
            }
        }
        if (possibleConectionFound && forceAccessibilityFromMainRoom)
        {
            createPassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            connectClosestRooms(allRooms, true);
        }

        if (!forceAccessibilityFromMainRoom)
        {
            connectClosestRooms(allRooms, true);
        }
    }

    void createPassage(Room roomA,Room roomB,cord tileA,cord tileB)
    {
        Room.connectRooms(roomA, roomB);

        List<cord> line = getLine(tileA, tileB);
        foreach(cord elem in line)
        {
            drawCircle(elem, radius);
        }
    }

    void drawCircle(cord c,int r)
    {
        for(int x = -r; x <= r; x++)
        {
            for (int y = -r; y <= r; y++)
            {
                if (x * x + y * y <= r * r)
                {
                    int realX = c.tileX + x;
                    int realY = c.tileY + y;
                    if (isInMapRabge(realX, realY))
                    {
                        map[realX, realY] = 0;
                    }
                }
            }
        }
    }

    List<cord> getLine(cord from,cord to)
    {
        List<cord> line = new List<cord>();
        int x = from.tileX;
        int y = from.tileY;

        int dx = to.tileX - from.tileX;
        int dy = to.tileY - from.tileY;
        bool inverted = false;
        int step = Math.Sign(dx);
        int gradientStep = Math.Sign(dy);

        int longest = Mathf.Abs(dx);
        int shortest = Mathf.Abs(dy);
        if (longest < shortest)
        {
            inverted = true;
            longest = Mathf.Abs(dy);
            shortest = Mathf.Abs(dx);

            step = Math.Sign(dy);
            gradientStep = Math.Sign(dx);
        }

        int gradientAccumulation = longest / 2;
        for(int i = 0; i < longest; i++)
        {
            line.Add(new cord(x, y));

            if (inverted)
            {
                y += step;
            }
            else
            {
                x += step;
            }

            gradientAccumulation += shortest;
            if(gradientAccumulation >= longest)
            {
                if (inverted)
                {
                    x += gradientStep;
                }
                else
                {
                    y += gradientStep;
                }
                gradientAccumulation -= longest;
            }
        }
        return line;
    }

    Vector3 cordToWordPoint(cord tile)
    {
        return new Vector3(-width / 2 + .5f + tile.tileX, 2, -height / 2 + .5f + tile.tileY);
    }

    List<List<cord>> getRegion(int tiltType)
    {
        List<List<cord>> regions = new List<List<cord>>();
        int[,] mapFlogs = new int[width, height];

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                if(mapFlogs[x,y] == 0 && map[x,y] == tiltType)
                {
                    List<cord> region = getRegionTiles(x, y);
                    regions.Add(region);

                    foreach(cord tile in region)
                    {
                        mapFlogs[tile.tileX, tile.tileY] = 1;
                    }
                }
            }
        }
        return regions;
    }

    public bool isInMapRabge(int x,int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (isInMapRabge(neighbourX,neighbourY))
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                   wallCount++;
                }
            }
        }

        return wallCount;
    }

    struct cord
    {
        public int tileX;
        public int tileY;

        public cord(int x,int y)
        {
            tileX = x;
            tileY = y;
        }
    }

    class Room :IComparable<Room>
    {
        public List<cord> tiles;
        public List<cord> edgeTiles;
        public List<Room> connectedRoom;
        public int roomSize;
        public bool isAssessableFromMainRoom;
        public bool isMainRoom;

        public Room()
        {
        }

        public Room(List<cord> roomTiles, int[,] map)
        {
            tiles = roomTiles;
            roomSize = roomTiles.Count;
            connectedRoom = new List<Room>();
            edgeTiles = new List<cord>();  

            foreach(cord tile in tiles)
            {
                for(int x= tile.tileX - 1; x < tile.tileX + 1; x++)
                {
                    for (int y = tile.tileY; y < tile.tileY+1; y++)
                    {
                        if(x == tile.tileX || y == tile.tileY)
                        {
                            if(x >= 0 && x < map.GetLength(0) && y >= 0 && y < map.GetLength(1))
                            {
                                if(map[x,y] == 1)
                                {
                                    edgeTiles.Add(tile);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void setAccessableFromMainRoom()
        {
            if (!isAssessableFromMainRoom)
            {
                isAssessableFromMainRoom = true;
                foreach(Room room in connectedRoom)
                {
                    room.setAccessableFromMainRoom();
                }
            }
        }

        public static void connectRooms(Room roomA,Room roomB)
        {
            if (roomA.isAssessableFromMainRoom)
            {
                roomB.setAccessableFromMainRoom();
            }
            else
            {
                if (roomB.isAssessableFromMainRoom)
                {
                    roomA.setAccessableFromMainRoom();
                }
            }
            roomA.connectedRoom.Add(roomB);
            roomB.connectedRoom.Add(roomA);
        }

        public bool isConnected(Room room)
        {
            return connectedRoom.Contains(room);
        }

        int IComparable<Room>.CompareTo(Room other)
        {
            return other.roomSize.CompareTo(roomSize);
        }
    }

}
