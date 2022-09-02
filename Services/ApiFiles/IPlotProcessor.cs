using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ApiFiles
{
    public interface IPlotProcessor
    {
        Task<decimal[]> LoadPlot(string city, string plotNumber);
    }
}
