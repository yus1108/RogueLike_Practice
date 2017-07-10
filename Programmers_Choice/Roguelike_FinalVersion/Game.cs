using System;
using FSPG;
using System.Collections.Generic;
using System.Threading;

namespace Roguelike_FinalVersion
{
    public class Game
    {
        private const int screenWidth = 80;
        private const int screenHeight = 24;
        private const int mapWidth = screenWidth - 1;
        private const int mapHeight = screenHeight - 2;
        private int mNumEnemy;

        private Player mPlayer;
        private Map mMap;
        private List<Enemy> mEnemies;
        public List<bool> mEnemySeen;

        private bool quit;

        private bool mNext;

        public bool dead;
        public int reward;

        public Game()
        {
            // do nothing
        }
        internal void Menu()
        {
            // TODO: make a menu for this game
        }

        internal void Init()
        {
            Utility.SetupWindow("Roguelike v1.0", screenWidth, screenHeight);
            Utility.EOLWrap(false);
            Console.CursorVisible = false;

            mPlayer = new Player(screenWidth, screenHeight, this);
            mMap = new Map(screenWidth, screenHeight, this);
            mEnemies = new List<Enemy>();
            mEnemySeen = new List<bool>();
            mNext = false;
            quit = false;
            dead = false;

            mNumEnemy = Utility.Rand() % (mMap.countRoom + 1);

            for (int i = 0; i < mNumEnemy; i++)
            {
                mEnemies.Add(new Enemy(screenWidth, screenHeight, this));
                mEnemySeen.Add(false);
            }

            // initial satus to start the game
            mPlayer.setFood(calcGroundGrids(mMap, mapWidth, mapHeight));

            // initial map and player rendering
            Draw();
        }

        public void Run()
        {
            while (!quit && mPlayer.GetIsAlive())
            {
                // read input
                ConsoleKeyInfo input = Console.ReadKey(true);
                switch (input.KeyChar)
                {
                    case 'q':
                        quit = true;
                        break;
                    default:
                        break;
                }

                // update
                Update(input);

                // render
                Draw();

                // timestep
                Thread.Sleep(100);
            }
        }

        public void End()
        {
            Console.SetCursorPosition(0, screenHeight - 1);
            Console.Write("Gmae Over!!!");
            Console.ReadLine();
        }



        private void Reset()
        {
            Console.Clear();
            mPlayer.Reset();
            mMap = new Map(screenWidth, screenHeight, this);

            for (int i = 0; i < mEnemies.Count; i++)
            {
                mEnemies.RemoveAt(i);
                mEnemySeen.RemoveAt(i--);
            }

            mNumEnemy = Utility.Rand() % (mMap.countRoom + 1);
            for (int i = 0; i < mNumEnemy; i++)
            {
                mEnemies.Add(new Enemy(screenWidth, screenHeight, this));
                mEnemySeen.Add(false);
            }

            mNext = true;
        }

        private void Draw()
        {
            Utility.LockConsole(true);
            Console.Clear();
            mMap.Draw();
            mPlayer.Draw();
            if (!mNext)
            {
                for (int i = 0; i < mEnemies.Count; i++)
                    EnemyDraw(mEnemySeen, mEnemies, i);        //enemies[i].draw();
            }
            if (dead)
            {
                ConsoleColor prev = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(screenWidth / 2 - 5, screenHeight - 2);
                Console.Write("Enemy Died!!! You earned " + reward + " foods");
                dead = false;
                Console.ForegroundColor = prev;
            }
            
            Utility.LockConsole(false);
        }

        public static void EnemyDraw(List<bool> b, List<Enemy> e, int i)
        {
            if (b[i])
                e[i].Draw();
        }

        private void Update(ConsoleKeyInfo input)
        {
            if (!mPlayer.Update(input))
                UpdateEnemy(mEnemies);

            mNext = false;
            if (mMap.checkNextLevel(mPlayer.getX(), mPlayer.getY()))
                Reset();
        }

        public static void UpdateEnemy(List<Enemy> e)
        {
            for (int i = 0; i < e.Count; i++)
                if (e[i].Update(i))
                {
                    i--;
                }
        }

        public int calcGroundGrids(Map map, int width, int height)
        {
            int grid = 0;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (map.GetMap(i, j) != Map.tile.Empty)
                        grid++;
                }
            }

            return grid;
        }

        public Player GetPlayer() { return mPlayer; }
        public Map GetMap() { return mMap; }
        public List<Enemy> GetEnemies() { return mEnemies; }
    }
}