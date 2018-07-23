using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INeuro.Interface
{
    interface INeuron2DNetwork
    {
        INeuron2D[] Neurons { get; set; }
        int GetAnswer(int[,] input);
        void Study(int[,] input, int correctAnswer, int factor);
    }
}
