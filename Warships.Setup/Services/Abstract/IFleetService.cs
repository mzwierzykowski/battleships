using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warships.Setup.Models;

namespace Warships.Setup.Services.Abstract
{
    public interface IFleetService
    {
        public List<Ship> BuildFleet();
    }
}
