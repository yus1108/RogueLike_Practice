using System;
using System.Threading;
using Roguelike_map;
using RoguelikPlayer;
using FSPG;
using System.Collections.Generic;

namespace Roguelike_test1
{
    class Program
    {
        enum item
        {
            food,
            healingPotion,
            elixir,
            weaponReinforcing,
            armorReinforcing
        }

        private static bool quit = false;
        private static bool treasure = false;
        private static bool food = false;
        private static bool hPotion = false;
        private static bool elix = false;
        private static bool wr = false;
        private static bool ar = false;


        private static int foodEarned;
        private const int screenWidth = 80;
        private const int screenHeight = 24;
        private const int mapWidth = screenWidth - 1;
        private const int mapHeight = screenHeight - 2;


        static void Main(string[] args)
        {
            Console.SetWindowSize(screenWidth, screenHeight);
            Console.SetBufferSize(screenWidth, screenHeight);
            Console.CursorVisible = false;

            bool gameOver = false;

            Player player = new Player(screenWidth, screenHeight);
            Map map = new Map(screenWidth, screenHeight, player.getLevel());
            List<Enemy> enemies = new List<Enemy>();
            List<bool> enemySeen = new List<bool>();

            int countE = Utility.Rand() % (map.countRoom + 1);

            // create FOW
            bool[,] sight = new bool[mapWidth, mapHeight];
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    sight[i, j] = false;
                }
            }
            for (int i = player.getSightX(); i < player.getSightX() + player.getSightWidth(); i++)
            {
                for (int j = player.getSightY(); j < player.getSightY() + player.getSightHeight(); j++)
                {
                    if (i >= 0 && i <= mapWidth && j >= 0 && j <= mapHeight)
                        sight[i, j] = true;
                }
            }

            for (int i = 0; i < countE; i++)
            {
                enemies.Add(new Enemy(screenWidth, screenHeight, player.getLevel(), map, sight));
                enemySeen.Add(false);
            }

            // initial satus to start the game
            player.setFood(((mapWidth * mapHeight) - calcGroundGrids(map, mapWidth, mapHeight)) / 3);

            

            // initial map and player rendering
            drawMap(map, sight);
            drawPlayer(player);
            //for (int i = 0; i < enemies.Count; i++)
            //    enemies[i].Draw();

