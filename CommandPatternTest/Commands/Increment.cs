using CommandPatternTest.ReferencePrimitives;

namespace CommandPatternTest.Commands
{
    public class Increment : Command
    {
        private int prevValue;
        private int executedValue;
        private IntVar value;

        public override string Name => "Increment";

        public Increment(IntVar value)
        {
            prevValue = executedValue = value.Value;
            this.value = value;
        }

        public override object ReturnValue()
        {
            return executedValue;
        }

        public override void Execute()
        {
            prevValue = value.Value;
            executedValue = value.Value += 1;
        }

        public override void Undo()
        {
            value.Value = prevValue;
        }
    }
}