using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MarinaData
{
    [Table("Customer")]
    public class Customer
    {
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string Username { get; set; }

        [Required]
        [StringLength(30)]
        public string Password { get; set; }

        [Required]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        public string LastName { get; set; }

        [Required]
        [StringLength(15)]
        [RegularExpression(@"^\d{3}-\d{3}-\d{4}$",
         ErrorMessage = "Phone numbers must be in the XXX-XXX-XXXX format")]
        public string Phone { get; set; }

        [Required]
        [StringLength(30)]
        public string City { get; set; }

        // navigation property
        public virtual ICollection<Lease> Leases { get; set; }
    }

}
