﻿using MySql.Data.MySqlClient;
using UIHotel.App.Provider;

namespace UIHotel.Data.Seeds
{
    public abstract class DBSeeder
    {
        public abstract void Run(DataContext context);

        public static void Seed()
        {
            using (var connection = new MySqlConnection(SettingProvider.SQL_Connection_Str))
            using (var context = new DataContext(connection, false))
            {
                try
                {
                    new SettingSeeder().Run(context);
                    new RoomSeeder().Run(context);
                    new RoomStatusSeeder().Run(context);
                    new InvoiceSeeder().Run(context);
                    new RoomPriceSeeder().Run(context);
                    new GuestSeeder().Run(context);
                    new LedgerSeeder().Run(context);

                    context.SaveChanges();
                } catch
                {
                    //
                }
            }
        }
    }
}
