using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.GraphExplorer.Demo
{
    using Services;
    using Services.Interfaces;

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
