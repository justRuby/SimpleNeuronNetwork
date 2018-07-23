using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INeuro.Interface
{
    public interface INeuron2D
    {
        int[,] Weight { get; set; }
        int Handle(int[,] input);
        void Study(int[,] input, int factor);
    }
}
