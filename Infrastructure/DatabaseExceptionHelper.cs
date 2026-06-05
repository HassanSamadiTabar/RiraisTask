using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RiraisTask.Application.Exceptions;

namespace RiraisTask.Infrastructure;

public static class DatabaseExceptionHelper
{
    public static Exception MapDbUpdateException(DbUpdateException exception, string operation)
    {
        if (exception is DbUpdateConcurrencyException)
        {
            return new PersonConcurrencyException("The record was modified by another process.");
        }

        if (IsUniqueConstraintViolation(exception))
        {
            return new PersonConflictException("National code already exists.");
        }

        return new PersonDatabaseException($"Database error while {operation}.", exception);
    }

    private static bool IsUniqueConstraintViolation(DbUpdateException exception)
    {
        for (Exception? current = exception.InnerException; current is not null; current = current.InnerException)
        {
            if (current is SqlException sqlException && (sqlException.Number == 2601 || sqlException.Number == 2627))
            {
                return true;
            }
        }

        return false;
    }
}
