using GeneticAlgorithm.Business.ChromosomePools;
using GeneticAlgorithm.Business.Chromosomes;
using GeneticAlgorithm.Business.FitnessAlgorithms;
using GeneticAlgorithm.Business.Habitats;
using GeneticAlgorithm.Business.Interpreters;
using GeneticAlgorithm.Business.Mutators;
using GeneticAlgorithm.Business.Selectors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm
{
    class Program
    {
        static int goalValue;
        static Habitat myHabitat;
        static Random numberGenerator;
        static Chromosome source;
        static Chromosome target;
        static Chromosome answer;
        static List<Chromosome> pool;
        static List<Chromosome> nextPool;
        static int generation;
        static Chromosome alpha;

        static int chromosomeLength = 80;
        static int populationSize = 50;

        static void Main(string[] args)
        {
            bool exit = false;

            do
            {
                ExecuteApplication();

                exit = DoesExit();
            } while (!exit);
        }

        private static bool DoesExit()
        {
            Console.WriteLine("Continue? (Y/N):");
            string response = Console.ReadLine();

            return response.ToUpper() == "N";
        }

        private static void ExecuteApplication()
        {
            Initialize();

            while (!IsCandidateAnswer(source) && !IsCandidateAnswer(target))
            {
                generation++;

                if (generation % 100 == 0)
                {
                    DisplayInterumData();
                }

                while (pool.Count > 1)
                {
                    SetCandidates();

                    ApplyModification();

                    Rescore();

                    DetermineAlpha();

                    MoveToNextPool();
                }
                pool = new List<Chromosome>();
                pool.AddRange(nextPool);
                nextPool = new List<Chromosome>();
            }

            answer = SelectAnswerCandidate(source, target);

            DisplayAnswer();
        }

        private static void Initialize()
        {
            goalValue = GetGoalValue();

            numberGenerator = new Random();

            HabitatConfiguration config = GetHabitatConfiguration();

            myHabitat = new Habitat(config);

            nextPool = new List<Chromosome>();
            source = new Chromosome();
            source.Score = int.MaxValue;
            target = new Chromosome();
            target.Score = int.MaxValue;
            alpha = new Chromosome();
            alpha.Score = double.MaxValue;
            pool = myHabitat.GenerateSeedPool(populationSize);

            myHabitat.Score(pool);
        }

        private static HabitatConfiguration GetHabitatConfiguration()
        {
            HabitatConfiguration config = new HabitatConfiguration();

            config.CrossoverRate = .7;
            config.FitnessAlgorithm = new DifferenceAlgorithm(new MathChromosomeInterpreter(), goalValue);
            config.MutationRate = .001;
            config.NumberGenerator = numberGenerator;
            config.PoolGenerator = new BinaryChromosomePoolGenerator(numberGenerator, new BinaryChromosomePoolConfiguration() { ChromosomeSequenceLength = chromosomeLength, PadToNibble = false });
            config.ReproductionSelector = new RouletteWheelSelector(numberGenerator);
            config.SequenceMutator = new BinarySequenceMutator();

            return config;
        }

        private static void DetermineAlpha()
        {
            if (source.Score < alpha.Score)
            {
                alpha.Score = source.Score;
                alpha.ID = source.ID;
                alpha.Sequence = source.Sequence;
            }

            if (target.Score < alpha.Score)
            {
                alpha.Score = target.Score;
                alpha.ID = target.ID;
                alpha.Sequence = target.Sequence;
            }
        }

        private static void DisplayInterumData()
        {
            Console.Clear();
            Console.WriteLine("Answer Value: " + goalValue);
            Console.WriteLine("Alpha ID:"+ alpha.ID);
            Console.WriteLine("Distance from Answer:" + alpha.Score);
            Console.WriteLine("Closest Answer:" + new MathChromosomeInterpreter().EvaluateChromosome(alpha));
            Console.WriteLine("Generation: " + generation);
            Console.WriteLine("Source Score: " + source.Score);
            Console.WriteLine("Target Score: " + target.Score);
        }

        private static void MoveToNextPool()
        {
            nextPool.Add(target);
            nextPool.Add(source);
        }

        private static void DisplayAnswer()
        {
            Console.WriteLine(generation + " generations processed");
            Console.WriteLine("Sequence: " + answer.Sequence);
            Console.WriteLine("Decoded: " + DecodeSequence(answer.Sequence));
        }

        private static string DecodeSequence(string sequence)
        {
            MathChromosomeInterpreter interpreter = new MathChromosomeInterpreter();
            string decoded = string.Empty;

            var decodedList = interpreter.DecodeSequence(sequence);

            foreach(string element in decodedList)
            {
                decoded += element;
            }

            return decoded;
        }

        private static Chromosome SelectAnswerCandidate(Chromosome source, Chromosome target)
        {
            return IsCandidateAnswer(source) ? source : target;
        }

        private static int GetGoalValue()
        {
            Console.WriteLine("Please enter a number");
            string userInput = Console.ReadLine();
            int output = 0;

            while (!int.TryParse(userInput, out output))
            {
                Console.WriteLine("Invalid number, Please enter a number");
                userInput = Console.ReadLine();
            }

            return output;
        }

        private static bool IsCandidateAnswer(Chromosome candidate)
        {
            return candidate.Score == 0;
        }

        private static void SetCandidates()
        {
            source = myHabitat.ExtractCandidate(pool);
            target = myHabitat.ExtractCandidate(pool);
        }

        private static void ApplyModification()
        {
            myHabitat.ApplyCrossover(source, target);

            myHabitat.ApplyMutation(source);
            myHabitat.ApplyMutation(target);
        }

        private static void Rescore()
        {
            myHabitat.Score(source);
            myHabitat.Score(target);
        }

        

        

        

        
    }
}
