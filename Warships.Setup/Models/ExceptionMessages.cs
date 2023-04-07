using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warships.Setup.Models
{
    public static class ExceptionMessages
    {
        public static readonly string MissingFleetConfiguration = "Unable to create fleet: missing fleet configuration.";
        public static readonly string BuildShipCircuitBreakerException = "Exceeded maximum attemtps limit on building ship";
    }
}
