using System.Diagnostics.CodeAnalysis;
using Warships.Setup.Models;
using Warships.Setup.Services.Abstract;

namespace Warships.Setup.Services
{
    [ExcludeFromCodeCoverage]
    internal class BuildDirectionGenerator : IBuildDirectionGenerator
    {
        public BuildDirection GetRandom()
        {
            var rnd = new Random();
            int randomNumber = rnd.Next(0, 1000);
            var direction = (BuildDirection)(randomNumber % 2);
            return direction;
        }
    }
}
