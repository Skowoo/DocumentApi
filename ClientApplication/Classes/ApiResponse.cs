namespace ClientApplication.Classes
{
    /// <summary>
    /// Custom record to store results from ApiServices
    /// </summary>
    /// <typeparam name="T"> Generic class of returned object </typeparam>
    /// <remarks>
    /// Creates new instance of ApiResponse
    /// </remarks>
    /// <param name="result"></param>
    /// <param name="value"></param>
    /// <param name="errors"></param>
    public readonly struct ApiResponse<T>(bool result, T? value, List<(string Property, string Message)>? errors)
    {
        /// <summary>
        /// True if the request was successful
        /// </summary>
        public bool Success { get; init; } = result;

        /// <summary>
        /// If the request was successful, the value returned
        /// </summary>
        public T? Value { get; init; } = value;

        /// <summary>
        /// If the request was not successful, the errors returned
        /// </summary>
        public List<(string Property, string Message)>? Errors { get; init; } = errors;
    }
}
