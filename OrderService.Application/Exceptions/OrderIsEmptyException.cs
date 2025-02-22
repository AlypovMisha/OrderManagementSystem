namespace OrderService.Application.Exceptions
{
    [Serializable]
    internal class OrderIsEmptyException : Exception
    {
        public OrderIsEmptyException() : base("The order must not be empty")
        {
        }

        public OrderIsEmptyException(string? message) : base(message)
        {
        }

        public OrderIsEmptyException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}