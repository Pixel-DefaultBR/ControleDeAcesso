namespace ControleDeAcesso.Model.Response
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Value { get; set; }

        private Result(bool isSuccess, T? value = default, string? errorMessage = null)
        {
            IsSuccess = isSuccess;
            Value = value;
            ErrorMessage = errorMessage;
        }

        public static Result<T> Success(T value) => new(true, value);
        public static Result<T> Failure(string errorMessage) => new(false, default, errorMessage);
    }

}