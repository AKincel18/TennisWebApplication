using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TennisApplication.Others
{
    public class DetachLocal
    {
        public static void Detach<T>(DbContext context, T t, int entryId)
            where T : class, IIdentifier
        {
            var local = context.Set<T>()
                .Local
                .FirstOrDefault(entry => entry.Id.Equals(entryId));

            if (local != null && !local.Equals(null))
            {
                context.Entry(local).State = EntityState.Detached;
            }

            context.Entry(t).State = EntityState.Modified;
        }
    }

    public interface IIdentifier
    {
        int Id { get; }
    }
}