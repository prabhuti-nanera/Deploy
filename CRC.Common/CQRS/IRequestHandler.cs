using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CRC.Common.Models;

namespace CRC.Common.CQRS
{
    public interface IRequest<TResponse> : MediatR.IRequest<BaseResponse<TResponse>>
    {
    }

    public interface IRequestHandler<in TRequest, TResponse> : MediatR.IRequestHandler<TRequest, BaseResponse<TResponse>>
        where TRequest : IRequest<TResponse>
    {
    }

    public abstract class RequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public abstract Task<BaseResponse<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);
    }
}
