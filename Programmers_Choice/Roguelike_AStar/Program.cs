using System;
using Roguelike_map;
using RoguelikPlayer;
using System.Collections.Generic;
using FSPG;

namespace Roguelike_AStar
{
    class Program
    {
        private const int screenWidth = 80;
        private const int screenHeight = 24;
        private const int mapWidth = screenWidth - 1;
        private const int mapHeight = screenHeight - 2;

        static void Main(string[] args)
        {
            Console.SetWindowSize(screenWidth, screenHeight);
            Console.SetBufferSize(screenWidth, screenHeight);
            Console.CursorVisible = false;

            List<Enemy> enemies = new List<Enemy>();

            int level = 1;

            while(true)
            {
                Console.Clear();
                Map map = new Map(screenWidth, screenHeight, level);
                int co = Utility.Rand() % (map.countRoom + 1);
                for (int i = 0; i < co; i++)
                    enemies.Add(new Enemy(screenWidth, screenHeight, level, map, null));
                level++;

                drawMap(map);
                drawPoint(mapWidth / 2 + 1, mapHeight / 2 + 1, '@', ConsoleColor.Red);
                drawPoint(map.GetNextX(), map.GetNextY(), 'N', ConsoleColor.Red);

                for (int i = 0; i < enemies.Count; i++)
                    enemies[i].Draw();

                Point p1 = new Point(map.GetNextX(), map.GetNextY());
                Point p2 = new Point(mapWidth / 2 + 1, mapHeight / 2 + 1);

                findShortestD(map, mapWidth / 2 + 1, mapHeight / 2 + 1);

                ConsoleKeyInfo input = Console.ReadKey(true);

                for (int i = 0; i < enemies.Count; i++)
                    enemies.RemoveAt(i--);
            }
        }

        private static void findShortestD(Map map, int v1, int v2)
        {
        }


        public static void drawMap(Map map)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    Console.SetCursorPosition(i, j);
                    Console.Write(map.getTile(i, j));
                }
            }

        }

        public static void drawPoint(int x, int y, char t, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y);
            Console.Write(t);
        }
    }
}
