namespace SQL_API.Wrappers.Abstract
{
    public interface IResponse
    {
        string Message { get; set; }
        bool IsSuccess { get; set; }
    }
}
