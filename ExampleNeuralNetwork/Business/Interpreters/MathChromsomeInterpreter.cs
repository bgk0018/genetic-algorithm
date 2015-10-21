using GeneticAlgorithm.Business.Chromosomes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm.Business.Interpreters
{
    public class MathChromosomeInterpreter : IChromosomeInterpreter
    {
        private readonly Dictionary<string, string> genes;
        private readonly string operatorSet = @"+-*/";

        public MathChromosomeInterpreter()
        {
            genes = new Dictionary<string, string>();
            GenerateGenes();
        }

        private void GenerateGenes()
        {
            PopulateGenesOperands();
            PopulateGenesOperators();
        }

        public double EvaluateChromosome(Chromosome chromosome)
        {
            IList<string> decodedSequence = DecodeSequence(chromosome.Sequence);
            return EvaluateDecodedSequence(decodedSequence);
        }

        public IList<string>DecodeSequence(string sequence)
        {
            IList<string> elements = new List<string>();
            string parse = sequence;

            for (int i = 0; i < parse.Length; i++)
            {
                if (genes.ContainsValue(parse.Substring(0, 4)))
                {
                    elements.Add(genes.FirstOrDefault(x => x.Value == parse.Substring(0, 4)).Key);
                }

                parse = parse.Substring(4);
            }

            elements = CleanDecodedSequence(elements);

            return elements;
        }

        private IList<string> CleanDecodedSequence(IList<string> elements)
        {
            List<string> cleaned = new List<string>();
            bool getOperand = true;

            foreach(string element in elements)
            {
                if(getOperand && IsOperand(element))
                {
                    cleaned.Add(element);
                    getOperand = false;
                }
                else if(!getOperand && IsOperator(element))
                {
                    cleaned.Add(element);
                    getOperand = true;
                }
            }

            if(cleaned.Count > 0 && IsOperator(cleaned.Last()))
            {
                cleaned.RemoveAt(cleaned.Count-1);
            }

            return cleaned;
        }

        private bool IsOperand(string element)
        {
            int unneeded = 0;
            return (int.TryParse(element, out unneeded));
        }

        private bool IsOperator(string element)
        {
            return (operatorSet.Contains(element));
        }

        private double EvaluateDecodedSequence(IList<string> decodedSequence)
        {
            double answer = 0;
            string operation = string.Empty;
            double operand = 0;
            bool IsFirstValue = true;

            foreach(string element in decodedSequence)
            {
                if(IsFirstValue)
                {
                    double.TryParse(element, out answer);
                    IsFirstValue = false;
                }
                else
                {
                    if(IsOperator(element))
                    {
                        operation = element;
                    }
                    else if(IsOperand(element))
                    {
                        double.TryParse(element, out operand);
                        answer = Operate(answer, operation, operand);
                    }
                }
            }

            return answer;
        }

        private double Operate(double answer, string operation, double operand)
        {
            switch(operation)
            {
                case "+":
                    return answer + operand;
                case "-":
                    return answer - operand;
                case "*":
                    return answer * operand;
                case @"/":
                    return answer / operand;
                default:
                    throw new ArgumentException("Invalid operation: " + operation);
            }
        }

        private void PopulateGenesOperators()
        {
            genes.Add("+", "1010");
            genes.Add("-", "1011");
            genes.Add("*", "1100");
            genes.Add(@"/", "1101");
        }

        private void PopulateGenesOperands()
        {
            for (int i = 1; i <= 9; i++)
            {
                string bits = GetBitRepresentation(i);

                genes.Add(i.ToString(), bits);
            }
        }

        private string GetBitRepresentation(int i)
        {
            string s = Convert.ToString(i, 2);
            string result = string.Empty;

            int[] bits = s.PadLeft(4, '0')
                         .Select(c => int.Parse(c.ToString()))
                         .ToArray();

            foreach (int bit in bits)
            {
                result += bit.ToString();
            }

            return result;
        }
    }
}