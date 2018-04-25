using System.Collections.Generic;

namespace NivalTest
{
    class Calculator
    {
        public int Calculate (List<Operation> operations)
        {
            int result = 0;

            for (int i = 0; i < operations.Count; i++)
            {
                var o = operations[i];

                switch(o.OperandType)
                {
                    case Operand.Add:
                        result += o.Mod;
                        break;
                    case Operand.Subtract:
                        result -= o.Mod;
                        break;
                    case Operand.Multiply:
                        result *= o.Mod;
                        break;
                    case Operand.Divide:
                        result /= o.Mod;
                        break;
                }
            }

            return result;
        }
    }
}
