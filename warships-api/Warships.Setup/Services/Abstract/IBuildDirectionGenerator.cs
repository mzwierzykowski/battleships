using Warships.Setup.Models;

namespace Warships.Setup.Services.Abstract
{
    public interface IBuildDirectionGenerator
    {
        public BuildDirection GetRandom();
    }
}
