using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsolSnakeRacer
{
    // biezacy kierunek w ktorym porusza sie snake
    public enum Direction
    {
        Left
      , Right
      , Up
      , Down
    }

    public class Snake : ISnake
    {
        private readonly string SnakeHead = "@";                                //symbol glowy sneka
        private readonly string SnakeBody = "#";                                //symbol ciala sneka
        private readonly ConsoleColor SnakeBodyColor = ConsoleColor.Green;      //kolor ciala sneka
        private ConsoleColor SnakeHeadColor = ConsoleColor.DarkGreen;           //kolor glowy sneka

        public int Length { get; private set; } = 3;                                // poczatkowa dlugosc
        public Direction Direction { get; set; } = Direction.Left;          // kierunek glowy
        public Coordinate HeadPosition { get; set; } = new Coordinate(9, 9); // pozycja glowy
        List<Coordinate> Tail { get; set; } = new List<Coordinate>();

        private bool hisAlive = true;
        private bool outOfRange = false;

        public Snake()
        {
            HeadPosition = new Coordinate(9, 9);     // poczatkowy punkt startowy glowy
            Direction = Direction.Right;            // poczatkowy kierunek glowy
            //RiseEventBeforMove();
            //RiseEventAfterNewCoordinate();
        }

        public bool GameOver
        {
            // czy w naszym ogonie ktoras z czesci ogona nie pokrywa sie z koordynatami glowy
            // czli czy waz nie wpadl na swoj ogon

            get
            {
                bool colisionWithTail = false;

                if (
                    Tail.Where(                             // sprawdzamy wykorzystujac System.Linq.Where
                                c =>                        // gdzie kordynaty (c =>)
                                  c.x == HeadPosition.x     // pokrywaj sie w pozyji X z glowa
                               && c.y == HeadPosition.y     // oraz pokrywaj sie w pozyji Y z glowa
                              ).ToList()                    // sprawdzamy dla wszystkich elementow Listy koordynatow ogona
                                        .Count > 1          // sprawdzamy czy kolizja nastapila w na conajmniej jednym odcinku ogona
                   )
                    colisionWithTail = true;
                // Mozliwosci skonczenia gry:
                return (colisionWithTail                 // - kolizaja z ogonem
                        || outOfRange                       // - wyjechanie poza ekran
                        || hisAlive == false                 // - kolizja ze sciana (i nie zyje)
                              );
            }
        }

        public void EatMeal()
        {
            Length++;
            Console.Beep(); // dzwiek zjedzenia
            RiseEventAfterNewLength();
            RiseEventAfterEatMeal();
        }

        public void Move()
        {
            int NewX = HeadPosition.x;
            int NewY = HeadPosition.y;

            //Okreslenie nowej pozycji glowy
            switch (Direction)
            {
                case Direction.Left:
                    NewX--;
                    //HeadPosition.x--;
                    break;
                case Direction.Right:
                    NewX++;
                    //HeadPosition.x++;
                    break;
                case Direction.Up:
                    NewY--;
                    //HeadPosition.y--;
                    break;
                case Direction.Down:
                    NewY++;
                    //HeadPosition.y++;
                    break;
            }



            HeadPosition.x = NewX;
            HeadPosition.y = NewY;
            RiseEventAfterNewCoordinate();

            RiseEventAfterMove();  // Zaraz po ruchu (przed rysowaniem) przekazujemy infor o nowych koordynatach
            //TO DO - delegata AfterMove: czyli ruch nastapil
            //TO DO - tutaj delegata ktora pozwoli okreslic czy nie ma kolizji ze sciana
            //      - delegata powinna umozliwic zwrocic informacje: o kolizji

            Draw(); //Rysowanie glowy

            RiseEventBeforMove(); // zaraz po narysowaniu
            //przypisanie nowej pozycji glowy
            //TO DO - delegata BeforMove: czyli ruch nastapi i wiem z jakiego miejsca na jakie
            //      - delegata powinna umozliwic zwrocic informacje o innej nowej pozycji; np.:
            //                                            -1- w Pac-Man przy przejsciu z lewej strony na prawa (przez tunel)

        }

        public void HeadColision()
        {
            SnakeHeadColor = ConsoleColor.Red;
            hisAlive = false;
        }

        //Rysowanie calego snake 
        void Draw()
        {
            //TO DO - zamiast try/catch zrobic sprawdzenie bufora ekranu: czye head position nie wykracza poza ekran
            try
            {

                //rysowanie nowej pozycji glowy
                Console.ForegroundColor = SnakeHeadColor;
                Console.SetCursorPosition(HeadPosition.x, HeadPosition.y);
                Console.Write(SnakeHead);

                //rysowanie ogona na poprzedniej pozycji glowy
                if (Tail.Count > 1)
                {
                    Console.ForegroundColor = SnakeBodyColor;
                    Console.SetCursorPosition(Tail[Tail.Count - 1].x, Tail[Tail.Count - 1].y);
                    Console.Write(SnakeBody);
                }

                //zbieramy koordynaty po ktorych przeslismy
                Tail.Add(new Coordinate(HeadPosition.x, HeadPosition.y));

                //czyszczenie ogona
                if (Tail.Count > this.Length)
                {
                    var endTail = Tail.First();                         //wybieram pierwszy element listy => koniec ogona
                    Console.SetCursorPosition(endTail.x, endTail.y);    //1 - czyscimy na ekranie
                    Console.Write(" ");
                    Tail.Remove(endTail);                               //2 - czyscimy informacje (z listy)
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                outOfRange = true;
            }
        }




        // --------------------------------------------------------- --- DELEGATY --- --------------------------------------------------------- ---
        /* --- 1 --- */
        public delegate void EventBeforMove();
        private EventBeforMove listOfHandlersBeforMove;

        public void RegisterWithEventBeforMove(EventBeforMove methodToCall) => listOfHandlersBeforMove += methodToCall;
        private void RiseEventBeforMove() => listOfHandlersBeforMove?.Invoke();

        /* --- 2 --- */
        public delegate void EventAfterNewCoordinate(Coordinate HeadPosition);
        private EventAfterNewCoordinate listOfHandlersAfterNewCoordinate;

        public void RegisterWithEventAfterNewCoordinate(EventAfterNewCoordinate methodToCall) => listOfHandlersAfterNewCoordinate += methodToCall;
        private void RiseEventAfterNewCoordinate() => listOfHandlersAfterNewCoordinate?.Invoke(this.HeadPosition);

        /* --- 3 --- */
        public delegate void EventAfterMove();
        private EventAfterMove listOfHandlersAfterMove;

        public void RegisterWithEventAfterMove(EventAfterMove methodToCall) => listOfHandlersAfterMove += methodToCall;
        private void RiseEventAfterMove()
        {
            listOfHandlersAfterMove?.Invoke();
        }

        /* --- 4 --- */
        public delegate void EventAfterEatMeal();
        private EventAfterEatMeal listOfHandlersAfterEatMeal;

        public void RegisterWithEventAfterEatMeal(EventAfterEatMeal methodToCall) => listOfHandlersAfterEatMeal += methodToCall;
        private void RiseEventAfterEatMeal() => listOfHandlersAfterEatMeal?.Invoke();

        /* --- 5 --- */
        public delegate void EventAfterNewLength(int Length);
        private EventAfterNewLength listOfHandlersAfterNewLength;

        public void RegisterWithEventAfterNewLength(EventAfterNewLength methodToCall) => listOfHandlersAfterNewLength += methodToCall;
        private void RiseEventAfterNewLength() => listOfHandlersAfterNewLength?.Invoke(this.Length);
        // --------------------------------------------------------- ----------------- --- ---------------------------------------------------------

    }
}
