using GeneticAlgorithm.Business.Chromosomes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm.Business.Selectors
{
    public class RouletteWheelSelector : IReproductionSelector
    {
        private Random numberGenerator;

        public RouletteWheelSelector(Random numberGenerator)
        {
            this.numberGenerator = numberGenerator;
        }

        public Chromosome SelectCandidate(IList<Chromosome> pool)
        {
            double total = SumScores(pool);
            double selector = GenerateSelector(total);
            Chromosome candidate = new Chromosome();

            ShufflePool(pool);

            candidate = SelectCandidateFrom(pool, selector);

            return candidate;
        }

        private Chromosome SelectCandidateFrom(IList<Chromosome> pool, double selector)
        {
            Chromosome candidate = new Chromosome();
            double runningTotal = 0;

            foreach (Chromosome potentialCandidate in pool)
            {
                if(IsSelectedCandidate(potentialCandidate, runningTotal, selector))
                {
                    return potentialCandidate;
                }

                runningTotal += Math.Abs(potentialCandidate.Score);
            }

            return candidate;
        }

        private bool IsSelectedCandidate(Chromosome potentialCandidate, double runningTotal, double selector)
        {
            return (Math.Abs(potentialCandidate.Score) + runningTotal >= selector);
        }

        private double GenerateSelector(double total)
        {
            return numberGenerator.NextDouble() * total;
        }

        private void ShufflePool(IList<Chromosome> pool)
        {
            int n = pool.Count;
            while (n > 1)
            {
                n--;
                int k = numberGenerator.Next(n + 1);

                Chromosome value = pool[k];
                pool[k] = pool[n];
                pool[n] = value;
            }
        }

        private double SumScores(IList<Chromosome> pool)
        {
            double sum = 0;

            foreach(Chromosome chromosome in pool)
            {
                sum += Math.Abs(chromosome.Score);
            }

            return sum;
        }
    }
}
