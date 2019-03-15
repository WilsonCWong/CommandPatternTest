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
        
        private List<Command> commandQueue;
        private int currentCommand = -1; // 0 is reserved for the first command in the command queue.

        public bool CanContinue { get; private set; }

        public Game() {
            CanContinue = true;
            counter = new IntVar(0);
            commandQueue = new List<Command>();
        }


        private void ReadInput()
        {
            cki = null;
            if (Console.KeyAvailable)
                cki = Console.ReadKey(true);
        }

        private void LogQueue()
        {
            if (currentCommand == -1)
                Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($" [0 (Initial State)] ");
            Console.ResetColor();
                
            for (int i = 0; i < commandQueue.Count; i++)
            {
                if (currentCommand == i)
                    Console.ForegroundColor = ConsoleColor.Green;
                string suffix = (i == commandQueue.Count - 1) ? " (Latest State)" : "";
                Console.Write($" [{commandQueue[i].ReturnValue()}{suffix}] ");
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
                        // New command overwrites the one(s) after the current index if
                        // the queue is not at the latest command (due to undo)
                        if (currentCommand != commandQueue.Count - 1 && commandQueue.Count > 0)
                            commandQueue.RemoveRange(currentCommand + 1, commandQueue.Count - (currentCommand + 1));
                        commandQueue.Add(new Increment(counter));
                        currentCommand = commandQueue.Count - 1;
                        commandQueue[currentCommand].Execute();
                        executedFunction = "New";
                        break;
                    case ConsoleKey.Q: // Undo
                        if (currentCommand >= 0)
                        {
                            commandQueue[currentCommand].Undo();
                            currentCommand = (currentCommand == 0) ? -1 : currentCommand - 1;
                            executedFunction = "Undo";
                        }
                        break;
                    case ConsoleKey.R: // Redo
                        if (currentCommand != commandQueue.Count - 1 && commandQueue.Count >= 1)
                        {
                            currentCommand++;
                            commandQueue[currentCommand].Execute();
                            executedFunction = "Redo";
                        }
                        break;
                    default:
                        break;
                }

                if (executedFunction != "" && commandQueue.Count > 0)
                {
                    Console.Write($"{executedFunction}: ");
                    LogQueue();
                }
            }
        }
    }

}