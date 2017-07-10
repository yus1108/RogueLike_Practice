using FSPG;
using System;
using System.Collections.Generic;

namespace Roguelike_FinalVersion
{
    public class Player
    {
        public enum item
        {
            noItem,
            food,
            healingPotion,
            elixir,
            weaponReinforcing,
            armorReinforcing
        }
        private item mItem;
        private int amount;

        private Game mGame;

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
        private bool[,] mSight;

        // status
        private bool isAlive;
        private int level;
        private int food;
        private float MaxHp;
        private float hp;
        private float ap;
        private float dp;

        private bool damageTaken;
        private float prng;
        private float probDamage;

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
            mItem = item.noItem;

            hp = Utility.Rand() % 11 + 5;
            MaxHp = hp;
            ap = Utility.Rand() % 2 + 1;
            dp = Utility.Rand() % 2 + 1;

            damageTaken = false;
            mSight = new bool[mapWidth, mapHeight];
            InitSight();
        }

        /**
         * reset method
         * */
        public void Reset()
        {
            isAlive = true;
            x = mapWidth / 2 + 2;
            y = mapHeight / 2 + 2;

            sightX = x - 2;
            sightY = y - 2;

            level++;
            mItem = item.noItem;
            damageTaken = false;
            InitSight();
        }

