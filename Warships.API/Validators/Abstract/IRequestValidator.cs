using Warships.API.Models;

namespace Warships.API.Validators.Abstract
{
    public interface IRequestValidator
    {
        public bool IsValid(Request request);
    }
}
