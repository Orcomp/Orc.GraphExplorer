using Orc.GraphExplorer.Demo.Mock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.GraphExplorer.Demo
{
    public class MainViewModel
    {
        public IGraphDataService GraphDataService
        {
            get
            {
                return new CsvGraphDataService();
            }
        }
    }
}
