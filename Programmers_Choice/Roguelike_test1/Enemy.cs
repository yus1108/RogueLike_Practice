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
        private bool isAlive;
        private int level;
        private float hp;
        private float ap;
        private float dp;

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

            this.isAlive = true;
            this.level = level;
            hp = Utility.Rand() % (level + 1) + level;
            ap = Utility.Rand() % (level + 1) + level;
            dp = Utility.Rand() % (level + 1) + level;
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

            this.isAlive = true;
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
        public void setX(int x) { this.x = x; sightX = x - 2; }
        public void setY(int y) { this.y = y; sightY = y - 2; }
        public void setLevel(int level) { this.level = level; }

        //public int getFood() { return food; }
        //public void setFood(int food) { this.food = food; }
        //public void addFood(int food) { this.food += food; }

        public int getSightX() { return sightX; }
        public int getSightY() { return sightY; }
        public void setSightX(int x) { sightX = x; }
        public void setSightY(int y) { sightY = y; }

        public float requestHP() { return hp; }
        public float requestAP() { return ap; }
        public float requestDP() { return dp; }

        public int getSightWidth() { return sightWidth; }
        public int getSightHeight() { return sightHeight; }

        public void SetHp(float hp) { this.hp = hp; }
        public void SetAP(float ap) { this.ap = ap; }
        public void SetDP(float dp) { this.dp = dp; }

        public bool GetIsAlive()
        {
            return isAlive;
        }

        public void Damaged(Player p)
        {
            float probDamage = p.requestAP() / this.dp;
            float prng = (Utility.Rand() % 101) / 100.0f;
            if (probDamage > prng)
            {
                if (probDamage >= 1)
                    hp -= (probDamage - prng);
                else
                    hp -= prng;

                if (hp <= 0)
                {
                    isAlive = false;
                    int food = Utility.Rand() % (50 * level);
                    if (food > 50)
                    {
                        int div = food / 50;
                        food = 50 + 10 * (div - 1);
                    }
                    Console.SetCursorPosition(defaultScreenWidth / 2 - 5, defaultScreenHeight - 2);
                    Console.Write("Enemy Died!!! You earned " + food + " foods");
                    p.addFood(food);
                } else
                {
                    Console.SetCursorPosition(defaultScreenWidth / 2 - 5, defaultScreenHeight - 2);
                    Console.Write("Enemy's hp: " + hp + ", " + prng + " damaged");
                }
                
            } else
            {
                Console.SetCursorPosition(defaultScreenWidth / 2 - 5, defaultScreenHeight - 2);
                Console.Write("Enemy Evaded attack !!!");
            }

            
        }

        public void Draw()
        {
            Console.SetCursorPosition(prevX, prevY);
            if(!sight[prevX,prevY])
                Console.Write(" ");
            else
                Console.Write(map.getTile(prevX, prevY));


            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(x, y);
            Console.Write("E");
            Console.ForegroundColor = ConsoleColor.Gray;

            prevX = x;
            prevY = y;
        }
    }
}
