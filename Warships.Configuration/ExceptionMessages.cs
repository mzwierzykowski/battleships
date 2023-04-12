namespace Warships.Configuration
{
    public static class ExceptionMessages
    {
        public static readonly string MissingFleetConfiguration = "Unable to create fleet: missing fleet configuration.";
        public static readonly string MissingBoardDimensionConfiguration = "Unable to generate board: missing board dimension configuration.";
        public static readonly string BuildShipCircuitBreakerException = "Exceeded maximum attemtps limit on building ship";
    }
}
