using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F1Simulator.Utils.DatabaseConnectionFactory
{
    public interface IDatabaseConnection<TConnection>
    {
        TConnection Connect();
    }
}
