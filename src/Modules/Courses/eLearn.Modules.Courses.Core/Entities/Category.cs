using System;
using Shared.Infrastructure.Entities;

namespace eLearn.Modules.Courses.Core.Entities
{
    internal class Category : EntityBase
    {
        public string Name { get; set; } = String.Empty;

        public string ImageUrl { get; set; } = String.Empty;

        public string Detail { get; set; } = String.Empty;
    }
}