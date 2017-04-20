using System;
using System.Collections.Generic;
using System.Text;

namespace ticTacToe2
{
    class Game
    {
        public static void gameLoop()
        {
            StringBuilder State = new StringBuilder("_________");
            int player1 = 0, player2 = 1;
            bool done = false, p1Wins = false;
            _globals.p1 = new Stack<Tuple<StringBuilder, double[]>>();
            _globals.p2 = new Stack<Tuple<StringBuilder, double[]>>();
            _globals.moveNum0 = new Stack<int>();
            _globals.moveNum1 = new Stack<int>();

            while (!done)
            {
                //done = isFinished(State);
                var next = checkStates(State, player1);
                _globals.p1.Push(next);
                State = makeMove(next, player1);
                done = isFinished(State);
                if (done)
                {
                    p1Wins = true;
                    break;
                }
                //done = isFinished(State);
                next = checkStates(State, player2);
                _globals.p2.Push(next);
                State = makeMove(next, player2);
                done = isFinished(State);
            }
            rewards(p1Wins);

            Console.Write(State[0] + "|" + State[1] + "|" + State[2] + "\n" + State[3] + "|" + State[4] + "|" + State[5] + "\n" + State[6] + "|" + State[7] + "|" + State[8] + "\n");

            _globals.p1.Clear();
            _globals.p2.Clear();
            _globals.moveNum1.Clear();
            _globals.moveNum0.Clear();
            State.Clear();
        }

        // Works
        public static Tuple<StringBuilder, double[]> checkStates(StringBuilder status, int playerNum)
        {
            bool contains = false;
            int location = 0;
            Tuple<StringBuilder, double[]> nextMove;
            List<Tuple<StringBuilder, double[]>> allStates = new List<Tuple<StringBuilder, double[]>>();
            //Stack<Tuple<StringBuilder, double[]>> gameStack = new Stack<Tuple<StringBuilder, double[]>>();

            if (playerNum == 0)
            {
                allStates = _globals.allStates0;
                //gameStack = _globals.p1;
            }
            else
            {
                allStates = _globals.allStates1;
                //gameStack = _globals.p2;
            }

            foreach (var thing in allStates)
            {
                if (thing.Item1 == status)
                {
                    contains = true;
                    break;
                }
                location++;
            }

            if (contains)
            {
                nextMove = allStates[location];
            }
            else
            {
                double[] values = new double[9];

                for (int i = 0; i < 8; i++)
                {
                    if (status[i] == '_')
                        values[i] = 0.5;
                    else
                        values[i] = 0;
                }

                allStates.Add(Tuple.Create(status, values));

                if (playerNum == 0)
                    _globals.allStates0.Add(Tuple.Create(status, values));
                else
                    _globals.allStates1.Add(Tuple.Create(status, values));

                nextMove = allStates[location];
            }
            return nextMove;
        }

        // should be fine
        public static bool isFinished(StringBuilder status)
        {
            var counter = 0;

            string checker = status.ToString();
            foreach(char thing in checker)
            {
                if (thing != '_')
                    counter++;
            }

            if (counter == 0)
                return true;

            if ((status[0] == 'o' && status[3] == 'o' && status[6] == 'o') || (status[0] == 'x' && status[3] == 'x' && status[6] == 'x'))
                return true;
            if ((status[1] == 'o' && status[4] == 'o' && status[7] == 'o') || (status[1] == 'x' && status[4] == 'x' && status[7] == 'x'))
                return true;
            if ((status[2] == 'o' && status[5] == 'o' && status[8] == 'o') || (status[2] == 'x' && status[5] == 'x' && status[8] == 'x'))
                return true;
            if ((status[0] == 'o' && status[1] == 'o' && status[2] == 'o') || (status[0] == 'x' && status[1] == 'x' && status[2] == 'x'))
                return true;
            if ((status[3] == 'o' && status[4] == 'o' && status[5] == 'o') || (status[3] == 'x' && status[4] == 'x' && status[5] == 'x'))
                return true;
            if ((status[6] == 'o' && status[7] == 'o' && status[8] == 'o') || (status[6] == 'x' && status[7] == 'x' && status[8] == 'x'))
                return true;
            if ((status[0] == 'o' && status[4] == 'o' && status[8] == 'o') || (status[0] == 'x' && status[4] == 'x' && status[8] == 'x'))
                return true;
            if ((status[2] == 'o' && status[4] == 'o' && status[6] == 'o') || (status[2] == 'x' && status[4] == 'x' && status[6] == 'x'))
                return true;
            
            return false;
        }

