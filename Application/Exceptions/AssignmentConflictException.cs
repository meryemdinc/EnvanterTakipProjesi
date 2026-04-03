namespace Application.Exceptions;

    public class AssignmentConflictException : Exception
    {
        public AssignmentConflictException(string message) : base(message)
        {
        }
    }
