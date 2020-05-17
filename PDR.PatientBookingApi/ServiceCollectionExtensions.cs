using Microsoft.Extensions.DependencyInjection;
using PDR.PatientBookingApi.Mappers;

namespace PDR.PatientBookingApi
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterApiModelMappers(this IServiceCollection collection)
        {
            collection.AddScoped<IBookingOrderMapper, BookingOrderMapper>();
        }
    }
}