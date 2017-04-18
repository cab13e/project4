using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ticTacToe
{
	class Game
	{

        public static void checkStates(StringBuilder status, Stack<Tuple<StringBuilder,double[]>> gameStates, int playerNum)
        {
            bool contains = false;
            int location = 0;
            Tuple<StringBuilder, double[]> nextMove;
            List<Tuple<StringBuilder, double[]>> allStates;

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

            if(contains)
            {
                nextMove = allStates[location];
            }
            else
            {
                double[] values = new double[8];

                for (int i = 0; i < 8; i++)
                {
                    if (status[i] == '_')
                        values[i] = 0.5;
                    else
                        values[i] = 0;
                }
                allStates.Add(Tuple.Create(status, values));
                nextMove = allStates[location];
            }
            gameStates.Push(nextMove);
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

        public static StringBuilder makeMove(Tuple<StringBuilder,double[]> currentState, int playerNum)
        {
            Random rand = new Random();

            if (playerNum == 0)
            {
                // treat as player 'x'
                int count = 0;
                foreach (var thing in currentState.Item2)
                    count++;
                int moveNum = Math.Max(rand.Next(count + 1), rand.Next(count+1));

                currentState.Item1[moveNum] = 'x';
            }
            else
            {
                // treat as player 'o'
                int count = 0;
                foreach (var thing in currentState.Item2)
                    count++;
                int moveNum = Math.Max(rand.Next(count + 1), rand.Next(count + 1));

                currentState.Item1[moveNum] = 'o';
            }
            return currentState.Item1;
        }

        public static void rewards(Stack<Tuple<StringBuilder,double[]>> player1, Stack<Tuple<StringBuilder,double[]>> player2)
        {
            
        }

        public static void gameLoop()
        {
            StringBuilder State = new StringBuilder("_________");
            int player1 = 0, player2 = 1;
            bool done = false;
            Stack<Tuple<StringBuilder, double[]>> p1 = new Stack<Tuple<StringBuilder, double[]>>();
            Stack<Tuple<StringBuilder, double[]>> p2 = new Stack<Tuple<StringBuilder, double[]>>();
            Tuple<StringBuilder, double[]> p1States;
            Tuple<StringBuilder, double[]> p2States;
            while(!done)
            {
                done = isFinished(State);
                checkStates(State,p1,player1);
                p1States = p1.Peek();
                makeMove(p1States,player1);
                done = isFinished(State);
                checkStates(State,p2,player2);
                p2States = p2.Peek();
                makeMove(p2States,player2);
            }
            rewards(p1,p2);
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
			
		}
	}
}
