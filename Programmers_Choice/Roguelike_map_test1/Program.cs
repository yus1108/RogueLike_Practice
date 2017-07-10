using System;
using FSPG;

namespace Roguelike_map_test1
{
    enum direction
    {
        North,
        West,
        South,
        East
    }
    enum tile
    {
        Empty,
        Ground,
        Door,
        NextLevel
    }

    class Program
    {
        private const int screenWidth = 80;
        private const int screenHeight = 24;
        private const int mapWidth = 79;
        private const int mapHeight = 22;
        private const int minSize = 4;
        private const int maxSize = 6;

        static void Main(string[] args)
        {
            //
            // console settings
            Console.SetWindowSize(screenWidth, screenHeight);
            Console.SetBufferSize(screenWidth, screenHeight);
            Console.CursorVisible = false;

            //
            // init
            int counter = 0;
            // tile arrays
            tile[,] map = new tile[mapWidth, mapHeight];
            // the first room
            int roomWidth = minSize;
            int roomHeight = minSize;
            int roomX = mapWidth / 2;
            int roomY = mapHeight / 2;
            // drawing room
            for (int i = roomX; i < roomX + roomWidth; i++)
            {
                for (int j = roomY; j < roomY + roomHeight; j++)
                {
                    map[i, j] = tile.Ground;
                }
            }
            // door position
            int doorX;
            int doorY;
            // area for room and corridor
            int areaWidth;
            int areaHeight;
            int areaX;
            int areaY;
            // determine area for corridor
            bool drawCorridor = true;
            direction doorDir = 0;
            // initialize last room to add next level
            int lastRoomX = 0;
            int lastRoomY = 0;
            int lastRoomWidth = 0;
            int lastRoomHeight = 0;




            //
            //
            // game looping
            while (true)
            {

                // input
                // impossible to continue creating map
                if (counter == 1000)
                    break;
                    


                //
                // update

                // choose direction of door 
                if (drawCorridor)
                    doorDir = (direction)(Utility.Rand() % 4);

                // make area for room and corridor
                areaWidth = Utility.Rand() % (maxSize - minSize + 1) + minSize;
                areaHeight = Utility.Rand() % (maxSize - minSize + 1) + minSize;

                // positioning door and area
                switch (doorDir)
                {
                    case direction.North:
                        doorX = Utility.Rand() % roomWidth + roomX;
                        doorY = roomY;
                        areaX = doorX - (areaWidth / 2);
                        areaY = doorY - areaHeight;
                        break;
                    case direction.South:
                        doorX = Utility.Rand() % roomWidth + roomX;
                        doorY = roomY + roomHeight - 1;
                        areaX = doorX - (areaWidth / 2);
                        areaY = doorY + 1;
                        break;
                    case direction.East:
                        doorX = roomX + roomWidth - 1;
                        doorY = Utility.Rand() % roomHeight + roomY;
                        areaX = doorX + 1;
                        areaY = doorY - (areaHeight / 2);
                        break;
                    case direction.West:
                        doorX = roomX;
                        doorY = Utility.Rand() % roomHeight + roomY;
                        areaX = doorX - areaWidth;
                        areaY = doorY - (areaHeight / 2);
                        break;
                    default:
                        doorX = 0;
                        doorY = 0;
                        areaX = 0;
                        areaY = 0;
                        break;
                }

                // check obstacles in area
                if (areaX + areaWidth >= mapWidth || areaY + areaHeight >= mapHeight)
                {
                    Console.SetCursorPosition(0, screenHeight - 1);
                    Console.Write("check fail " + doorDir);
                    counter++;
                    drawCorridor = true;
                    continue;
                }
                else if (areaX < 0 || areaY < 0)
                {
                    Console.SetCursorPosition(0, screenHeight - 1);
                    Console.Write("check fail" + doorDir);
                    counter++;
                    drawCorridor = true;
                    continue;
                }

                bool check = true;
                for (int i = areaX; i < areaX + areaWidth; i++)
                {
                    for (int j = areaY; j < areaY + areaHeight; j++)
                    {
                        switch (map[i, j])
                        {
                            case tile.Empty:
                                break;
                            case tile.Ground:
                                check = false;
                                break;
                            case tile.Door:
                                check = false;
                                break;
                            default:
                                break;
                        }
                    }
                }

                if (!check)
                {
                    Console.SetCursorPosition(0, screenHeight - 1);
                    Console.Write("check fail" + doorDir);
                    counter++;
                    drawCorridor = true;
                    continue;
                }

                 if (drawCorridor && (doorDir == direction.East || doorDir == direction.West) )
                {
                    areaY = doorY;
                    areaHeight = 1;
                }
                if (drawCorridor && (doorDir == direction.North || doorDir == direction.South))
                {
                    areaX = doorX;
                    areaWidth = 1;
                }

                // locate doors and rooms and corridors
                map[doorX, doorY] = tile.Door;
                for (int i = areaX; i < areaX + areaWidth; i++)
                {
                    for (int j = areaY; j < areaY + areaHeight; j++)
                    { 
                        map[i, j] = tile.Ground;
                    }
                }   

                // saving previous area to continue drawing new rooms.
                roomWidth = areaWidth;
                roomHeight = areaHeight;
                roomX = areaX;
                roomY = areaY;

                if (!drawCorridor)
                {
                    lastRoomX = areaX + 1;
                    lastRoomY = areaY + 1;
                    lastRoomWidth = areaWidth - 2;
                    lastRoomHeight = areaHeight - 2;
                }

                drawCorridor = !drawCorridor;



                //
                // rendering
                Console.Clear();
                for (int i = 0; i < mapWidth; i++)
                {
                    for (int j = 0; j < mapHeight; j++)
                    {
                        Console.SetCursorPosition(i, j);
                        switch (map[i, j])
                        {
                            case tile.Empty:
                                break;
                            case tile.Ground:
                                Console.Write("#");
                                break;
                            case tile.Door:
                                Console.Write("D");
                                break;
                            default:
                                break;
                        }
                    }
                }


                //
                // timestep
                counter = 0;

            }

            int nextX = Utility.Rand() % lastRoomWidth + lastRoomX;
            int nextY = Utility.Rand() % lastRoomHeight + lastRoomY;
            //Console.SetCursorPosition(nextX, nextY);
            //Console.Write("N");
            map[nextX, nextY] = tile.NextLevel;
            Console.Clear();
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    Console.SetCursorPosition(i, j);
                    switch (map[i, j])
                    {
                        case tile.Empty:
                            break;
                        case tile.Ground:
                            Console.Write("#");
                            break;
                        case tile.Door:
                            Console.Write("D");
                            break;
                        case tile.NextLevel:
                            Console.Write("N");
                            break;
                        default:
                            break;
                    }
                }
            }
            ConsoleKeyInfo input = Console.ReadKey(true);
        }
    }
}
