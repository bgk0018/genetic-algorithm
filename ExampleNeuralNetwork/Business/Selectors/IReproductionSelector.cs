using GeneticAlgorithm.Business.Chromosomes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm.Business.Selectors
{
    public interface IReproductionSelector
    {
        Chromosome SelectCandidate(IList<Chromosome> pool);
    }
}
