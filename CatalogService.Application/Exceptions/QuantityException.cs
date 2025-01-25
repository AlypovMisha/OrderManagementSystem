namespace CatalogService.Application.Exceptions
{
    public class QuantityException : Exception
    {
        public QuantityException() : base("Insufficient quantity of product")
        {
        }

        public QuantityException(string message) : base(message)
        {
        }
    }
}
