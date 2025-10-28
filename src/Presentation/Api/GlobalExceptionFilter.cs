using Microsoft.AspNetCore.Mvc.Filters;

namespace Api
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            //var response = new ResponseModel<object>
            //{
            //    Success = false,
            //    Message = NotifiAndAlertsResources.SomethingWentWrongAlert,
            //    Errors = new List<string> { context.Exception.Message }
            //};

            //context.Result = new ObjectResult(response)
            //{
            //    StatusCode = StatusCodes.Status500InternalServerError
            //};
        }
    }

}
