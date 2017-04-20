using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ticTacToe
{
    class Game
    {

        public static Tuple<StringBuilder, double[]> checkStates(StringBuilder status, Stack<Tuple<StringBuilder, double[]>> gameStates, int playerNum)
        {
            bool contains = false;
            int location = 0;
            Tuple<StringBuilder, double[]> nextMove;
            List<Tuple<StringBuilder, double[]>> allStates = new List<Tuple<StringBuilder, double[]>>();

            if (playerNum == 0)
            {
                allStates = _globals.allStates0;
            }
            else
            {
                allStates = _globals.allStates1;
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

        public static bool isFinished(StringBuilder status)
        {
            if ((status[0] == 'o' && status[3] == 'o' && status[6] == 'o') || (status[0] == 'x' && status[3] == 'x' && status[6] == 'x'))
                return true;
            else if ((status[1] == 'o' && status[4] == 'o' && status[7] == 'o') || (status[1] == 'x' && status[4] == 'x' && status[7] == 'x'))
                return true;
            else if ((status[2] == 'o' && status[5] == 'o' && status[8] == 'o') || (status[2] == 'x' && status[5] == 'x' && status[8] == 'x'))
                return true;
            else if ((status[0] == 'o' && status[1] == 'o' && status[2] == 'o') || (status[0] == 'x' && status[1] == 'x' && status[2] == 'x'))
                return true;
            else if ((status[3] == 'o' && status[4] == 'o' && status[5] == 'o') || (status[3] == 'x' && status[4] == 'x' && status[5] == 'x'))
                return true;
            else if ((status[6] == 'o' && status[7] == 'o' && status[8] == 'o') || (status[6] == 'x' && status[7] == 'x' && status[8] == 'x'))
                return true;
            else if ((status[0] == 'o' && status[4] == 'o' && status[8] == 'o') || (status[0] == 'x' && status[4] == 'x' && status[8] == 'x'))
                return true;
            else if ((status[2] == 'o' && status[4] == 'o' && status[6] == 'o') || (status[2] == 'x' && status[4] == 'x' && status[6] == 'x'))
                return true;
            else
                return false;
        }

        public static StringBuilder makeMove(Tuple<StringBuilder, double[]> currentState, int playerNum, Stack<int> moves)
        {
            Random rand = new Random();

            if (playerNum == 0)
            {
                // treat as player 'x'
                int moveNum = Math.Max(rand.Next(9), rand.Next(9));

                currentState.Item1[moveNum] = 'x';
                moves.Push(moveNum);
            }
            else
            {
                // treat as player 'o'
                int moveNum = Math.Max(rand.Next(9), rand.Next(9));

                currentState.Item1[moveNum] = 'o';
                moves.Push(moveNum);
            }
            return currentState.Item1;
        }

        public static void rewards(Stack<Tuple<StringBuilder, double[]>> player1, Stack<Tuple<StringBuilder, double[]>> player2, Stack<int> moveNum0, Stack<int> moveNum1, bool whoWon)
        {
            double valueChange0 = .1;
            double valueChange1 = .1;

            while (player1.Count > 0)
            {
                var next = player1.Pop();
                var num = moveNum0.Pop();
                int counter = 0;

                while(next.Item1 != _globals.allStates0[counter].Item1)
                {
                    if (counter >= player1.Count)
                        break;
                    counter++;
                }

                if (whoWon)
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

            while (player2.Count > 0)
            {
                var next = player2.Pop();
                var num = moveNum1.Pop();
                int counter = 0;

                while (next.Item1 != _globals.allStates1[counter].Item1)
                {
                    if (counter >= player2.Count)
                        break;
                    counter++;
                }

                if (!whoWon)
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

        public static void gameLoop()
        {
            StringBuilder State = new StringBuilder("_________");
            int player1 = 0, player2 = 1;
            bool done = false;
            bool p1Wins = false;
            Stack<Tuple<StringBuilder, double[]>> p1 = new Stack<Tuple<StringBuilder, double[]>>();
            Stack<Tuple<StringBuilder, double[]>> p2 = new Stack<Tuple<StringBuilder, double[]>>();
            Tuple<StringBuilder, double[]> p1States;
            Tuple<StringBuilder, double[]> p2States;
            Stack<int> moveNum0 = new Stack<int>();
            Stack<int> moveNum1 = new Stack<int>();
            while (!done)
            {
                done = isFinished(State);
                var next = checkStates(State, p1, player1);
                p1.Push(next);
                p1States = p1.Peek();
                State = makeMove(p1States, player1, moveNum0);
                done = isFinished(State);
                if (done)
                {
                    p1Wins = true;
                    break;
                }
                next = checkStates(State, p2, player2);
                p2.Push(next);
                p2States = p2.Peek();
                State = makeMove(p2States, player2, moveNum1);
            }
            //Console.WriteLine(State);
            rewards(p1, p2, moveNum0, moveNum1, p1Wins);
            //Console.WriteLine("Finished game");
        }


    }

    class _globals
    {
        public static List<Tuple<StringBuilder, double[]>> allStates0;
        public static List<Tuple<StringBuilder, double[]>> allStates1;
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
            while (gameCounter < 100)
            {
                Game.gameLoop();
                Console.WriteLine("Finished game " + gameCounter);
                gameCounter++;
            }

            Console.WriteLine("Player 1");
            foreach(var thing in _globals.allStates0)
            {
                Console.WriteLine(thing.Item1);
                foreach(var blep in thing.Item2)
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
        }
    }
}
