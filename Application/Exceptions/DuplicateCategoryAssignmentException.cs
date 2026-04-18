namespace Application.Exceptions;
public class DuplicateCategoryAssignmentException : Exception
{
    public DuplicateCategoryAssignmentException(string message) : base(message)
    {
    }
}