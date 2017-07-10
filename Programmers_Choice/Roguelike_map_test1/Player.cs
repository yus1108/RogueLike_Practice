using FSPG;
using System;

namespace RoguelikPlayer
{
    class Player
    {
        // window screen
        private const int defaultScreenWidth = 80;
        private const int defaultScreenHeight = 24;
        private int mapWidth;
        private int mapHeight;

        // position
        private int x;
        private int y;

        // sight range
        private int sightX;
        private int sightY;
        private const int sightWidth = 5;
        private const int sightHeight = 5;

        // status
        private int level;
        private int food;
        private float hp;
        private float ap;
        private float dp;

        private void init(int screenWidth, int screenHeight)
        {
            mapWidth = screenWidth - 1;
            mapHeight = screenHeight - 2;

            x = mapWidth / 2 + 2;
            y = mapHeight / 2 + 2;

            sightX = x - 2;
            sightY = y - 2;

            food = 0;
            level = 1;

            hp = Utility.Rand() % 11 + 5;
            ap = Utility.Rand() % 2 + 1;
            dp = Utility.Rand() % 2 + 1;
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

            level++;
        }

        public Player()
        {
            init(defaultScreenWidth, defaultScreenHeight);
        }

        public Player(int screenWidth, int screenHeight)
        {
            init(screenWidth, screenHeight);
        }

        public void moveUp() { y--; sightY--; food--; }
        public void moveDown() { y++; sightY++; food--; }
        public void moveLeft() { x--; sightX--; food--; }
        public void moveRight() { x++; sightX++; food--; }

        public int getX() { return x; }
        public int getY() { return y; }
        public int getLevel() { return level; }
        public int getHP() { return (int)Math.Round(hp); }
        public int getAP() { return (int)Math.Round(ap); }
        public int getDP() { return (int)Math.Round(dp); }
        public void setX(int x) { this.x = x; }
        public void setY(int y) { this.y = y; }
        public void setLevel(int level) { this.level = level; }

        public int getFood() { return food; }
        public void setFood(int food) { this.food = food; }
        public void addFood(int food) { this.food += food; }

        public int getSightX() { return sightX; }
        public int getSightY() { return sightY; }
        public void setSightX(int x) { sightX = x; }
        public void setSightY(int y) { sightY = y; }

        public int getSightWidth() { return sightWidth; }
        public int getSightHeight() { return sightHeight; }
    }
}
