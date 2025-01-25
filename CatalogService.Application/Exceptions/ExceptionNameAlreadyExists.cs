namespace CatalogService.Application.Exceptions
{
    public class ExceptionNameAlreadyExists : Exception
    {
        public ExceptionNameAlreadyExists() : base("The name of the product being created already exists in the database")
        {
        }

        public ExceptionNameAlreadyExists(string message) : base(message)
        {
        }
    }
}
