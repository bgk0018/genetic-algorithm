using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm.Business.Mutators
{
    public class BinarySequenceMutator : ISequenceMutator
    {
        public char Mutate(char piece)
        {
            if (piece == '0')
                return '1';
            else
                return '0';
        }
    }
}
