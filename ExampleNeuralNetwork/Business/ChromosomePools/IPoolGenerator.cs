using GeneticAlgorithm.Business.Chromosomes;
using System.Collections.Generic;

namespace GeneticAlgorithm.Business.ChromosomePools
{
    public interface IPoolGenerator
    {
        List<Chromosome> GeneratePool(int count);
    }
}