using System;
using FSPG;
using System.Collections.Generic;

namespace Roguelike_FinalVersion
{
    public class Enemy
    {
        private Game mGame;

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

        private bool damageTaken;
        private float prng;
        private float probDamage;
        private int food;

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

            damageTaken = false;

        }

        public Enemy(int screenWidth, int screenHeight, Game game)
        {
            mGame = game;
            init(screenWidth, screenHeight, game.GetPlayer().getLevel(), game.GetMap(), game.GetPlayer().getSight());
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
            probDamage = p.requestAP() / this.dp;
            prng = (Utility.Rand() % 101) / 100.0f;
            damageTaken = true;

            if (probDamage > prng)
            {
                if (probDamage >= 1)
                    hp -= (probDamage - prng);
                else
                    hp -= prng;

                if (hp <= 0)
                {
                    isAlive = false;
                    food = Utility.Rand() % (50 * level);
                    if (food > 50)
                    {
                        int div = food / 50;
                        food = 50 + 10 * (div - 1);
                    }
                    p.addFood(food);
                    mGame.dead = true;
                    mGame.reward = food;
                }
                
            }

            
        }

        public void Draw()
        {
            ConsoleColor prev = Console.ForegroundColor;

            Console.SetCursorPosition(prevX, prevY);
            if(!sight[prevX,prevY])
                Console.Write(" ");
            else
                Console.Write(map.getTile(prevX, prevY));

            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(x, y);
            Console.Write("E");

            if (damageTaken)
            {
                float dmg;
                if (probDamage > prng)
                {
                    if (hp > 0)
                    {
                        if (probDamage >= 1)
                            dmg = ((int)((probDamage - prng) * 100)) / 100.0f;
                        else
                            dmg = prng;

                        Console.SetCursorPosition(defaultScreenWidth / 2 - 5, defaultScreenHeight - 2);
                        Console.Write("Enemy's hp: " + (((int)(hp * 100)) / 100.0f) + ", " + dmg + " damaged");
                    }

                }
                else
                {
                    Console.SetCursorPosition(defaultScreenWidth / 2 - 5, defaultScreenHeight - 2);
                    Console.Write("Enemy Evaded attack !!!");
                }
                damageTaken = false;
            }
                

            Console.ForegroundColor = prev;

            prevX = x;
            prevY = y;
        }

        public bool Update(int index)
        {
            Player p = mGame.GetPlayer();
            List<Enemy> e = mGame.GetEnemies();

            int direction = Utility.Rand() % 4;
            int x = getX();
            int y = getY();

            bool found = false;
            for (int j = getSightX(); j < getSightX() + getSightWidth(); j++)
            {
                for (int k = getSightY(); k < getSightY() + getSightHeight(); k++)
                {
                    if (j == getX() && k == getY())
                        continue;

                    if (j == p.getX() && k == p.getY())
                    {
                        found = true;
                    }

                }
            }

            if (found)
            {
                int xDir = getX() - p.getX();  // negative player is right to enemy
                int yDir = getY() - p.getY();  // negative player is bottom to enemy

                int decXY = Utility.Rand() % 2;

                if (decXY == 0)
                {
                    if (xDir < 0)
                    {
                        direction = 0;
                    }
                    else if (xDir == 0)
                    {
                        if (yDir < 0)
                        {
                            direction = 2;
                        }
                        else
                        {
                            direction = 3;
                        }

                    }
                    else
                    {
                        direction = 1;
                    }
                }
                else if (decXY == 1)
                {
                    if (yDir < 0)
                    {
                        direction = 2;
                    }
                    else if (yDir == 0)
                    {
                        if (xDir < 0)
                        {
                            direction = 0;

                        }
                        else
                        {
                            direction = 1;
                        }
                    }
                    else
                    {
                        direction = 3;
                    }
                }
            }

            found = false;

            switch (direction)
            {
                case 0:
                    x += 1;
                    break;
                case 1:
                    x -= 1;
                    break;
                case 2:
                    y += 1;
                    break;
                case 3:
                    y -= 1;
                    break;
                default:
                    break;
            }

            bool collide = false;
            if (map.checkGround(x, y) && !(x == p.getX() && y == p.getY()))
            {
                for (int j = 0; j < e.Count; j++)
                {
                    if (j == index)
                        continue;

                    if (x == e[j].getX() && y == e[j].getY())
                    {
                        collide = true;
                    }

                }

                if (!collide)
                {
                    setX(x);
                    setY(y);
                }
            }
            else if (x == p.getX() && y == p.getY())
            {
                p.Damaged(this);
                if (!GetIsAlive())
                {
                    e.RemoveAt(index);
                    return true;
                }
                    
            }

            return false;
        }
    }
}
