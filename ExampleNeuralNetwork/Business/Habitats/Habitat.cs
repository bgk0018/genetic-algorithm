using GeneticAlgorithm.Business.ChromosomePools;
using GeneticAlgorithm.Business.Chromosomes;
using GeneticAlgorithm.Business.FitnessAlgorithms;
using GeneticAlgorithm.Business.Mutators;
using GeneticAlgorithm.Business.Selectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm.Business.Habitats
{
    public class Habitat
    {
        private readonly IPoolGenerator poolGenerator;
        private readonly IFitnessAlgorithm fitnessAlgorithm;
        private readonly IReproductionSelector reproductionSelector;
        private readonly ISequenceMutator sequenceMutator;
        private readonly double mutationRate;
        private readonly double crossoverRate;
        private readonly Random numberGenerator;

        public Habitat(HabitatConfiguration config)
        {
            this.poolGenerator = config.PoolGenerator;
            this.fitnessAlgorithm = config.FitnessAlgorithm;
            this.reproductionSelector = config.ReproductionSelector;
            this.mutationRate = config.MutationRate;
            this.crossoverRate = config.CrossoverRate;
            this.numberGenerator = config.NumberGenerator;
            this.sequenceMutator = config.SequenceMutator;
        }

        public List<Chromosome> GenerateSeedPool(int count)
        {
            return poolGenerator.GeneratePool(count);
        }

        public void Score(Chromosome chromosome)
        {
            fitnessAlgorithm.CalculateScore(chromosome);
        }

        public void Score(IList<Chromosome> pool)
        {
            foreach(Chromosome chromosome in pool)
            {
                Score(chromosome);
            }
        }

        public Chromosome ExtractCandidate(IList<Chromosome> pool)
        {
            Chromosome candidate = new Chromosome();
            candidate = reproductionSelector.SelectCandidate(pool);

            pool.Remove(candidate);

            return candidate;
        }

        public void ApplyCrossover(Chromosome source, Chromosome target)
        {
            int crossoverPoint = 0;

            if(CanCrossover())
            {
                crossoverPoint = SelectCrossoverPosition(source);

                source.Sequence = GetPreCrossoverSequence(source, crossoverPoint) + GetPostCrossoverSequence(target, crossoverPoint);
                target.Sequence = GetPreCrossoverSequence(target, crossoverPoint) + GetPostCrossoverSequence(source, crossoverPoint);
            }
        }

        private string GetPostCrossoverSequence(Chromosome chromosome, int crossoverPoint)
        {
            return chromosome.Sequence.Substring(crossoverPoint);
        }

        private string GetPreCrossoverSequence(Chromosome chromosome, int crossoverPoint)
        {
            return chromosome.Sequence.Substring(0, crossoverPoint);
        }

        private int SelectCrossoverPosition(Chromosome source)
        {
            return numberGenerator.Next(0, source.Sequence.Length+1);
        }

        private bool CanCrossover()
        {
            return (numberGenerator.NextDouble() > crossoverRate);
        }

        public void ApplyMutation(Chromosome target)
        {
            string mutatedSequence = string.Empty;

            foreach(char block in target.Sequence)
            {
               mutatedSequence += Mutate(block);
            }

            target.Sequence = mutatedSequence;
        }

        private char Mutate(char block)
        {
            if(CanMutate())
            {
                return sequenceMutator.Mutate(block);
            }

            return block;
        }

        private bool CanMutate()
        {
            return (numberGenerator.NextDouble() < mutationRate);
        }
        
    }
}