        private void InitSight()
        {
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    mSight[i, j] = false;
                }
            }
            for (int i = getSightX(); i < getSightX() + getSightWidth(); i++)
            {
                for (int j = getSightY(); j < getSightY() + getSightHeight(); j++)
                {
                    if (i >= 0 && i <= mapWidth && j >= 0 && j <= mapHeight)
                        mSight[i, j] = true;
                }
            }
        }

        public Player(int screenWidth, int screenHeight, Game game)
        {
            mGame = game;
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
            probDamage = e.requestAP() / this.dp;
            prng = (Utility.Rand() % 101) / 100.0f;
            damageTaken = true;

            if (probDamage > prng)
            {
                if (probDamage >= 1)
                    hp -= (probDamage - prng);
                else
                    hp -= prng;
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
        public bool[,] getSight() { return mSight; }
        public void setSightX(int x) { sightX = x; }
        public void setSightY(int y) { sightY = y; }
        public void setSight(bool[,] sight) { mSight = sight; }

        public int getSightWidth() { return sightWidth; }
        public int getSightHeight() { return sightHeight; }


        public void Draw()
        {
            ConsoleColor prev = Console.ForegroundColor;

            Console.SetCursorPosition(getX(), getY());
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write('@');

            if (damageTaken)
            {
                if (probDamage > prng)
                {
                    float dmg;

                    if (probDamage >= 1)
                        dmg = ((int)((probDamage - prng) * 100)) / 100.0f;
                    else
                        dmg = prng;
                    
                    Console.SetCursorPosition(0, defaultScreenHeight - 2);
                    Console.Write("Player Lost " + dmg + " hp!!!");
                }
                else
                {
                    Console.SetCursorPosition(0, defaultScreenHeight - 2);
                    Console.Write("Player Evaded attack !!!");
                }
                damageTaken = false;
            }

            DrawStatus();
            Console.ForegroundColor = prev;
        }

        private void DrawStatus()
        {
            Console.SetCursorPosition(0, defaultScreenHeight - 1);
            if (mItem != item.noItem)
            {
                Console.SetCursorPosition(0, defaultScreenHeight - 1);
                switch (mItem)
                {
                    case item.food:
                        Console.Write("Earned " + amount + " foods!");
                        break;
                    case item.healingPotion:
                        Console.Write("HP recovered by " + amount + "!");
                        break;
                    case item.elixir:
                        Console.Write("HP increased by " + amount + "!");
                        break;
                    case item.weaponReinforcing:
                        Console.Write("Weapon reinforced by  " + amount + "!");
                        break;
                    case item.armorReinforcing:
                        Console.Write("Armor reinforced by " + amount + "!");
                        break;
                    default:
                        break;
                }

                mItem = item.noItem;

            }
            else
            {
                Console.Write("Level: " + getLevel() + "\tFood: " + getFood() + "\tHP: " + getHP() + " / " + getMAXHP() + "\tAP: " + getAP() + "\tDP: " + getDP());
            }
        }

        public bool Update(ConsoleKeyInfo input)
        {
            Map map = mGame.GetMap();
            List<Enemy> enemies = mGame.GetEnemies();

            // Player Move with collision check
            bool collision = false;
            bool enemyCollision = false;
            switch (input.KeyChar)
            {
                case 'i':
                    if (map.checkGround(getX(), getY() - 1))
                    {
                        for (int i = 0; i < enemies.Count; i++)
                        {
                            if (enemies[i].getX() == getX() && enemies[i].getY() == getY() - 1)
                            {
                                enemyCollision = true;
                                enemies[i].Damaged(this);
                                if (!enemies[i].GetIsAlive())
                                    enemies.RemoveAt(i);
                                SubFood(1);
                            }
                        }
                        if (!enemyCollision)
                        {
                            moveUp();
                        }
                    } else
                        collision = true;
                    break;
                case 'j':
                    if (map.checkGround(getX() - 1, getY()))
                    {
                        for (int i = 0; i < enemies.Count; i++)
                        {
                            if (enemies[i].getX() == getX() - 1 && enemies[i].getY() == getY())
                            {
                                enemyCollision = true;
                                enemies[i].Damaged(this);
                                if (!enemies[i].GetIsAlive())
                                    enemies.RemoveAt(i);
                                SubFood(1);
                            }
                        }
                        if (!enemyCollision)
                        {
                            moveLeft();
                        }
                    } else
                        collision = true;
                    break;
                case 'l':
                    if (map.checkGround(getX() + 1, getY()))
                    {
                        for (int i = 0; i < enemies.Count; i++)
                        {
                            if (enemies[i].getX() == getX() + 1 && enemies[i].getY() == getY())
                            {
                                enemyCollision = true;
                                enemies[i].Damaged(this);
                                if (!enemies[i].GetIsAlive())
                                    enemies.RemoveAt(i);
                                SubFood(1);
                            }
                        }
                        if (!enemyCollision)
                        {
                            moveRight();
                        }
                    } else
                        collision = true;
                    break;
                case 'k':
                    if (map.checkGround(getX(), getY() + 1))
                    {
                        for (int i = 0; i < enemies.Count; i++)
                        {
                            if (enemies[i].getX() == getX() && enemies[i].getY() == getY() + 1)
                            {
                                enemyCollision = true;
                                enemies[i].Damaged(this);
                                if (!enemies[i].GetIsAlive())
                                    enemies.RemoveAt(i);
                                SubFood(1);
                            }
                        }
                        if (!enemyCollision)
                        {
                            moveDown();
                        }

                    }
                    else
                        collision = true;
                    break;
                case ' ':
                    SubFood(1);
                    break;
                default:
                    break;
            }


            SightUpdate(enemies);
            TreasureCollision(map);

            return collision;
        }

        private void SightUpdate(List<Enemy> enemies)
        {
            if (getFood() == 0)
                isAlive = false;

            // new sights added
            for (int i = getSightX(); i < getSightX() + getSightWidth(); i++)
            {
                for (int j = getSightY(); j < getSightY() + getSightHeight(); j++)
                {
                    if (i >= 0 && i < mapWidth && j >= 0 && j < mapHeight)
                    {
                        mSight[i, j] = true;
                    }
                }
            }
            // enemy in sight
            for (int k = 0; k < enemies.Count; k++)
            {
                mGame.mEnemySeen[k] = false;
                for (int i = 0; i < mapWidth; i++)
                {
                    for (int j = 0; j < mapHeight; j++)
                    {
                        if (mSight[i, j])
                        {
                            if (enemies[k].getX() == i && enemies[k].getY() == j)
                                mGame.mEnemySeen[k] = true;
                        }

                    }
                }
            }
        }
        private void TreasureCollision(Map map)
        {
            // collision with treasure
            if (map.GetMap(x, y) == Map.tile.Treasure)
            {
                mItem = (item)(Utility.Rand() % 5 + 1);
                amount = 0;

                switch (mItem)
                {
                    case item.food:
                        amount = 50 / (Utility.Rand() % (getLevel() + 1) + 1);
                        addFood(amount);
                        break;
                    case item.healingPotion:
                        amount = Utility.Rand() % 3 + 5;
                        SetHp(getHP() + amount);
                        if (getHP() > getMAXHP())
                            SetHp(getMAXHP());
                        break;
                    case item.elixir:
                        amount = Utility.Rand() % 3 + 1;
                        SetMaxHp(getMAXHP() + amount);
                        SetHp(getHP() + amount);
                        break;
                    case item.weaponReinforcing:
                        amount = Utility.Rand() % 2 + 1;
                        SetAP(getAP() + amount);
                        break;
                    case item.armorReinforcing:
                        amount = Utility.Rand() % 2 + 1;
                        SetDP(getDP() + amount);
                        break;
                    default:
                        break;
                }
                map.setTile(getX(), getY(), Map.tile.Ground);

            }
        }

        public item GetItem() { return mItem; }
        public void SetItem(item it) { mItem = it; }
    }
}
