using GeneticAlgorithm.Business.Chromosomes;
using GeneticAlgorithm.Business.Interpreters;
using System;

namespace GeneticAlgorithm.Business.FitnessAlgorithms
{
    public class DifferenceAlgorithm : IFitnessAlgorithm
    {
        private int target;
        private IChromosomeInterpreter chromosomeInterpreter;

        public DifferenceAlgorithm(IChromosomeInterpreter chromosomeInterpreter, int target)
        {
            this.target = target;
            this.chromosomeInterpreter = chromosomeInterpreter;
        }

        public void CalculateScore(Chromosome chromosome)
        {
            double value = EvaluateChromosome(chromosome);

            if(SolutionFound(value))
            {
                chromosome.Score =  0;
            }
            else
            {
                chromosome.Score = ApplyScoring(target, value);
            }
        }

        private double ApplyScoring(int target, double value)
        {
            double score = (1 / (target - value));

            return score;
        }

        private bool SolutionFound(double value)
        {
            return value == target;
        }

        private double EvaluateChromosome(Chromosome chromosome)
        {
            return chromosomeInterpreter.EvaluateChromosome(chromosome);
        }
    }
}