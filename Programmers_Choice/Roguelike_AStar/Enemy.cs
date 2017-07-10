using System;
using FSPG;
using Roguelike_map;

namespace RoguelikPlayer
{
    class Enemy
    {
        bool[,] sight;
        Map map;

        // window screen
        private const int defaultScreenWidth = 80;
        private const int defaultScreenHeight = 24;
        private int mapWidth;
        private int mapHeight;

        // position
        private int x;
        private int y;

        private int prevX;
        private int prevY;

        // sight range
        private int sightX;
        private int sightY;
        private const int sightWidth = 5;
        private const int sightHeight = 5;

        // status
        private int level;
        private int hp;
        private int ap;
        private int dp;

        private void init(int screenWidth, int screenHeight, int level, Map m, bool[,] sight)
        {
            map = m;
            this.sight = sight;
            mapWidth = screenWidth - 1;
            mapHeight = screenHeight - 2;

            x = Utility.Rand() % mapWidth;
            y = Utility.Rand() % mapHeight;

            while (!m.checkGround(x,y) || (x >= mapWidth/2 - 6 && x <= mapWidth/2 + 10 && y >= mapHeight / 2 - 6 && y <= mapHeight / 2 + 10))
            {
                x = Utility.Rand() % mapWidth;
                y = Utility.Rand() % mapHeight;
            }

            prevX = x;
            prevY = y;

            sightX = x - 2;
            sightY = y - 2;

            this.level = level;
            hp = Utility.Rand() % (level + 1) + 1;
            ap = Utility.Rand() % (level + 1) + 1;
            dp = Utility.Rand() % (level + 1) + 1;
        }

        /**
         * reset method
         * */
        public void init()
        {
            x = mapWidth / 2 + 2;
            y = mapHeight / 2 + 2;

            sightX = x - 2;
            sightY = y - 2;

            prevX = x;
            prevY = y;

            level++;
        }

        public Enemy()
        {
            init(defaultScreenWidth, defaultScreenHeight, 1, null, null);
        }

        public Enemy(int screenWidth, int screenHeight, int level, Map map, bool[,] sight)
        {
            init(screenWidth, screenHeight, level, map, sight);
        }

        //public void moveUp() { y--; sightY--; food--; }
        //public void moveDown() { y++; sightY++; food--; }
        //public void moveLeft() { x--; sightX--; food--; }
        //public void moveRight() { x++; sightX++; food--; }

        public int getX() { return x; }
        public int getY() { return y; }
        public int getLevel() { return level; }
        public void setX(int x) { this.x = x; }
        public void setY(int y) { this.y = y; }
        public void setLevel(int level) { this.level = level; }

        //public int getFood() { return food; }
        //public void setFood(int food) { this.food = food; }
        //public void addFood(int food) { this.food += food; }

        public int getSightX() { return sightX; }
        public int getSightY() { return sightY; }
        public void setSightX(int x) { sightX = x; }
        public void setSightY(int y) { sightY = y; }

        public int getSightWidth() { return sightWidth; }
        public int getSightHeight() { return sightHeight; }

        public void Draw()
        {
            Console.SetCursorPosition(prevX, prevY);
            if (sight != null)
            {
                if (!sight[prevX, prevY])
                    Console.Write(" ");
                else
                    Console.Write(map.getTile(prevX, prevY));
            }
            


            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(x, y);
            Console.Write("E");
            Console.ForegroundColor = ConsoleColor.Gray;

            prevX = x;
            prevY = y;
        }
    }
}
