using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm.Business.Chromosomes
{
    public class Chromosome
    {
        public int ID { get; set; }

        public string Sequence { get; set; }

        public double Score { get; set; }
    }
}
