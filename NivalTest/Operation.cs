using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NivalTest
{
    class Operation
    {
        public Operand OperandType { get; set; }
        public int Mod { get; set; }
        public Guid Uid { get; set; }

        public Operation(Guid uid,Operand op, int mod)
        {
            OperandType = op;
            Mod = mod;
            Uid = uid;
        }

        public override string ToString()
        {
            return Uid + "\t" + OperandType + "\t" + Mod;
        }
    }
}
