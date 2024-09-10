using Microsoft.AspNetCore.Mvc;
using server.DTOs;

namespace server.Services;
public class ResponseService
{
    public ActionResult ResponseMessage<T>(ResponseStatus status,T data, string? message = null)
    {
        if (string.IsNullOrEmpty(message))
        {
            message = status switch
            {
                ResponseStatus.Success => "Success",
                ResponseStatus.Error => "Error",
                _ => "Invalid status"
            };
        }

        int code = status switch
        {
            ResponseStatus.Success => StatusCodes.Status200OK,
            ResponseStatus.Error => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };

        return new ObjectResult(new ResponseModel<T>
        {
            Code = code,
            Message = message,
            Data = data
        })
        {
            StatusCode = code
        };
    }

    public ActionResult ResponseSuccess(string message)
    {
        return new ObjectResult(new ResponseModel<string>
        {
            Code = StatusCodes.Status200OK,
            Message = message,
            Data = null
        })
        {
            StatusCode = StatusCodes.Status200OK
        };
    }

    public ActionResult ResponseError(string message)
    {
        return new ObjectResult(new ResponseModel<string>
        {
            Code = StatusCodes.Status500InternalServerError,
            Message = message,
            Data = null
        })
        {
            StatusCode = StatusCodes.Status200OK
        };
    }
}
