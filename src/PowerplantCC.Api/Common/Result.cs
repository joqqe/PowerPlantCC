namespace PowerplantCC.Api.Common
{
    public class Result<T>
    {
        public T? Value { get; private set; }
        public Exception? Exception { get; private set; }
        public bool IsSuccess { get; private set; }

        private Result()
        { }

        public static Result<T> Success(T value) => new()
        {
            IsSuccess = true,
            Value = value,
            Exception = null
        };

        public static Result<T> Error(Exception ex) => new()
        {
            IsSuccess = false,
            Value = default,
            Exception = ex
        };
    }

    public class Result
    {
        public Exception? Exception { get; private set; }
        public bool IsSuccess { get; private set; }

        private Result()
        { }

        public static Result Success() => new()
        {
            IsSuccess = true,
            Exception = null
        };

        public static Result Error(Exception ex) => new()
        {
            IsSuccess = false,
            Exception = ex
        };
    }
}
