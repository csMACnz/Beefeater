using System;

namespace Beefeater
{
    // This exception is intended to be thrown as a fatal error, and should not be caught.
#pragma warning disable CA1032 // Implement standard exception constructors
#pragma warning disable CA1064 // Exceptions should be public
#pragma warning disable S3871 // Exception types should be "public"
    internal class PanicException : Exception
#pragma warning restore S3871 // Exception types should be "public"
#pragma warning restore CA1064 // Exceptions should be public
#pragma warning restore CA1032 // Implement standard exception constructors
    {
    }
}