        // broken?
        public static StringBuilder makeMove(Tuple<StringBuilder, double[]> currentState, int playerNum)
        {
            Random rand = new Random();

            if (playerNum == 0)
            {
                int moveNum = Math.Max(rand.Next(9), rand.Next(9));
                if (currentState.Item1[moveNum] != '_')
                {
                    while (currentState.Item1[moveNum] != '_')
                    {
                        moveNum = Math.Max(rand.Next(9), rand.Next(9));
                    }
                }

                currentState.Item1[moveNum] = 'x';
                _globals.moveNum0.Push(moveNum);
            }
            else
            {
                int moveNum = Math.Max(rand.Next(9), rand.Next(9));
                if (currentState.Item1[moveNum] != '_')
                {
                    while (currentState.Item1[moveNum] != '_')
                    {
                        moveNum = Math.Max(rand.Next(9), rand.Next(9));
                    }
                }

                currentState.Item1[moveNum] = 'o';
                _globals.moveNum1.Push(moveNum);
            }
            return currentState.Item1;
        }

        // this works
        public static void rewards(bool player1Wins)
        {
            double valueChange0 = .1, valueChange1 = .1;

            if (player1Wins)
            {
                var win = _globals.p1.Pop();
                var lose = _globals.p2.Pop();
                var num1 = _globals.moveNum0.Pop();
                var num2 = _globals.moveNum1.Pop();
                var counter = 0;

                while (win.Item1 != _globals.allStates0[counter].Item1)
                {
                    if (counter >= _globals.p1.Count)
                        break;
                    counter++;
                }
                _globals.allStates0[counter].Item2[num1] = 1;

                counter = 0;
                while (lose.Item1 != _globals.allStates1[counter].Item1)
                {
                    if (counter >= _globals.p2.Count)
                        break;
                    counter++;
                }
                _globals.allStates1[counter].Item2[num2] = 0;
            }
            else
            {
                var lose = _globals.p1.Pop();
                var win = _globals.p2.Pop();
                var num1 = _globals.moveNum0.Pop();
                var num2 = _globals.moveNum1.Pop();
                var counter = 0;

                while (win.Item1 != _globals.allStates0[counter].Item1)
                {
                    if (counter >= _globals.p1.Count)
                        break;
                    counter++;
                }
                _globals.allStates0[counter].Item2[num1] = 0;

                counter = 0;
                while (lose.Item1 != _globals.allStates1[counter].Item1)
                {
                    if (counter >= _globals.p2.Count)
                        break;
                    counter++;
                }
                _globals.allStates1[counter].Item2[num2] = 1;
            }
            while (_globals.p1.Count > 0)
            {
                var next = _globals.p1.Pop();
                var num = _globals.moveNum0.Pop();
                int counter = 0;

                while (next.Item1 != _globals.allStates0[counter].Item1)
                {
                    if (counter >= _globals.p1.Count)
                        break;
                    counter++;
                }

                if (player1Wins)
                {
                    _globals.allStates0[counter].Item2[num] += valueChange0;
                    valueChange0 *= .2;
                }
                else
                {
                    _globals.allStates0[counter].Item2[num] -= valueChange0;
                    valueChange0 *= .2;
                }
            }

            while (_globals.p2.Count > 0)
            {
                var next = _globals.p2.Pop();
                var num = _globals.moveNum1.Pop();
                int counter = 0;

                while (next.Item1 != _globals.allStates1[counter].Item1)
                {
                    if (counter >= _globals.p2.Count)
                        break;
                    counter++;
                }

                if (!player1Wins)
                {
                    _globals.allStates1[counter].Item2[num] += valueChange1;
                    valueChange1 *= .2;
                }
                else
                {
                    _globals.allStates1[counter].Item2[num] -= valueChange1;
                    valueChange1 *= .2;
                }
            }
        }

