﻿using System.ComponentModel.DataAnnotations.Schema;

namespace MarinaData
{
    [Table("Lease")]
    public class Lease
    {
        public int ID { get; set; }
        public int SlipID { get; set; }
        public int CustomerID { get; set; }

        //navigation properties
        public virtual Customer? Customer { get; set; }
        public virtual Slip? Slip { get; set; }
    }
}
