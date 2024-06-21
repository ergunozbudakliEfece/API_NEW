using SQL_API.Wrappers.Abstract;

namespace SQL_API.Wrappers.Concrete
{
    public class ErrorResponse : IResponse
    {
        public string Message { get; set; } = string.Empty;
        public bool IsSuccess { get; set; } = false;

        public ErrorResponse(string Message)
        {
            this.Message = Message;
        }

        public ErrorResponse(Exception Ex)
        {
            this.Message = Ex.Message;
        }
    }
}
