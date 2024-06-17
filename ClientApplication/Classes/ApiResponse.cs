namespace ClientApplication.Classes
{
    /// <summary>
    /// Custom record to store requests results from ApiServices calls
    /// </summary>
    /// <typeparam name="T"> Generic class of returned object </typeparam>
    /// <remarks>
    /// Creates new instance of ApiResponse
    /// </remarks>
    /// <param name="result"> Boolean value which indicates if request suceeded </param>
    /// <param name="value"> If result succeeded property stores acquired data </param>
    /// <param name="errors"> If result failed property stores returned errors </param>
    public readonly struct ApiResponse<T>(bool result, T? value, List<(string Property, string Message)>? errors)
    {
        /// <summary>
        /// True if the request was successful
        /// </summary>
        public bool IsSuccess { get; init; } = result;

        /// <summary>
        /// If the request was successful, the value returned
        /// </summary>
        public T? Data { get; init; } = value;

        /// <summary>
        /// If the request was not successful, the errors returned
        /// </summary>
        public List<(string Property, string Message)>? ErrorDetails { get; init; } = errors;
    }
}
