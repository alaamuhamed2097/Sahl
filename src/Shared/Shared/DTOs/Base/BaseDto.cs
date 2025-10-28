using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.DTOs.Base
{
    public class BaseDto
    {
        public Guid Id { get; set; }
    }
}
