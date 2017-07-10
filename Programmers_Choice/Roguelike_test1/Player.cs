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
        private bool isAlive;
        private int level;
        private int food;
        private float MaxHp;
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
            isAlive = true;

            hp = Utility.Rand() % 11 + 5;
            MaxHp = hp;
            ap = Utility.Rand() % 2 + 1;
            dp = Utility.Rand() % 2 + 1;
        }

        /**
         * reset method
         * */
        public void init()
        {
            isAlive = true;
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
        public int getMAXHP() { return (int)Math.Round(MaxHp); }

        public float getHP() { return ((int)(hp * 100))/ 100.0f; }
        public int getAP() { return (int)Math.Round(ap); }
        public int getDP() { return (int)Math.Round(dp); }

        public float requestHP() { return hp; }
        public float requestAP() { return ap; }
        public float requestDP() { return dp; }


        public void setX(int x) { this.x = x; }
        public void setY(int y) { this.y = y; }
        public void setLevel(int level) { this.level = level; }
        public void SetMaxHp(float max) { this.MaxHp = max; }
        public void SetHp(float hp) { this.hp = hp; }
        public void SetAP(float ap) { this.ap = ap; }
        public void SetDP(float dp) { this.dp = dp; }

        public void Damaged(Enemy e)
        {
            float probDamage = e.requestAP() / this.dp;
            float prng = (Utility.Rand() % 101) / 100.0f;
            if (probDamage > prng)
            {
                Console.SetCursorPosition(0, defaultScreenHeight - 2);
                Console.Write("Player Lost " + prng + " hp!!!");
                if (probDamage >= 1)
                    hp -= (probDamage - prng);
                else
                    hp -= prng;
            }
            else
            {
                Console.SetCursorPosition(0, defaultScreenHeight - 2);
                Console.Write("Player Evaded attack !!!");
            }


            if (hp <= 0)
                isAlive = false;
        }

        public bool GetIsAlive()
        {
            return isAlive;
        }

        public int getFood() { return food; }
        public void setFood(int food) { this.food = food; }
        public void addFood(int food) { this.food += food; }
        public void SubFood(int food) { this.food -= food; }

        public int getSightX() { return sightX; }
        public int getSightY() { return sightY; }
        public void setSightX(int x) { sightX = x; }
        public void setSightY(int y) { sightY = y; }

        public int getSightWidth() { return sightWidth; }
        public int getSightHeight() { return sightHeight; }
    }
}
