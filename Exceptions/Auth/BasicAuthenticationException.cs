using System;

namespace App.Exceptions.Auth
{
    [Serializable]
    public class BasicAuthenticationException : Exception
    {

        public BasicAuthenticationException() : base("Email e/ou senha inválidos")
        {

        }

        public BasicAuthenticationException(string? message) : base(message)
        {

        }
        //
        // Summary:
        //     Initializes a new instance of the System.ArgumentException class with a specified
        //     error message and a reference to the inner exception that is the cause of this
        //     exception.
        //
        // Parameters:
        //   message:
        //     The error message that explains the reason for the exception.
        //
        //   innerException:
        //     The exception that is the cause of the current exception. If the innerException
        //     parameter is not a null reference, the current exception is raised in a catch
        //     block that handles the inner exception.
        public BasicAuthenticationException(string? message, Exception? innerException) : base(message, innerException)
        {

        }

}
}