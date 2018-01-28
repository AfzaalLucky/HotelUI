﻿using System;
using System.Collections.Generic;
using System.Linq;
using UIHotel.Data.Table;

namespace UIHotel.Data.Seeds
{
    public class InvoiceSeeder : DBSeeder
    {
        public override void Run(DataContext context)
        {
            context.Invoices.Add(new Invoice()
            {
                Id = "TEST_INVOICE",
                IdCheckin = "TEST_INVOICE",
                IdGuest = 123,
                CreateAt = DateTime.Now
            });

            context.InvoiceDetails.Add(new InvoiceDetail()
            {
                IdInvoice = "TEST_INVOICE",
                TransactionDate = DateTime.Now,
                AmmountIn = 100000,
                Description = "Deposito",
                CreateAt = DateTime.Now
            });

            context.InvoiceDetails.Add(new InvoiceDetail()
            {
                IdInvoice = "TEST_INVOICE",
                TransactionDate = DateTime.Now,
                AmmountOut = 20000,
                Description = "Room Invoice",
                CreateAt = DateTime.Now
            });

            context.SaveChanges();
        }
    }
}
