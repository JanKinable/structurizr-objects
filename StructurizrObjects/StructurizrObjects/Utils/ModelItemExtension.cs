using System.Collections.Generic;
using System.Linq;
using Structurizr;

namespace StructurizrObjects.Utils
{
    public static class ModelItemExtension
    {
        public static void AddUniqueTag(this ModelItem model, string tag)
        {
            var tags = model.Tags.Split(',').Distinct().ToList();
            if (! tags.Exists(x => x == tag))
            {
                tags.Add(tag);
            }

            model.Tags = string.Join(",", tags);
        }

        public static void AddUniqueTags(this ModelItem model, IEnumerable<string> tags)
        {
            var currentTags = model.Tags.Split(',').Distinct().ToList();
            var mergedList = currentTags.Concat(tags);
            model.Tags = string.Join(",", mergedList.Distinct());
        }
    }
}