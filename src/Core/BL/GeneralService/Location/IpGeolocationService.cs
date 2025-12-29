using BL.Contracts.GeneralService.Location;
using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace BL.GeneralService.Location;

/// <summary>
/// Service for retrieving geolocation information from IP addresses using MaxMind GeoIP2 database
/// </summary>
public class IpGeolocationService : IIpGeolocationService, IDisposable
{
    private readonly ILogger<IpGeolocationService> _logger;
    private readonly IConfiguration _configuration;
    private readonly Dictionary<string, CountryInfo> _countryMappings;
    private readonly DatabaseReader? _databaseReader;
    private bool _disposed = false;

    public IpGeolocationService(
        ILogger<IpGeolocationService> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _countryMappings = LoadCountryMappings();
        _databaseReader = InitializeMaxMindDatabase();
    }

    private DatabaseReader? InitializeMaxMindDatabase()
    {
        try
        {
            var databasePath = _configuration["MaxMind:DatabasePath"];
            if (string.IsNullOrEmpty(databasePath))
            {
                _logger.LogWarning("MaxMind database path not configured. Using fallback method.");
                return null;
            }

            var fullPath = Path.Combine(AppContext.BaseDirectory, databasePath);
            if (!File.Exists(fullPath))
            {
                _logger.LogError("MaxMind database file not found at: {DatabasePath}", fullPath);
                return null;
            }

            var reader = new DatabaseReader(fullPath);
            _logger.LogInformation("MaxMind GeoIP2 database initialized successfully from: {DatabasePath}", fullPath);
            return reader;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize MaxMind database");
            return null;
        }
    }

    private Dictionary<string, CountryInfo> LoadCountryMappings()
    {
        try
        {
            var assembly = typeof(IpGeolocationService).Assembly;
            var resourceName = assembly.GetManifestResourceNames()
                .FirstOrDefault(r => r.EndsWith("countries.json"));

            if (string.IsNullOrEmpty(resourceName))
                throw new InvalidOperationException("countries.json not found as embedded resource.");

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
                throw new InvalidOperationException("Failed to load embedded resource: countries.json");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var countryDict = JsonSerializer.Deserialize<Dictionary<string, CountryInfo>>(stream, options);
            return countryDict ?? new Dictionary<string, CountryInfo>();
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Failed to load country mappings from countries.json");
            return new Dictionary<string, CountryInfo>
            {
                ["US"] = new CountryInfo { Code = "US", Name = "United States", IsMiddleEast = false }
            };
        }
    }

    public async Task<IpGeolocationResult> GetLocationFromIpAsync(string ipAddress)
    {
        if (!IsValidIpAddress(ipAddress))
        {
            return CreateDefaultResult("Invalid IP address format");
        }

        return await Task.Run(() => GetLocationFromMaxMind(ipAddress));
    }

    public Task<IpGeolocationResult> GetLocationFromIpAsync(IPAddress ipAddress)
        => GetLocationFromIpAsync(ipAddress.ToString());

    private IpGeolocationResult GetLocationFromMaxMind(string ipAddress)
    {
        if (_databaseReader == null)
        {
            _logger.LogWarning("MaxMind database not available for IP: {IpAddress}", ipAddress);
            return CreateDefaultResult("MaxMind database not available");
        }

        try
        {
            if (!IPAddress.TryParse(ipAddress, out var ip))
            {
                return CreateDefaultResult("Invalid IP address format");
            }

            var response = _databaseReader.City(ip);
            var countryCode = response.Country?.IsoCode?.ToUpper() ?? "US";
            var countryInfo = _countryMappings.TryGetValue(countryCode, out var info) ? info : null;

            var result = new IpGeolocationResult
            {
                Success = true,
                IpAddress = ipAddress,
                CountryCode = countryCode,
                CountryName = countryInfo?.Name ?? response.Country?.Name ?? "Unknown",
                Region = response.MostSpecificSubdivision?.Name,
                City = response.City?.Name,
                Latitude = (response.Location?.Latitude),
                Longitude = (response.Location?.Longitude),
                IsMiddleEast = countryInfo?.IsMiddleEast == true,
                ServiceUsed = "MaxMind GeoIP2"
            };

            if (_configuration.GetValue("MaxMind:EnableLogging", false))
            {
                _logger.LogDebug("IP {IpAddress} resolved to {Country}, {City}",
                    ipAddress, result.CountryName, result.City);
            }

            return result;
        }
        catch (AddressNotFoundException)
        {
            _logger.LogWarning("IP address not found in MaxMind database: {IpAddress}", ipAddress);
            return CreateDefaultResult("IP address not found in database");
        }
        catch (GeoIP2Exception ex)
        {
            _logger.LogError(ex, "MaxMind GeoIP2 error for IP: {IpAddress}", ipAddress);
            return CreateErrorResult("Geolocation lookup failed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during geolocation for IP: {IpAddress}", ipAddress);
            return CreateErrorResult("Service error");
        }
    }

    private bool IsValidIpAddress(string ipAddress)
    {
        if (string.IsNullOrWhiteSpace(ipAddress)) return false;
        if (IPAddress.TryParse(ipAddress, out var addr))
        {
            return !IPAddress.IsLoopback(addr) && !IsPrivateIp(addr);
        }
        return false;
    }

    private bool IsPrivateIp(IPAddress addr)
    {
        if (addr.IsIPv4MappedToIPv6 || addr.IsIPv6Teredo)
            addr = addr.MapToIPv4();

        if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        {
            var bytes = addr.GetAddressBytes();
            return
                (bytes[0] == 10) ||
                (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31) ||
                (bytes[0] == 192 && bytes[1] == 168);
        }
        return false;
    }

    private IpGeolocationResult CreateDefaultResult(string message)
    {
        var defaultCode = _configuration["MaxMind:DefaultCountryCode"] ?? "US";
        var countryInfo = _countryMappings.TryGetValue(defaultCode.ToUpper(), out var info) ? info : null;

        return new IpGeolocationResult
        {
            Success = false,
            CountryCode = defaultCode.ToUpper(),
            CountryName = countryInfo?.Name ?? "United States",
            ErrorMessage = message,
            IsMiddleEast = countryInfo?.IsMiddleEast == true,
            ServiceUsed = "MaxMind GeoIP2 (Fallback)"
        };
    }

    private IpGeolocationResult CreateErrorResult(string message)
    {
        return new IpGeolocationResult
        {
            Success = false,
            ErrorMessage = message,
            ServiceUsed = "MaxMind GeoIP2"
        };
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _databaseReader?.Dispose();
            _disposed = true;
        }
    }

    public class CountryInfo
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsMiddleEast { get; set; }
    }
}

/// <summary>
/// Result object for IP geolocation lookup
/// </summary>
public class IpGeolocationResult
{
    public bool Success { get; set; }
    public string? IpAddress { get; set; }
    public string? CountryCode { get; set; }
    public string? CountryName { get; set; }
    public string? Region { get; set; }
    public string? City { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? Isp { get; set; }
    public string? Organization { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ServiceUsed { get; set; }
    public bool IsMiddleEast { get; set; }
}