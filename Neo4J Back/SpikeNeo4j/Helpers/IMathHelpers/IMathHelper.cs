using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Helpers.IMathHelpers
{
    public interface IMathHelper
    {
        decimal[] MidPoint(double lat1, double lon1, double lat2, double lon2);
    }
}
