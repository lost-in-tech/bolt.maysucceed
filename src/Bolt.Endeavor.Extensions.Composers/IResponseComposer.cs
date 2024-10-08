﻿namespace Bolt.Endeavor.Extensions.Composers;

public interface IResponseComposer
{
    Task<MaySucceed<ICollection<TResponse>>> Compose<TRequest, TResponse>(TRequest request, 
        CancellationToken cancellationToken = default);
}
