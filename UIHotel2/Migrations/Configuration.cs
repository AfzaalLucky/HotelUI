namespace UIHotel2.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using UIHotel2.Data.Tables;

    internal sealed class Configuration : DbMigrationsConfiguration<Data.HotelContext>
    {
        public Configuration()
        {
            SetSqlGenerator("MySql.Data.MySqlClient", new MySql.Data.Entity.MySqlMigrationSqlGenerator());
            CodeGenerator = new MySql.Data.Entity.MySqlMigrationCodeGenerator();
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Data.HotelContext context)
        {
            //  This method will be called after migrating to the latest version.
            context.RoomStates.AddOrUpdate(x => x.StateName,
                new RoomState { StateName = "Vacant", StateColor = "32C787" },
                new RoomState { StateName = "Booked", StateColor = "FF9800" },
                new RoomState { StateName = "Occupied", StateColor = "FF5652" },
                new RoomState { StateName = "Cleaning", StateColor = "AB47BC" },
                new RoomState { StateName = "Maintance", StateColor = "757575" },
                new RoomState { StateName = "Late Checkout", StateColor = "2196F3" });
            context.RoomCategories.AddOrUpdate(x => x.CategoryName,
                new RoomCategory { CategoryName = "Big" },
                new RoomCategory { CategoryName = "Medium" },
                new RoomCategory { CategoryName = "Small" });
            context.RoomPriceKinds.AddOrUpdate(x => x.KindName,
                new RoomPriceKind { KindName = "WeekDay", KindColor = "43A047" },
                new RoomPriceKind { KindName = "WeekEnd", KindColor = "D32F2F" },
                new RoomPriceKind { KindName = "Holiday", KindColor = "00695C" });
            context.Settings.AddOrUpdate(x => x.Key,
                new Setting { Key = "app.name", Value = "Hotel Management System" },
                new Setting { Key = "app.key", Value = "" },
                new Setting { Key = "hotel.name", Value = "Hotel Test" },
                new Setting { Key = "hotel.address", Value = "" },
                new Setting { Key = "hotel.logo", Value = "" },
                new Setting { Key = "time.checkin", Value = "00:00:00" },
                new Setting { Key = "time.checkout", Value = "00:00:00" },
                new Setting { Key = "time.fullcharge", Value = "00:00:00" },
                new Setting { Key = "penalty", Value = "20000" },
                new Setting { Key = "deposit", Value = "50000" });
            context.SaveChanges();

            var big = context.RoomCategories.Where(x => x.CategoryName == "Big").Single();
            var vacant = context.RoomStates.Where(x => x.StateName == "Vacant").Single();
            var weekday = context.RoomPriceKinds.Where(x => x.KindName == "WeekDay").Single();

            context.RoomCalendars.AddOrUpdate(x => x.DateAt,
                new RoomCalendar { DateAt = DateTime.Now, Kind = weekday });

            context.Rooms.RemoveRange(context.Rooms.ToList());
            context.SaveChanges();
            context.Rooms.AddOrUpdate(x => x.RoomNumber,
                new Room { RoomNumber = "201", State = vacant, Category = big });
            context.SaveChanges();
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
