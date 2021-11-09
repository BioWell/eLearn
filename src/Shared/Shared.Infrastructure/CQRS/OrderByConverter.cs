using System;
using System.Linq;
using AutoMapper;

namespace Shared.Infrastructure.CQRS
{
    public class OrderByConverter :
        IValueConverter<string, string[]>,
        IValueConverter<string[], string>
    {
        public string[] Convert(string? orderBy, ResolutionContext? context)
        {
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                return orderBy
                    .Split(',')
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim()).ToArray();
            }

            return Array.Empty<string>();
        }
        public string Convert(string[] orderBy, ResolutionContext? context) => orderBy?.Any() == true ? string.Join(",", orderBy) : String.Empty;
    }
}