namespace Domain
{
    public class Result

    {
        public bool Success { get; private set; }

        public string Error { get; private set; }

        public bool Failure { get; private set; }

        protected Result(bool success, string error)
        {
            Success = success;
            Failure = !Success;
            Error = error;
        }

        public static Result Fail(string message)
            => new Result(false, message);

        public static Result Ok()
            => new Result(true, string.Empty);
    }

    public class Result<T> : Result

    {
        public T Value { get; set; }

        protected internal Result(T value, bool success, string error)

            : base(success, error)

        {
            Value = value;
        }

        public static Result<T> Ok(T value)
            => new Result<T>(value, true, string.Empty);
    }
}