            while (!quit && !gameOver && player.GetIsAlive())
            {
                // Input
                ConsoleKeyInfo input = Console.ReadKey(true);

                // Update
                if (player.getLevel() == 10)
                {
                    Console.Clear();
                    Console.WriteLine("congratulation!!!! You completed the game!!!");
                    break;
                }
                Utility.LockConsole(true);
                Console.Clear();
                // Player Move with collision check
                bool collision = false;
                switch (input.KeyChar)
                {
                    case 'i':
                        if (map.checkGround(player.getX(), player.getY() - 1))
                        {
                            for (int i = 0; i < enemies.Count; i++)
                            {
                                if (enemies[i].getX() == player.getX() && enemies[i].getY() == player.getY() - 1)
                                {
                                    collision = true;
                                    player.Damaged(enemies[i]);
                                    enemies[i].Damaged(player);
                                    if (!enemies[i].GetIsAlive())
                                        enemies.RemoveAt(i);
                                    player.SubFood(1);
                                }
                            }
                            if (!collision)
                            {
                                player.moveUp();
                                UpdateEnemy(enemies, player, map);

                            }
                        }

                        break;
                    case 'j':
                        if (map.checkGround(player.getX() - 1, player.getY()))
                        {
                            for (int i = 0; i < enemies.Count; i++)
                            {
                                if (enemies[i].getX() == player.getX() - 1 && enemies[i].getY() == player.getY())
                                {
                                    collision = true;
                                    player.Damaged(enemies[i]);
                                    enemies[i].Damaged(player);
                                    if (!enemies[i].GetIsAlive())
                                        enemies.RemoveAt(i);
                                    player.SubFood(1);
                                }
                            }
                            if (!collision)
                            {
                                player.moveLeft();
                                UpdateEnemy(enemies, player, map);

                            }
                        }
                        break;
                    case 'l':
                        if (map.checkGround(player.getX() + 1, player.getY()))
                        {
                            for (int i = 0; i < enemies.Count; i++)
                            {
                                if (enemies[i].getX() == player.getX() + 1 && enemies[i].getY() == player.getY())
                                {
                                    collision = true;
                                    player.Damaged(enemies[i]);
                                    enemies[i].Damaged(player);
                                    if (!enemies[i].GetIsAlive())
                                        enemies.RemoveAt(i);
                                    player.SubFood(1);
                                }
                            }
                            if (!collision)
                            {
                                player.moveRight();
                                UpdateEnemy(enemies, player, map);

                            }
                        }
                        break;
                    case 'k':
                        if (map.checkGround(player.getX(), player.getY() + 1))
                        {
                            for (int i = 0; i < enemies.Count; i++)
                            {
                                if (enemies[i].getX() == player.getX() && enemies[i].getY() == player.getY() + 1)
                                {
                                    collision = true;
                                    player.Damaged(enemies[i]);
                                    enemies[i].Damaged(player);
                                    if (!enemies[i].GetIsAlive())
                                        enemies.RemoveAt(i);
                                    player.SubFood(1);
                                }
                            }
                            if (!collision)
                            {
                                player.moveDown();
                                // enemies update
                                UpdateEnemy(enemies, player, map);
                            }

                        }
                        break;
                    case ' ':
                        player.SubFood(1);
                        UpdateEnemy(enemies, player, map);
                        break;
                    case 'q':
                        quit = true;
                        break;
                    default:
                        break;
                }



                if (player.getFood() == 0)
                    gameOver = true;

                // new sights added
                for (int i = player.getSightX(); i < player.getSightX() + player.getSightWidth(); i++)
                {
                    for (int j = player.getSightY(); j < player.getSightY() + player.getSightHeight(); j++)
                    {
                        if (i >= 0 && i < mapWidth && j >= 0 && j < mapHeight)
                        {
                            sight[i, j] = true;
                        }
                    }
                }
                // enemy in sight
                for (int k = 0; k < enemies.Count; k++)
                {
                    enemySeen[k] = false;
                    for (int i = 0; i < mapWidth; i++)
                    {
                        for (int j = 0; j < mapHeight; j++)
                        {
                            if (sight[i, j])
                            {
                                if (enemies[k].getX() == i && enemies[k].getY() == j)
                                    enemySeen[k] = true;
                            }

                        }
                    }
                }




                // collision with treasure
                if (map.getTile(player.getX(), player.getY()) == "T")
                {
                    item variation = (item)(Utility.Rand() % 5);
                    switch (variation)
                    {
                        case item.food:
                            foodEarned = 50 / (Utility.Rand() % (player.getLevel() + 1) + 1);
                            player.addFood(foodEarned);
                            food = true;
                            break;
                        case item.healingPotion:
                            foodEarned = Utility.Rand() % 3 + 5;
                            player.SetHp(player.getHP() + foodEarned);
                            if (player.getHP() > player.getMAXHP())
                                player.SetHp(player.getMAXHP());
                            hPotion = true;
                            break;
                        case item.elixir:
                            foodEarned = Utility.Rand() % 3 + 1;
                            player.SetMaxHp(player.getMAXHP() + foodEarned);
                            player.SetHp(player.getHP() + foodEarned);
                            elix = true;
                            break;
                        case item.weaponReinforcing:
                            foodEarned = Utility.Rand() % 2 + 1;
                            player.SetAP(player.getAP() + foodEarned);
                            wr = true;
                            break;
                        case item.armorReinforcing:
                            foodEarned = Utility.Rand() % 2 + 1;
                            player.SetDP(player.getDP() + foodEarned);
                            ar = true;
                            break;
                        default:
                            break;
                    }
                    map.setTile(player.getX(), player.getY(), Map.tile.Ground);
                    treasure = true;

                }

                bool next = false;
                // go to next level
                if (map.checkNextLevel(player.getX(), player.getY()))
                {
                    Console.Clear();
                    player.init();
                    map = new Map(screenWidth, screenHeight, player.getLevel());

                    for (int i = 0; i < enemies.Count; i++)
                    {
                        enemies.RemoveAt(i);
                        enemySeen.RemoveAt(i--);
                    }

                    for (int i = 0; i < mapWidth; i++)
                    {
                        for (int j = 0; j < mapHeight; j++)
                        {
                            sight[i, j] = false;
                        }
                    }
                    for (int i = player.getSightX(); i < player.getSightX() + player.getSightWidth(); i++)
                    {
                        for (int j = player.getSightY(); j < player.getSightY() + player.getSightHeight(); j++)
                        {
                            if (i >= 0 && i <= mapWidth && j >= 0 && j <= mapHeight)
                                sight[i, j] = true;
                        }
                    }

                    countE = Utility.Rand() % (map.countRoom + 1);
                    for (int i = 0; i < countE; i++)
                    {
                        enemies.Add(new Enemy(screenWidth, screenHeight, Utility.Rand() % player.getLevel() + 1, map, sight));
                        enemySeen.Add(false);
                    }

                    next = true;
                }


                // Rendering


                drawMap(map, sight);
                drawPlayer(player);
                if (!next)
                {
                    for (int i = 0; i < enemies.Count; i++)
                        EnemyDraw(enemySeen, enemies, i);        //enemies[i].draw();
                }
                
                Utility.LockConsole(false);
                // Clear
                Thread.Sleep(100);
            }

