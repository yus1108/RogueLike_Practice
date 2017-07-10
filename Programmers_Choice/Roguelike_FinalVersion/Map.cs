using System;
using FSPG;

namespace Roguelike_FinalVersion
{
    public class Map
    {
        private Game mGame;

        enum direction
        {
            North,
            West,
            South,
            East
        }
        public enum tile
        {
            Empty,
            Ground,
            Door,
            NextLevel,
            Treasure
        }

        public int countRoom;

        private const int defaultScreenWidth = 80;
        private const int defaultScreenHeight = 24;
        private const int minSize = 4;
        private const int maxSize = 6;

        private int mapWidth;
        private int mapHeight;

        private tile[,] map;
        private int counter;

        // previous area
        private int roomWidth;
        private int roomHeight;
        private int roomX;
        private int roomY;

        // door position
        private int doorX;
        private int doorY;

        // area for room and corridor
        private int areaWidth;
        private int areaHeight;
        private int areaX;
        private int areaY;

        // determine area for corridor
        private bool drawCorridor;
        private direction doorDir;

        // position and area for the last room
        private int lastRoomX;
        private int lastRoomY;
        private int lastRoomWidth;
        private int lastRoomHeight;

        // position of entrance for next room
        private int nextX;
        private int nextY;

        // initialized all the variables necceesary in map generation
        public void init(int screenWidth, int screenHeight)
        {
            mapWidth = screenWidth - 1;
            mapHeight = screenHeight - 2;
            counter = 0;

            // assigning tile array with the default value
            map = new tile[mapWidth, mapHeight];

            // the first room where player starts
            roomWidth = minSize;
            roomHeight = minSize;
            roomX = mapWidth / 2;
            roomY = mapHeight / 2;

            // drawing the first room in the map
            drawRect(roomX, roomY, roomWidth, roomHeight);

            // begin with drawing corridor
            drawCorridor = true;
            doorDir = 0;

            // initialize last room to add next level
            lastRoomX = 0;
            lastRoomY = 0;
            lastRoomWidth = 0;
            lastRoomHeight = 0;

            countRoom = 0;
        }

        private void drawRect(int x, int y, int width, int height)
        {
            for (int i = x; i < x + width; i++)
            {
                for (int j = y; j < y + height; j++)
                {
                    map[i, j] = tile.Ground;
                }
            }
        }

        public Map(int screenWidth, int screenHeight, Game game)
        {
            // initialize
            mGame = game;
            init(screenWidth, screenHeight);

            // looping to generate procedural map
            generateLoop(mGame.GetPlayer().getLevel());

            // create an entrance to the next level
            nextX = makeRandom(lastRoomX, lastRoomX + lastRoomWidth);
            nextY = makeRandom(lastRoomY, lastRoomY + lastRoomHeight);
            map[nextX, nextY] = tile.NextLevel;
        }

        private void generateLoop(int level)
        {
            while (counter <= 1000)
            {
                // update
                if (drawCorridor)
                    doorDir = (direction)makeRandom(0, 3);

                // make area for room and corridor
                areaWidth = makeRandom(minSize, maxSize);
                areaHeight = makeRandom(minSize, maxSize);
                setPos(roomX, roomY, roomWidth, roomHeight);

                // check obstacles in area
                if (checkObstacles(areaX, areaY, areaWidth, areaHeight))
                {
                    counter++;
                    drawCorridor = true;
                    continue;
                }

                // set area for a corridor
                if (drawCorridor)
                    corridorSet(doorX, doorY);
                else
                    countRoom++;

                // locate doors and rooms and corridors
                map[doorX, doorY] = tile.Door;
                drawRect(areaX, areaY, areaWidth, areaHeight);

                // treasure
                updateTreasure(areaX, areaY, areaWidth, areaHeight, level);

                // saving previous area to continue drawing new rooms.
                setPrevRoom(areaX, areaY, areaWidth, areaHeight);

                // timestep
                counter = 0;
            }
        }

