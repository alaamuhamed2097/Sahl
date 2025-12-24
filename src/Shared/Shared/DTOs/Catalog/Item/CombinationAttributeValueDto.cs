using Resources;
using Shared.DTOs.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DTOs.Catalog.Item
{
    public class CombinationAttributeValueDto : BaseDto
    {
        public Guid AttributeId { get; set; }
        public string Value { get; set; }
    }
}