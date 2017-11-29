using System;
using AutoMapper;

namespace SFA.DAS.Support.Portal.Web.Services
{
    public interface IMappingService
    {
        TDest Map<TSource, TDest>(TSource source);

        TDest Map<TSource, TDest>(TSource source, Action<IMappingOperationOptions<TSource, TDest>> opts);
    }
}