            if (gameOver)
            {
                Console.Clear();
                Console.SetCursorPosition(0, screenHeight - 1);
                Console.Write("Gmae Over!!!");
                Console.ReadLine();
            }


        }

        public static void drawPlayer(Player player)
        {
            Console.SetCursorPosition(player.getX(), player.getY());
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write('@');
            Console.SetCursorPosition(0, screenHeight - 1);
            if (treasure == true)
            {
                Console.SetCursorPosition(0, screenHeight - 1);

                if (food)
                    Console.Write("Earned " + foodEarned + " foods!");
                if (elix)
                    Console.Write("HP increased by " + foodEarned + "!");
                if (hPotion)
                    Console.Write("HP recovered by " + foodEarned + "!");
                if (ar)
                    Console.Write("Armor reinforced by " + foodEarned + "!");
                if (wr)
                    Console.Write("Weapon reinforced by  " + foodEarned + "!");

                food = false;
                hPotion = false;
                elix = false;
                ar = false;
                wr = false;
                treasure = false;

            } else
            {
                Console.Write("Level: " + player.getLevel() + "\tFood: " + player.getFood() + "\tHP: " + player.getHP() + " / " + player.getMAXHP() + "\tAP: " + player.getAP() + "\tDP: " + player.getDP());
            }


            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void drawMap(Map map, bool[,] sight)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    if (!sight[i, j])
                        continue;
                    Console.SetCursorPosition(i, j);
                    Console.Write(map.getTile(i, j));
                }
            }

        }

        public static int calcGroundGrids(Map map, int width, int height)
        {
            int food = 0;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (map.getTile(i, j) != "")
                        food++;
                }
            }

            return food;
        }

        public static void EnemyDraw(List<bool> b, List<Enemy> e, int i)
        {
            if (b[i])
                e[i].Draw();
        }

        public static void UpdateEnemy(List<Enemy> e, Player p, Map m)
        {
            for (int i = 0; i < e.Count; i++)
            {
                int direction = Utility.Rand() % 4;
                int x = e[i].getX();
                int y = e[i].getY();

                bool found = false;
                for (int j = e[i].getSightX(); j < e[i].getSightX() + e[i].getSightWidth(); j++)
                {
                    for (int k = e[i].getSightY(); k < e[i].getSightY() + e[i].getSightHeight(); k++)
                    {
                        if (j == e[i].getX() && k == e[i].getY())
                            continue;

                        if (j == p.getX() && k == p.getY())
                        {
                            found = true;
                        }
                            
                    }
                }

                if (found)
                {
                    int xDir = e[i].getX() - p.getX();  // negative player is right to enemy
                    int yDir = e[i].getY() - p.getY();  // negative player is bottom to enemy

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
                if (m.checkGround(x, y) && ! (x == p.getX() && y == p.getY()))
                {
                    for (int j = 0; j < e.Count; j++)
                    {
                        if (j == i)
                            continue;

                        if (x == e[j].getX() && y == e[j].getY())
                        {
                            collide = true;
                        }

                    }

                    if (!collide)
                    {
                        e[i].setX(x);
                        e[i].setY(y);
                    }
                }
                else if (x == p.getX() && y == p.getY())
                {
                    p.Damaged(e[i]);
                    if (!e[i].GetIsAlive())
                        e.RemoveAt(i);
                }

                

            }

        }

    }
}
