namespace Infrastructure.Core
{
    /// <summary>
    /// Defines verbosity levels used by logging utilities.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>All log messages.</summary>
        All     = 0,
        /// <summary>Diagnostic messages helpful during development.</summary>
        Debug   = 1,
        /// <summary>Potential issues that are not necessarily errors.</summary>
        Warning = 3,
        /// <summary>Recoverable or unrecoverable errors.</summary>
        Error   = 4
        
    }
}
