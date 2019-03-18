using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Runtime.CompilerServices;
using CommandPatternTest.Commands;
using CommandPatternTest.ReferencePrimitives;

namespace CommandPatternTest
{
    using System;
					
    public class Program
    {
        public static void Main()
        {
            Game g = new Game();

            g.Awake();
            
            while (g.CanContinue) {
                g.Update();
            }
        }
    }

    public class Game {
        private ConsoleKeyInfo? cki;
        private IntVar counter;

        private CommandQueue commandQueue;

        public bool CanContinue { get; private set; }

        public Game() {
            CanContinue = true;
            counter = new IntVar(0);
            commandQueue = new CommandQueue();
        }

        private void ReadInput()
        {
            cki = null;
            if (Console.KeyAvailable)
                cki = Console.ReadKey(true);
        }

        private void LogQueue()
        {
            if (commandQueue.Index == -1)
                Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($" [0 (Initial State)] ");
            Console.ResetColor();
                
            for (int i = 0; i < commandQueue.Queue.Count; i++)
            {
                if (commandQueue.Index == i)
                    Console.ForegroundColor = ConsoleColor.Green;
                string suffix = (i == commandQueue.Queue.Count - 1) ? " (Latest State)" : "";
                Console.Write($" [{commandQueue.Queue[i].ReturnValue()}{suffix}] ");
                Console.ResetColor();
            }
            
            Console.WriteLine();
        }

        public void Awake()
        {
            Console.WriteLine("Welcome to a test of the command pattern.");
            Console.WriteLine("Controls: E - Increment value, U - Undo increment, R - Redo Increment");
        }
	
        public void Update()
        {
            ReadInput();
            if (cki != null)
            {
                string executedFunction = "";
                switch (cki.Value.Key)
                {
                    case ConsoleKey.E: // New command
                        commandQueue.Enqueue(new Increment(counter));
                        executedFunction = "New";
                        break;
                    case ConsoleKey.Q: // Undo
                        if (commandQueue.Undo())
                            executedFunction = "Undo";
                        break;
                    case ConsoleKey.R: // Redo
                        if (commandQueue.Redo())
                            executedFunction = "Redo";
                        break;
                    default:
                        break;
                }

                if (executedFunction != "" && !commandQueue.IsQueueEmpty())
                {
                    Console.Write($"{executedFunction}: ");
                    LogQueue();
                }
            }
        }
    }

}