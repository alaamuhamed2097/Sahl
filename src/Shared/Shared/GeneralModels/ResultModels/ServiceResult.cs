using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.GeneralModels.ResultModels
{
    public class ServiceResult<T> : OperationResult
    {
        public T Data { get; set; }

        public static ServiceResult<T> SuccessResult(T data) => new ServiceResult<T> { Success = true, Data = data };
        public static ServiceResult<T> FailureResult(string message, string errorCode = null) => new ServiceResult<T> { Success = false, Message = message, ErrorCode = errorCode };
    }
}
