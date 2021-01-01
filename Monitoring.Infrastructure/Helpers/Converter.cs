using System;
using System.ComponentModel.DataAnnotations;

namespace Monitoring.Infrastructure.Helpers
{
    public static class Converter
    {
        public static Guid StringToGuid(this string str)
        => Guid.TryParse(str, out Guid result) ? result : throw new ValidationException("Invalid guid string");

    }
}