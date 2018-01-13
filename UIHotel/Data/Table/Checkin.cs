﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace UIHotel.Data.Table
{
    [Table("checkin")]
    public class Checkin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id", Order = 0)]
        [StringLength(25)]
        public string Id { get; set; }

        [Required]
        [Column("id_room", Order = 1)]
        public long IdRoom { get; set; }

        [Column("id_booking", Order = 3)]
        [StringLength(25)]
        public string IdBooking { get; set; }

        [Required]
        [Column("id_guest", Order = 2)]
        public long IdGuest { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column("arrive_at", Order = 4, TypeName = "Date")]
        public DateTime ArriveAt { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column("departure_at", Order = 5, TypeName = "Date")]
        public DateTime DepartureAt { get; set; }

        [Column("count_child", Order = 6)]
        public short CountChild { get; set; }

        [Column("count_adult", Order = 7)]
        public short CountAdult { get; set; } = 1;

        [Required]
        [Column("checkin_at", Order = 8)]
        public DateTime CheckinAt { get; set; }

        [Column("checkout_at", Order = 9)]
        public DateTime CheckoutAt { get; set; }

        [Column("note", Order = 10)]
        [StringLength(255)]
        public string Note { get; set; }

        public static string GenerateID()
        {
            using (var context = new DataContext())
            {
                var CurrDate = DateTime.Now.ToString("yyyyMMdd");
                var Prefix = "CHK";
                var PrefixID = Prefix + CurrDate;
                var newId = 1;

                try
                {
                    var chk = (from a in context.CheckIn
                               where a.Id.StartsWith(PrefixID)
                               select a.Id).ToList();
                    var transform = (from a in chk
                                     select a.Replace(PrefixID, "")).Select(x => Convert.ToInt32(x)).Max();
                    newId = transform + 1;
                } catch (Exception ex)
                {

                }
                
                return PrefixID + string.Format("{0:D5}", newId);
            }
        }
    }
}
