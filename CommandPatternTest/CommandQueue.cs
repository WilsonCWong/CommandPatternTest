using System.Collections.Generic;
using CommandPatternTest.Commands;

namespace CommandPatternTest
{
    /// <summary>
    /// Represents a command queue and supports the adding, undoing, and redoing operations
    /// on commands of the queue.
    /// </summary>
    public class CommandQueue
    {
        /// <summary>
        /// The current command index of the queue.
        /// </summary>
        public int Index { get; private set; }
        
        /// <summary>
        /// The command queue.
        /// </summary>
        public List<Command> Queue { get; private set; }
        
        /// <summary>
        /// Create a command queue.
        /// </summary>
        public CommandQueue()
        {
            Queue = new List<Command>();
            Index = -1;
        }

        /// <summary>
        /// Check whether or not the current command in focus is the latest command enqueued.
        /// </summary>
        /// <returns>True if the current command is the one that is added most recently, false if not.</returns>
        private bool IsLatestCommand()
        {
            return (Index == Queue.Count - 1);
        }
        
        /// <summary>
        /// Check whether or not the command queue is empty.
        /// </summary>
        /// <returns>True if the queue is empty, false if it is not.</returns>
        public bool IsQueueEmpty()
        {
            return (Queue.Count == 0);
        }

        /// <summary>
        /// Gets the current command in the queue;
        /// </summary>
        /// <returns>The current command in focus in the queue.</returns>
        public Command CurrentCommand()
        {
            return (IsQueueEmpty()) ? null : Queue[Index];
        }
        
        /// <summary>
        /// Enqueues a command and executes it.
        /// </summary>
        /// <param name="c">The command.</param>
        public void Enqueue(Command c)
        {
            // This will overwrite all commands after the index of new one that is inserted,
            // which can occur if the current command isn't at the last element of the queue
            // due to undo functionality.
            if (!IsLatestCommand() && !IsQueueEmpty())
                Queue.RemoveRange(Index + 1, Queue.Count - (Index + 1));
            Queue.Add(c);
            Index = Queue.Count - 1;
            Queue[Index].Execute();
        }
        
        /// <summary>
        /// Undo the current command.
        /// </summary>
        /// <returns>Whether the operation was successful or not.</returns>
        public bool Undo()
        {
            if (Index < 0) return false;
            Queue[Index].Undo();
            Index = (Index == 0) ? -1 : Index - 1;
            return true;

        }
        
        /// <summary>
        /// Redo the next command in the queue.
        /// </summary>
        /// <returns>Whether the operation was successful or not.</returns>
        public bool Redo()
        {
            if (IsLatestCommand() || Queue.Count < 1) return false;
            Index++;
            Queue[Index].Execute();
            return true;

        }      
    }
}