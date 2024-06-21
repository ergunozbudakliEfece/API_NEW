using SQL_API.Wrappers.Abstract;

namespace SQL_API.Wrappers.Concrete
{
    public class SuccessResponse<T> : IResponse where T : class
    {
        public T? Value { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsSuccess { get; set; } = true;


        public SuccessResponse(T Value, string Message)
        {
            this.Value = Value;
            this.Message = Message;
        }
    }
}
