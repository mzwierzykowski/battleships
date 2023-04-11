using Microsoft.Extensions.Options;
using System.Configuration;
using Warships.API.Models;
using Warships.API.Validators.Abstract;
using Warships.Configuration;
using Warships.Setup.Models;

namespace Warships.API.Validators
{
    public class RequestValidator : IRequestValidator
    {
        private readonly BoardDimension _boardDimension;
        public RequestValidator(IOptions<BoardDimension> boardDimension)
        {
            _boardDimension = boardDimension.Value;
        }

        public bool IsValid(Request request)
        {
            if (_boardDimension == null)
                throw new ConfigurationErrorsException(ExceptionMessages.MissingBoardDimensionConfiguration);
            if (string.IsNullOrEmpty(request.PointId))
                return false;
            string[] coordinates = request.PointId.Split('.');
            if (coordinates.Length != 2)
                return false;
            if (!int.TryParse(coordinates[0], out int x))
                return false;
            if (!int.TryParse(coordinates[1], out int y))
                return false;
            if (x < 0 || x > _boardDimension.Width - 1)
                return false;
            if (y < 0 || y > _boardDimension.Height - 1)
                return false;

            return true;
        }
    }
}