        public static void printBoard(StringBuilder State)
        {
            Console.Write(State[0] + "|" + State[1] + "|" + State[2] + "\n" + State[3] + "|" + State[4] + "|" + State[5] + "\n" + State[6] + "|" + State[7] + "|" + State[8] + "\n");
        }

        public static void humanLoop()
        {
            StringBuilder State = new StringBuilder("_________");
            int player1 = 0, player2 = 1;
            bool done = false, p1Wins = false;
            _globals.p1 = new Stack<Tuple<StringBuilder, double[]>>();
            //_globals.p2 = new Stack<Tuple<StringBuilder, double[]>>();
            _globals.moveNum0 = new Stack<int>();
            //_globals.moveNum1 = new Stack<int>();


            printBoard(State);
            String input = Console.ReadLine();
            StringBuilder newState = new StringBuilder(input);
            State = newState;
            
            while (!done)
            {
                //done = isFinished(State);
                var next = checkStates(State, player1);
                _globals.p1.Push(next);
                State = humanMove(next);
                done = isFinished(State);
                if (done)
                {
                    p1Wins = true;
                    break;
                }
                printBoard(State);
                input = Console.ReadLine();
                newState = new StringBuilder(input);
                State = newState;
                //done = isFinished(State);
                done = isFinished(State);
            }
            //rewards(p1Wins);

            //Console.Write(State[0] + "|" + State[1] + "|" + State[2] + "\n" + State[3] + "|" + State[4] + "|" + State[5] + "\n" + State[6] + "|" + State[7] + "|" + State[8] + "\n");

            _globals.p1.Clear();
            _globals.p2.Clear();
            _globals.moveNum1.Clear();
            _globals.moveNum0.Clear();
            State.Clear();
        }

        public static StringBuilder humanMove(Tuple<StringBuilder,double[]> currentState)
        {
            //int moveNum = Math.Max(rand.Next(9), rand.Next(9));

            double max = 0;
            int counter = 0, bestIndex = 0;

            foreach(var thing in currentState.Item2)
            {
                if(thing > max)
                {
                    max = thing;
                    bestIndex = counter;
                }
                counter++;
            }

            currentState.Item1[bestIndex] = 'x';
            _globals.moveNum0.Push(bestIndex);
            return currentState.Item1;
        }

    }


    class _globals
    {
        public static List<Tuple<StringBuilder, double[]>> allStates0;
        public static List<Tuple<StringBuilder, double[]>> allStates1;
        public static Stack<Tuple<StringBuilder, double[]>> p1;
        public static Stack<Tuple<StringBuilder, double[]>> p2;
        public static Stack<int> moveNum0;
        public static Stack<int> moveNum1;
    }

    class MainClass
    {
        public static void Main(String[] args)
        {
            _globals.allStates0 = new List<Tuple<StringBuilder, double[]>>();
            _globals.allStates1 = new List<Tuple<StringBuilder, double[]>>();

            Console.WriteLine("Welcome to tic tac toe. To play with a human, type human. To train, type train.");
            //var read = Console.ReadLine();

            int gameCounter = 0;
            while (gameCounter < 5)
            {
                Game.gameLoop();
                Console.WriteLine("Finished game " + gameCounter);
                gameCounter++;
            }

            Game.humanLoop();



            /*
            Console.WriteLine("Player 1");
            foreach (var thing in _globals.allStates0)
            {
                Console.WriteLine(thing.Item1);
                foreach (var blep in thing.Item2)
                {
                    Console.Write(blep + ",");
                }
                Console.WriteLine();
            }

            Console.WriteLine("player 2");
            foreach (var thing in _globals.allStates1)
            {
                Console.WriteLine(thing.Item1);
                foreach (var blep in thing.Item2)
                {
                    Console.Write(blep + ",");
                }
                Console.WriteLine();
            }

*/         

        }
    }
}
