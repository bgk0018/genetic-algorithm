using GeneticAlgorithm.Business.Chromosomes;

namespace GeneticAlgorithm.Business.FitnessAlgorithms
{
    public interface IFitnessAlgorithm
    {
        void CalculateScore(Chromosome chromosome);
    }
}