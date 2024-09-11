using System.ComponentModel.DataAnnotations.Schema;

namespace MarinaData
{
    [Table("Slip")]
    public class Slip
    {
        public int ID { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }
        public int DockID { get; set; }

        // navigation properties
        public virtual Dock Dock { get; set; }
        public virtual ICollection<Lease> Leases { get; set; }
    }

}
