using GeneticAlgorithm.Business.ChromosomePools;
using GeneticAlgorithm.Business.FitnessAlgorithms;
using GeneticAlgorithm.Business.Mutators;
using GeneticAlgorithm.Business.Selectors;
using System;

namespace GeneticAlgorithm.Business.Habitats
{
    public class HabitatConfiguration
    {
        public IPoolGenerator PoolGenerator { get; set; }
        public IFitnessAlgorithm FitnessAlgorithm { get; set; }
        public IReproductionSelector ReproductionSelector { get; set; }
        public ISequenceMutator SequenceMutator { get; set; }
        public double MutationRate { get; set; }
        public Random NumberGenerator { get; set; }
        public double CrossoverRate { get; internal set; }
    }
}