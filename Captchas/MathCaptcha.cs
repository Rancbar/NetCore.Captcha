using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Captcha
{
    public class MathCaptcha : Captcha
    {
        private int FirstOperand;
        private int SecondOperand;

        private static Random rand = new Random((int)DateTime.Now.Ticks);

        public new static MathCaptchaOptions Options
        {
            get { return OptionContainer.GetOptions<MathCaptchaOptions>(); }
            set { OptionContainer.SetOptions(value); }
        }

        public MathCaptcha() : this(Options)
        {
        }
        public MathCaptcha(MathCaptchaOptions options) : base(options)
        {
            //generate new question
            FirstOperand = rand.Next(1, options.FirstMax);

            int operationIndex = rand.Next(0, options.Operations.Count);
            MathCaptchaOptions.OperationType operation = options.Operations[operationIndex];
            //delimiter = operation.Value;

            int max;
            if (operation == MathCaptchaOptions.OperationType.Sum)
            {
                var _tmpValue = options.TotalMax - FirstOperand;
                max = Math.Min(options.SecondMax, _tmpValue < 0 ? 1 : _tmpValue);
                SecondOperand = rand.Next(1, max);
                CaptchaAnswer = (FirstOperand + SecondOperand).ToString();
            }
            else if (operation == MathCaptchaOptions.OperationType.Subtraction)
            {
                max = options.AllowNegativeResult ? options.SecondMax : Math.Min(options.SecondMax, FirstOperand);
                SecondOperand = rand.Next(1, max);
                CaptchaAnswer = (FirstOperand - SecondOperand).ToString();
            }
            else if (operation == MathCaptchaOptions.OperationType.Multiplication)
            {
                max = Math.Min(options.SecondMax, options.TotalMax / FirstOperand);
                SecondOperand = rand.Next(1, max);
                CaptchaAnswer = (FirstOperand * SecondOperand).ToString();
            }
            else if (operation == MathCaptchaOptions.OperationType.Division)
            {
                max = Math.Min(options.SecondMax, FirstOperand);
                SecondOperand = rand.Next(1, max);
                SecondOperand = findDivisible(FirstOperand, SecondOperand);
                CaptchaAnswer = (FirstOperand / SecondOperand).ToString();
            }
            else
            {
                throw new InvalidOperationException($"The operation {operation.Value} is not a valid operation");
            }

            CaptchaQuestion = string.Format($"{FirstOperand} {operation.Value} {SecondOperand} = ?");
        }

        public static int findDivisible(int FirstOperand, int SecondOperand = 0, bool returnMaxOnNotFound = false)
        {
            for (int i = SecondOperand; i > 1; i--)
            {
                if (FirstOperand % i == 0)
                {
                    return i;
                }
            }
            for (int i = SecondOperand; i < FirstOperand; i++)
            {
                if (FirstOperand % i == 0)
                {
                    return i;
                }
            }

            return returnMaxOnNotFound ? FirstOperand : 1;
        }
    }
}
