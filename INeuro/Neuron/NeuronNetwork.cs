using System;
using System.Diagnostics;
using System.IO;
using INeuro.Core;
using INeuro.Interface;

namespace INeuro.Neuron
{
    public class NeuronNetwork : INeuron2DNetwork
    {
        private string NeuronPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\Neuron.json";

        public INeuron2D[] Neurons { get; set; }

        public NeuronNetwork(int neuronCount, int weight, int height)
        {
            if(!File.Exists(NeuronPath))
            {
                Neurons = new Neuron[neuronCount];

                for (int i = 0; i < Neurons.Length; i++)
                {
                    Neurons[i] = new Neuron()
                    {
                        Weight = new int[weight, height]
                    };
                }
            }
            else
            {
                Neurons = JsonHelper.Deserialize<Neuron[]>(NeuronPath);
            }
        }

        ~NeuronNetwork()
        {
            JsonHelper.Serialize(Neurons, NeuronPath);
        }

        public int GetAnswer(int[,] input)
        {
            var answers = new int[Neurons.Length];

            for (int i = 0; i < Neurons.Length; i++)
            {
                answers[i] = Neurons[i].Handle(input);
            }

            int maxIndex = 0;

            for (int i = 0; i < answers.Length; i++)
            {
                if(answers[i] > answers[maxIndex])
                {
                    maxIndex = i;
                }
            }

            return maxIndex;
        }

        public void Study(int[,] input, int correctAnswer, int factor)
        {
            Neurons[correctAnswer].Study(input, factor);
        }
    }
}
