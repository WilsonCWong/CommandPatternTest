using System;

namespace CommandPatternTest.Commands
{
    public abstract class Command
    {
        abstract public string Name { get; }
        
        public virtual object ReturnValue()
        {
            return null;
        }
        
        public virtual void Execute()
        {
            Console.WriteLine($"Executing {Name}");
        }

        public virtual void Undo()
        {
            Console.WriteLine($"Undoing {Name}");
        }
}
}