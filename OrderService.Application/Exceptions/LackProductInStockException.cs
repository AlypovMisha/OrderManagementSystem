namespace OrderService.Application.Exceptions
{
    public class LackProductInStockException : Exception
    {
        public LackProductInStockException() : base("Lack of product in stock")
        {
        }

        public LackProductInStockException(string? message) : base(message)
        {
        }

        public LackProductInStockException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
