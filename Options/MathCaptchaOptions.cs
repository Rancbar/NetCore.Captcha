using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Captcha
{
    public class MathCaptchaOptions : CaptchaOptions
    {
        public int FirstMax { get; private set; } = 99;
        public int SecondMax { get; private set; } = 99;
        public int TotalMax { get; private set; } = 100;
        public bool AllowNegativeResult { get; private set; } = false;
        public bool Sum { get; private set; } = true;
        public bool Subtraction { get; private set; } = true;
        public bool Multiplication { get; private set; } = false;
        public bool Division { get; private set; } = false;

        public List<OperationType> Operations = new List<OperationType>(new[] { OperationType.Sum, OperationType.Subtraction });

        public MathCaptchaOptions() { }

        public MathCaptchaOptions SetMathOptions(
            int FirstMax = 99,
            int SecondMax = 99,
            int TotalMax = 100,
            bool AllowNegativeResult = false,
            params OperationType[] operations
        )
        {
            if (FirstMax < 1 || SecondMax < 1 || TotalMax < 1)
            {
                throw new ArgumentException("The provided maximum arguments cannot be zero or negative");
            }
            if (operations.Length == 0)
            {
                throw new ArgumentException("At least one mathematical operation is required");
            }
            if (Array.IndexOf(operations, OperationType.Sum) >= 0 || Array.IndexOf(operations, OperationType.Multiplication) >= 0)
            {
                if (TotalMax <= FirstMax || TotalMax <= SecondMax)
                {
                    throw new ArgumentException("The total maximum should be greater than operands maximunm values");
                }
            }


            this.FirstMax = FirstMax;
            this.SecondMax = SecondMax;
            this.TotalMax = TotalMax;
            this.AllowNegativeResult = AllowNegativeResult;
            this.Sum = Array.IndexOf(operations, OperationType.Sum) >= 0;
            this.Subtraction = Array.IndexOf(operations, OperationType.Subtraction) >= 0;
            this.Multiplication = Array.IndexOf(operations, OperationType.Multiplication) >= 0;
            this.Division = Array.IndexOf(operations, OperationType.Division) >= 0;
            this.Operations = new List<OperationType>(operations);
            return this;
        }

        public class OperationType
        {
            public char Value { get; private set; }
            private OperationType(char Value)
            {
                this.Value = Value;
            }
            public static OperationType Sum { get; private set; } = new OperationType('+');
            public static OperationType Subtraction { get; private set; } = new OperationType('-');
            public static OperationType Multiplication { get; private set; } = new OperationType('x');
            public static OperationType Division { get; private set; } = new OperationType('÷');

        };
    }
}
