namespace Ecom.API.Helper
{
    public class ApiExceptions: ResponseAPI
    {
        public ApiExceptions(int StatusCode,string message=null,string details=null) : base(StatusCode, message)
        {
         
                Details = details;
            }
          
        public string Details { get; set; }

    }
}
