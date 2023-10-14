﻿namespace ConsoleApp.Parsing.Exceptions
{
    public abstract class InvalidTableFormatException : Exception
    {
        protected InvalidTableFormatException(string? message) : base($"Invalid table format: {message}")
        {
        }
    }
}
