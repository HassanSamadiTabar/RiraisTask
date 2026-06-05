namespace RiraisTask.Application.Exceptions;

public class PersonValidationException(string message) : Exception(message);

public class PersonNotFoundException(string message) : Exception(message);

public class PersonConflictException(string message) : Exception(message);

public class PersonConcurrencyException(string message) : Exception(message);

public class PersonDatabaseException(string message, Exception innerException)
    : Exception(message, innerException);
