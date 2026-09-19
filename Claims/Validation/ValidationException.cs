namespace Claims.Validation
{
    /// <summary> Thrown when invalid data is entered</summary>
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message)
        {
        }
    }
}
