using GeneticAlgorithm.Business.Chromosomes;

namespace GeneticAlgorithm.Business.Interpreters
{
    public interface IChromosomeInterpreter
    {
        double EvaluateChromosome(Chromosome chromosone);
    }
}