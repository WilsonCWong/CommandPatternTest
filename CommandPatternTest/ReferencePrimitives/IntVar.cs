using System.Dynamic;

namespace CommandPatternTest.ReferencePrimitives
{
    public class IntVar
    {
        public int Value { get; set; }

        public IntVar(int value)
        {
            Value = value;
        }
    }
}