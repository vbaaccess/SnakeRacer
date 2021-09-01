using System;

namespace ConsolSnakeRacer
{
    class Program
    {
        static void Main(string[] args)
        {
            int gameBoardSize = 54;                 // ustawienia rozmiaru obszaru gry

            //ustawienia okna konsoli:
            Console.CursorVisible = false;          // wylaczenie migajacego kursora
            GameConsolWindow.Size(gameBoardSize     // - ustawienia rozmiaru
                            , gameBoardSize);
            GameConsolWindow.Center();              // - wysrodkowanie okna

            // wstepne menu gry (nazie bez opcji)
            string s;
            Console.Clear();
            Console.Title = "CONSOL GAME - SNAKE RACER 2021 (c)";
            s = "CONSOL GAME SNAKE RACER";
            Console.SetCursorPosition((int)gameBoardSize / 2 - ((int)(s.Length) / 2), 1);
            Console.Write(s);

            s = "PRES KEY TO START GAME!";
            Console.SetCursorPosition((int)gameBoardSize / 2 - ((int)(s.Length) / 2), gameBoardSize / 2);
            Console.Write(s);
            Console.ReadLine();
            Console.Clear();
        }
    }
}
