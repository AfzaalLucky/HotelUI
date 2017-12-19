﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIHotel.Data
{
    [Table("room_category")]
    public class RoomCategory
    {
        [Key]
        [Column("id", Order = 0)]
        public long Id { get; set; }

        [Required]
        [StringLength(50)]
        [Column("category", Order = 1)]
        public string Category { get; set; }

        [StringLength(200)]
        [Column("description")]
        public string Description { get; set; }
    }
}
