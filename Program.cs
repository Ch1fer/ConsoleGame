using System;
using System.IO;
using System.Threading;

namespace MyGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Player pacman = new Player("pacman", ConsoleColor.Yellow, '@');

            World testWorld = new World("map.txt", pacman, 1, 1);
            Enemy stupidPrince = new GhostRandom("prince", 'O', ConsoleColor.Red,  60, 1);
            Enemy blindDog = new GhostSmart("blind dog", 'G', ConsoleColor.Magenta, 40, 15, 3);
            testWorld.AddEnemy(stupidPrince);
            testWorld.AddEnemy(blindDog);

            World village_42 = new World("map1.txt", pacman, 45, 20);
            Enemy randomClown = new GhostRandom("it", 'O', ConsoleColor.Red, 1, 1);
            Enemy randomVillager0 = new GhostRandom("villager_0", '*', ConsoleColor.Gray, 11, 8);
            Enemy randomVillager1 = new GhostRandom("villager_1", '*', ConsoleColor.Gray, 37, 8);
            Enemy randomVillager2 = new GhostRandom("villager_2", '*', ConsoleColor.Gray, 61, 8);
            village_42.AddEnemy(randomVillager0);
            village_42.AddEnemy(randomVillager1);
            village_42.AddEnemy(randomVillager2);

            World graveyard = new World("map2.txt", pacman, 61, 24);
            Enemy miner = new GhostSmart("miner", 'G', ConsoleColor.Magenta, 45, 8, 3);
            graveyard.AddEnemy(miner);

            World villa = new World("map3.txt", pacman, 43, 24);
            Enemy citizen_1 = new GhostRandom("paul", 'x', ConsoleColor.Red, 11, 15);
            Enemy citizen_2 = new GhostRandom("villiam", 'x', ConsoleColor.Red, 61, 15);
            Enemy citizen_3 = new GhostRandom("moris", 'x', ConsoleColor.Red, 50, 7);
            villa.AddEnemy(citizen_1);
            villa.AddEnemy(citizen_2);
            villa.AddEnemy(citizen_3);

            World scholl = new World("map4.txt", pacman, 43, 24);
            Enemy schoolboy_1 = new GhostChanger("Tom", 'i', ConsoleColor.Red, 12, 21);
            Enemy schoolboy_2 = new GhostChanger("Max", 'i', ConsoleColor.DarkBlue, 61, 15);
            Enemy schoolboy_3 = new GhostChanger("Jimmy", 'i', ConsoleColor.Green, 33, 8);
            Enemy schoolboy_4 = new GhostChanger("Billy", 'i', ConsoleColor.Cyan, 43, 17);
            Enemy schoolboy_5 = new GhostChanger("Oliver", 'i', ConsoleColor.DarkGreen, 25, 4);
            scholl.AddEnemy(schoolboy_1);
            scholl.AddEnemy(schoolboy_2);
            scholl.AddEnemy(schoolboy_3);
            scholl.AddEnemy(schoolboy_4);
            scholl.AddEnemy(schoolboy_5);

            World prison = new World("map5.txt", pacman, 43, 24);
            Enemy crazyDog = new GhostSmart("blind dog", 'B', ConsoleColor.DarkRed, 40, 15, 1);
            Enemy prisoner_1 = new GhostRandom("jojo", 'o', ConsoleColor.Red, 11, 15);
            Enemy prisoner_2 = new GhostRandom("martin", 'o', ConsoleColor.Red, 61, 15);
            prison.AddEnemy(prisoner_1);
            prison.AddEnemy(prisoner_2);
            prison.AddEnemy(crazyDog);

            World house = new World("map6.txt", pacman, 43, 24);
            Enemy dog = new GhostRandom("jack", 'b', ConsoleColor.DarkRed, 60, 6);
            Enemy policeman = new GhostChanger("Runger", 'T', ConsoleColor.DarkCyan, 15, 6);
            Enemy doctor_1 = new GhostRandom("Tenma", '+', ConsoleColor.Red, 15, 10);
            Enemy doctor_2 = new GhostRandom("Bob", '+', ConsoleColor.Red, 60, 16);
            house.AddEnemy(dog);
            house.AddEnemy(policeman);
            house.AddEnemy(doctor_1);
            house.AddEnemy(doctor_2);

            World mekka = new World("map7.txt", pacman, 6, 12);
            Enemy monk = new GhostSmart("Magomet", 'V', ConsoleColor.DarkMagenta, 75, 12, 3);
            Enemy monk_1 = new GhostRandom("Abdul", 'G', ConsoleColor.Green, 38, 5);
            Enemy monk_2 = new GhostRandom("Mahmut", 'G', ConsoleColor.Green, 38, 8);
            mekka.AddEnemy(monk);
            mekka.AddEnemy(monk_1);
            mekka.AddEnemy(monk_2);

            Game pacmanGame = new Game(pacman, 15, 0);
            pacmanGame.AddWorld(testWorld);
            pacmanGame.AddWorld(village_42);
            pacmanGame.AddWorld(graveyard);
            pacmanGame.AddWorld(villa);
            pacmanGame.AddWorld(scholl);
            pacmanGame.AddWorld(prison);
            pacmanGame.AddWorld(house);
            pacmanGame.AddWorld(mekka);

            pacmanGame.Run();
        }
    }
}