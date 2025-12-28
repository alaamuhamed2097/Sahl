using BL.GeneralService.Location;
using System.Net;

namespace BL.Contracts.GeneralService.Location;

public interface IIpGeolocationService
{
    Task<IpGeolocationResult> GetLocationFromIpAsync(string ipAddress);
    Task<IpGeolocationResult> GetLocationFromIpAsync(IPAddress ipAddress);
}