        public void setTile(int x, int y, tile ground) { map[x, y] = ground; }

        private void updateTreasure(int areaX, int areaY, int areaWidth, int areaHeight, int level)
        {
            if (!drawCorridor)
            {
                float prob = 1.0f / (Utility.Rand() % level + 2);
                float r = Utility.Rand() % 100;
                if (r <= prob * 100)
                    map[makeRandom(areaX, areaX + areaWidth - 1), makeRandom(areaY, areaY + areaHeight - 1)] = tile.Treasure;
            }
        }

        private void setPos(int x, int y, int width, int height)
        {
            switch (doorDir)
            {
                case direction.North:
                    doorX = makeRandom(x, x + width);
                    doorY = y;
                    areaX = doorX - (areaWidth / 2);
                    areaY = doorY - areaHeight;
                    break;
                case direction.South:
                    doorX = makeRandom(x, x + width);
                    doorY = y + height - 1;
                    areaX = doorX - (areaWidth / 2);
                    areaY = doorY + 1;
                    break;
                case direction.East:
                    doorX = x + width - 1;
                    doorY = makeRandom(y, y + height);
                    areaX = doorX + 1;
                    areaY = doorY - (areaHeight / 2);
                    break;
                case direction.West:
                    doorX = x;
                    doorY = makeRandom(y, y + height);
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
        }

        private int makeRandom(int min, int max)
        {
            return Utility.Rand() % (max - min + 1) + min;
        }

        private bool checkObstacles(int x, int y, int width, int height)
        {
            if (x + width >= mapWidth || y + height >= mapHeight)
                return true;
            else if (x < 0 || y < 0)
                return true;

            for (int i = x; i < x + width; i++)
            {
                for (int j = y; j < y + height; j++)
                {
                    switch (map[i, j])
                    {
                        case tile.Empty:
                            break;
                        case tile.Ground:
                            return true;
                        case tile.Door:
                            return true;
                        default:
                            break;
                    }
                }
            }
            return false;
        }

        private void corridorSet(int x, int y)
        {
            if (doorDir == direction.East || doorDir == direction.West)
            {
                areaY = y;
                areaHeight = 1;
            }
            if (doorDir == direction.North || doorDir == direction.South)
            {
                areaX = x;
                areaWidth = 1;
            }
        }

        private void setPrevRoom(int x, int y, int width, int height)
        {
            roomWidth = width;
            roomHeight = height;
            roomX = x;
            roomY = y;

            if (!drawCorridor)
            {
                lastRoomX = x + 1;
                lastRoomY = y + 1;
                lastRoomWidth = width - 2;
                lastRoomHeight = height - 2;
            }

            drawCorridor = !drawCorridor;
        }

        public bool checkGround(int x, int y)
        {
            // check out of boundaries
            if (x < 0 || x > mapWidth)
                return false;
            if (y < 0 || y > mapHeight)
                return false;

            // check ground
            if (map[x, y] != tile.Empty)
                return true;
            return false;
        }

        public bool checkNextLevel(int x, int y)
        {
            if (map[x, y] == tile.NextLevel)
                return true;
            return false;
        }

        public int GetNextX() { return nextX; }
        public int GetNextY() { return nextY; }

        public string getTile(int x, int y)
        {
            switch (map[x, y])
            {
                case tile.Empty:
                    return "";
                case tile.Ground:
                    return "#";
                case tile.Door:
                    return "D";
                case tile.NextLevel:
                    return "N";
                case tile.Treasure:
                    return "T";
                default:
                    break;
            }
            return "";
        }
        public tile GetMap(int x, int y) { return map[x, y]; }

        public void Draw()
        {
            bool[,] sight = mGame.GetPlayer().getSight();
            ConsoleColor prev = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Gray;
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    if (!sight[i, j])
                        continue;
                    Console.SetCursorPosition(i, j);
                    Console.Write(getTile(i, j));
                }
            }
            Console.ForegroundColor = prev;

        }
    }
}
