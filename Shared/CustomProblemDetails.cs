using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace RanqueDev.Api.Shared
{
    public class CustomProblemDetails : ProblemDetails
    {
        public List<string> Errors { get; private set; } = new List<string>();
        public CustomProblemDetails(HttpStatusCode status, string? details = null, IEnumerable<string>? errors = null)
        {
            Title = status switch
            {
                HttpStatusCode.BadRequest => "Ocorreu um ou mais erros de validações.",
                HttpStatusCode.InternalServerError => "Erro interno do servidor.",
                _ => "Ocorreu um erro."
            };
            Status = (int)status;
            Detail= details;
            if(errors is not null)
            {
                if (errors.Count() == 1)
                {
                    Detail = errors.First();
                }
                else if(errors.Count() > 1)
                {
                    Detail = "Varios erros ocorreram";
                    Errors.AddRange(errors);
                }
            }
        }
        public CustomProblemDetails(HttpStatusCode status,HttpRequest request,string? detail = null,IEnumerable<string>? errors = null) 
            : this(status,detail, errors)
        {
            Instance = request.Path;
        }
        private CustomProblemDetails(HttpStatusCode status) => Errors = new List<string>();
    }
}
