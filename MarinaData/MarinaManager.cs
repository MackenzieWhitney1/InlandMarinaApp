using Microsoft.EntityFrameworkCore;

namespace MarinaData
{
    public class MarinaManager
    {
        public static List<Dock> GetDocks(InlandMarinaContext db)
        {
            List<Dock> docks = db.Docks.ToList();
            return docks;
        }

        /// <summary>
        /// Retrieve a single slip with given id
        /// </summary>
        /// <param name="db">Context object</param>
        /// <param name="movieId">Id of the movie</param>
        /// <returns>Movie object with given id or null.</returns>
        public static Slip? GetSlipById(InlandMarinaContext db, int id)
        {
                Slip? slip = db.Slips.Find(id);
                return slip;
        }

        public static List<Slip> GetUnleasedSlips(InlandMarinaContext db)
        {
            List<Slip> slips = db.Slips
                    .Include(s=> s.Dock)
                    .Include(s => s.Leases)
                    .Where(s => s.Leases.Count == 0)
                    .ToList();
            return slips;
        }

        public static List<Slip> GetUnleasedSlipsByDock(InlandMarinaContext db, int dockId)
        {
            List<Slip> slips = db.Slips
                    .Include(s => s.Dock)
                    .Include(s => s.Leases)
                    .Where(s => s.Leases.Count == 0 && s.DockID == dockId)
               .ToList();
            return slips;
        }

        public static List<Slip> GetLeasedSlipsByCustomerId(InlandMarinaContext db, int? customerId) 
        {
            List<Slip> slips = db.Slips
                .Include(s => s.Leases)
                .Include(s => s.Dock)
                .Where(s => s.Leases.Any(l => l.CustomerID == customerId))
                .ToList();
            return slips;
        }

        public static void AddLease(InlandMarinaContext db, Lease newLease)
        {
            db.Leases.Add(newLease);
            db.SaveChanges();
        }
    }
}
