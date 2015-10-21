using GeneticAlgorithm.Business.Chromosomes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm.Business.ChromosomePools
{
    class BinaryChromosomePoolGenerator : IPoolGenerator
    {
        BinaryChromosomePoolConfiguration config;
        Random numberGenerator;

        public BinaryChromosomePoolGenerator(Random numberGenerator)
        {
            this.numberGenerator = numberGenerator;
            this.config = new BinaryChromosomePoolConfiguration();
        }

        public BinaryChromosomePoolGenerator(Random numberGenerator, BinaryChromosomePoolConfiguration config)
        {
            this.numberGenerator = numberGenerator;
            this.config = config;
        }

        public List<Chromosome> GeneratePool(int count)
        {
            List<Chromosome> pool = new List<Chromosome>();

            for(int i =1; i <= count; i++)
            {
                Chromosome chromosome = new Chromosome();

                chromosome.ID = i;
                chromosome.Sequence = GenerateSequence();

                pool.Add(chromosome);
            }

            return pool;
        }

        private string GenerateSequence()
        {
            string sequence = string.Empty;
            int length = DetermineSequenceLength();

            sequence = BuildSequence(length);

            if(config.PadToNibble)
            {
                sequence = PadSequence(sequence);
            }

            return sequence;
        }

        private string PadSequence(string sequence)
        {
            return sequence.PadLeft(sequence.Length % 4, '0');
        }

        private string BuildSequence(int length)
        {
            string sequence = string.Empty;

            for (int i = 0; i < length; i++)
            {
                sequence += GetBit();
            }

            return sequence;
        }

        private string GetBit()
        {
            return numberGenerator.Next(0, 2).ToString();
        }

        private int DetermineSequenceLength()
        {
            return (config.ChromosomeSequenceLength != 0 ? config.ChromosomeSequenceLength : numberGenerator.Next(0, 100));
        }
    }
}
