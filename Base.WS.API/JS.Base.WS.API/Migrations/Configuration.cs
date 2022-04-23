namespace JS.Base.WS.API.Migrations
{
    using JS.Base.WS.API.Models;
    using JS.Base.WS.API.Models.Authorization;
    using JS.Base.WS.API.Models.Domain;
    using JS.Base.WS.API.Models.Domain.Inventory;
    using JS.Base.WS.API.Models.PersonProfile;
    using JS.Base.WS.API.Models.Publicity;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<JS.Base.WS.API.DBContext.MyDBcontext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(JS.Base.WS.API.DBContext.MyDBcontext context)
        {
            //Locator Types
            context.LocatorTypes.AddOrUpdate(
              p => p.Description,
              new LocatorType { Code = "01", Description = "Direccion" },
              new LocatorType { Code = "02", Description = "Telefono Resid" },
              new LocatorType { Code = "03", Description = "Cellular" },
              new LocatorType { Code = "04", Description = "Correo" },
              new LocatorType { Code = "05", Description = "Persona" }
              );

            //Currencis
            context.Currencies.AddOrUpdate(
              p => p.Contry,
              new Currency { Contry = "Rep. Dominicana", ISO_Currency = "Peso Dominicano", ISO_Code = "DOP", ISO_Symbol = "RD$", ISO_Number = 214, IsActive = true },
              new Currency { Contry = "Estados Unidos", ISO_Currency = "Dólar Americano ", ISO_Code = "USD", ISO_Symbol = "$", ISO_Number = 840, IsActive = true }
              );

            context.UserStatus.AddOrUpdate(
                x => x.ShortName,
                new UserStatus { ShortName = "Active", Description = "Activo", IsActive = true, ShowToCustomer = true, CreatorUserId = 1, CreationTime = DateTime.Now, Colour = "btn btn-success" },
                new UserStatus { ShortName = "Inactive", Description = "Inactivo", IsActive = true, ShowToCustomer = true, CreatorUserId = 1, CreationTime = DateTime.Now, Colour = "btn btn-danger" },
                new UserStatus { ShortName = "PendingToActive", Description = "Pendiente de activar", IsActive = true, ShowToCustomer = true, CreatorUserId = 1, CreationTime = DateTime.Now, Colour = "btn btn-warning" },
                new UserStatus { ShortName = "PendingToChangePassword", Description = "Pendiente de cambiar contraseña", IsActive = true, ShowToCustomer = false, CreatorUserId = 1, CreationTime = DateTime.Now, Colour = "btn btn-primary" }
                );

            int userStatusId = context.UserStatus.Where(x => x.ShortName == "Active").Select(x => x.Id).FirstOrDefault();

            //System users
            //context.Users.AddOrUpdate(
            //  p => p.UserName,
            //  new User { UserName = "system", Password = "1tH03LsSOvhmKWdrAIHhCPDFBwMPEkmzzS+ePUfK74g=", Name = "System", Surname = "System", PersonId = null, EmailAddress = "antoni.sirena@gmail.com", PhoneNumber = "8299093042", StatusId = userStatusId, CreationTime = DateTime.Now, CreatorUserId = 1, IsActive = true, IsDeleted = false },
            //  new User { UserName = "admin", Password = "JAvlGPq9JyTdtvBO6x2llnRI1+gxwIyPqCKAn3THIKk=", Name = "Admin", Surname = "Admin", PersonId = null, EmailAddress = "antoni.sirena@gmail.com", PhoneNumber = "8299093042", StatusId = userStatusId, CreationTime = DateTime.Now, CreatorUserId = 1, IsActive = true, IsDeleted = false },
            //  new User { UserName = "visitador", Password = "Yo5Nrsy7ye8BfPEmd/i5Pk65+VW1g7ud9FE+WBqoZ4c=", Name = "Visitador", Surname = "Visitador", IsVisitorUser = true, PersonId = null, EmailAddress = "antoni.sirena@gmail.com", PhoneNumber = "8299093042", StatusId = userStatusId, CreationTime = DateTime.Now, CreatorUserId = 1, IsActive = true, IsDeleted = false }
            //);

            long userId = context.Users.Where(x => x.UserName == "system").Select(x => x.Id).FirstOrDefault();

            context.UserTypes.AddOrUpdate(
                x => x.ShortName,
                new UserType { ShortName = "Interno", Description = "Interno", ShowToCustomer = true, IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
                new UserType { ShortName = "Externo", Description = "Externo", ShowToCustomer = true, IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now }
                );

            context.Genders.AddOrUpdate(
                x => x.ShortName,
                new Gender { ShortName = "M", Description = "Maculino", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
                new Gender { ShortName = "F", Description = "Femenino", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now }
                );

            //Document Types
            context.DocumentTypes.AddOrUpdate(
                x => x.ShortName,
                new DocumentType { ShortName = "Cédula", Description = "Cédula", ShowToCustomer = true, IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
                new DocumentType { ShortName = "Pasaporte", Description = "Pasaporte", ShowToCustomer = false, IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
                new DocumentType { ShortName = "RNC", Description = "RNC", ShowToCustomer = false, IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now }
                );


            //Payment Methods
            context.PaymentMethods.AddOrUpdate(
                x => x.ShortName,
                new PaymentMethod { ShortName = "Effective", Description = "Efectivo", ShowToCustomer = true, IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
                new PaymentMethod { ShortName = "Transference", Description = "Transferencia", ShowToCustomer = true, IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
                new PaymentMethod { ShortName = "Card", Description = "Tarjeta", ShowToCustomer = false, IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now }
                );


            context.InventoryStatuses.AddOrUpdate(
                 x => x.ShortName,
                 new InventoryStatus { ShortName = "Open", Description = "Aperturado", Colour = "btn btn-success" },
                 new InventoryStatus { ShortName = "Closed", Description = "Cerrado", Colour = "btn btn-primary" }
                 );

            //context.InventorySections.AddOrUpdate(
            //      x => x.ShortName,
            //      new InventorySection { ShortName = "00", Description = "N/A", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventorySection { ShortName = "A", Description = "A", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventorySection { ShortName = "B", Description = "B", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventorySection { ShortName = "C", Description = "C", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventorySection { ShortName = "D", Description = "D", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventorySection { ShortName = "E", Description = "E", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventorySection { ShortName = "F", Description = "F", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventorySection { ShortName = "G", Description = "G", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventorySection { ShortName = "H", Description = "H", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventorySection { ShortName = "HT", Description = "HT", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventorySection { ShortName = "I", Description = "I", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventorySection { ShortName = "Gaveta", Description = "Gaveta", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventorySection { ShortName = "P", Description = "P", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventorySection { ShortName = "Q", Description = "Q", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventorySection { ShortName = "Déposito", Description = "Déposito", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventorySection { ShortName = "Riel", Description = "Riel", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now }
            //    );


            //context.InventoryTariffs.AddOrUpdate(
            //      x => x.ShortName,
            //      new InventoryTariff { ShortName = "1", Description = "1", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "2", Description = "2", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "3", Description = "3", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "4", Description = "4", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "5", Description = "5", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "6", Description = "6", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "7", Description = "7", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "8", Description = "8", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "9", Description = "9", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "10", Description = "10", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "11", Description = "11", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "12", Description = "12", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "13", Description = "13", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "14", Description = "14", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "15", Description = "15", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "16", Description = "16", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "17", Description = "17", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "18", Description = "18", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "19", Description = "19", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "20", Description = "20", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "21", Description = "21", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "22", Description = "22", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "23", Description = "23", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "24", Description = "24", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "25", Description = "25", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "26", Description = "26", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "27", Description = "27", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "28", Description = "28", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "29", Description = "29", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "30", Description = "30", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "31", Description = "31", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "32", Description = "32", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "33", Description = "33", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "34", Description = "34", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "35", Description = "35", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "36", Description = "36", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "37", Description = "37", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "38", Description = "38", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "39", Description = "39", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "40", Description = "40", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "41", Description = "41", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "42", Description = "42", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "43", Description = "43", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "44", Description = "44", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "45", Description = "45", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "46", Description = "46", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "47", Description = "47", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "48", Description = "48", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "49", Description = "49", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "50", Description = "50", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "51", Description = "51", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "52", Description = "52", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "53", Description = "53", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "54", Description = "54", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "55", Description = "55", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "56", Description = "56", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "57", Description = "57", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "58", Description = "58", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "59", Description = "59", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "60", Description = "60", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "61", Description = "61", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "62", Description = "62", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "63", Description = "63", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "64", Description = "64", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "65", Description = "65", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "66", Description = "66", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "67", Description = "67", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "68", Description = "68", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "69", Description = "69", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "70", Description = "70", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "71", Description = "71", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "72", Description = "72", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "73", Description = "73", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "74", Description = "74", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "75", Description = "75", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "76", Description = "76", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "77", Description = "77", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "78", Description = "78", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "79", Description = "79", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
            //      new InventoryTariff { ShortName = "80", Description = "80", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now }

            //      );


            //context.InventoryConfigurations.AddOrUpdate(
            //       x => x.ShortName,
            //       new InventoryConfiguration { ShortName = "Inv-Conf", ShowCost = false, ShowPrice = false, IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now }
            //       );


            //Schedule Hours
            //context.ScheduleHours.AddOrUpdate(
            //    x => x.Description,
            //    new ScheduleHour { Description = "1:00", Value = (double)1.00, ShowToCustomer = true },
            //    new ScheduleHour { Description = "1:30", Value = (double)1.50, ShowToCustomer = true },
            //    new ScheduleHour { Description = "2:00", Value = (double)2.00, ShowToCustomer = true },
            //    new ScheduleHour { Description = "2:30", Value = (double)2.50, ShowToCustomer = true },
            //    new ScheduleHour { Description = "3:00", Value = (double)3.00, ShowToCustomer = true },
            //    new ScheduleHour { Description = "3:30", Value = (double)3.50, ShowToCustomer = true },
            //    new ScheduleHour { Description = "4:00", Value = (double)4.00, ShowToCustomer = true },
            //    new ScheduleHour { Description = "4:30", Value = (double)4.50, ShowToCustomer = true },
            //    new ScheduleHour { Description = "5:00", Value = (double)5.00, ShowToCustomer = true },
            //    new ScheduleHour { Description = "5:30", Value = (double)5.50, ShowToCustomer = true },
            //    new ScheduleHour { Description = "6:00", Value = (double)6.00, ShowToCustomer = true },
            //    new ScheduleHour { Description = "6:30", Value = (double)6.50, ShowToCustomer = true },
            //    new ScheduleHour { Description = "7:00", Value = (double)7.00, ShowToCustomer = true },
            //    new ScheduleHour { Description = "7:30", Value = (double)7.50, ShowToCustomer = true },
            //    new ScheduleHour { Description = "8:00", Value = (double)8.00, ShowToCustomer = true },
            //    new ScheduleHour { Description = "8:30", Value = (double)8.50, ShowToCustomer = true },
            //    new ScheduleHour { Description = "9:00", Value = (double)9.00, ShowToCustomer = true },
            //    new ScheduleHour { Description = "9:30", Value = (double)9.50, ShowToCustomer = true },
            //    new ScheduleHour { Description = "10:00", Value = (double)10.00, ShowToCustomer = true },
            //    new ScheduleHour { Description = "10:30", Value = (double)10.50, ShowToCustomer = true },
            //    new ScheduleHour { Description = "11:00", Value = (double)11.00, ShowToCustomer = true },
            //    new ScheduleHour { Description = "11:30", Value = (double)11.50, ShowToCustomer = true },
            //    new ScheduleHour { Description = "12:00", Value = (double)12.00, ShowToCustomer = true },
            //    new ScheduleHour { Description = "12:30", Value = (double)12.50, ShowToCustomer = true },
            //    new ScheduleHour { Description = "13:00", Value = (double)13.00, ShowToCustomer = true },
            //    new ScheduleHour { Description = "13:30", Value = (double)13.50, ShowToCustomer = true },
            //    new ScheduleHour { Description = "14:00", Value = (double)14.00, ShowToCustomer = true },
            //    new ScheduleHour { Description = "14:30", Value = (double)14.50, ShowToCustomer = true },
            //    new ScheduleHour { Description = "15:00", Value = (double)15.00, ShowToCustomer = true },
            //    new ScheduleHour { Description = "15:30", Value = (double)15.50, ShowToCustomer = true },
            //    new ScheduleHour { Description = "16:00", Value = (double)16.00, ShowToCustomer = true },
            //    new ScheduleHour { Description = "16:30", Value = (double)16.50, ShowToCustomer = true },
            //    new ScheduleHour { Description = "17:00", Value = (double)17.00, ShowToCustomer = true },
            //    new ScheduleHour { Description = "17:30", Value = (double)17.50, ShowToCustomer = true },
            //    new ScheduleHour { Description = "18:00", Value = (double)18.00, ShowToCustomer = true },
            //    new ScheduleHour { Description = "18:30", Value = (double)18.50, ShowToCustomer = true },
            //    new ScheduleHour { Description = "19:00", Value = (double)19.00, ShowToCustomer = true },
            //    new ScheduleHour { Description = "19:30", Value = (double)19.50, ShowToCustomer = true },
            //    new ScheduleHour { Description = "20:00", Value = (double)20.00, ShowToCustomer = true },
            //    new ScheduleHour { Description = "20:30", Value = (double)20.50, ShowToCustomer = true },
            //    new ScheduleHour { Description = "21:00", Value = (double)21.00, ShowToCustomer = true },
            //    new ScheduleHour { Description = "21:30", Value = (double)21.50, ShowToCustomer = true },
            //    new ScheduleHour { Description = "22:00", Value = (double)22.00, ShowToCustomer = true },
            //    new ScheduleHour { Description = "22:30", Value = (double)22.50, ShowToCustomer = true },
            //    new ScheduleHour { Description = "23:00", Value = (double)23.00, ShowToCustomer = true },
            //    new ScheduleHour { Description = "23:30", Value = (double)23.50, ShowToCustomer = true },
            //    new ScheduleHour { Description = "24:00", Value = (double)24.00, ShowToCustomer = true },
            //    new ScheduleHour { Description = "24:30", Value = (double)24.50, ShowToCustomer = true }

            //    );


            //NoveltyTypes
            context.NoveltyTypes.AddOrUpdate(
                x => x.ShortName,
                new NoveltyType { ShortName = "Sporty", Description = "Deporte" },
                new NoveltyType { ShortName = "Politics", Description = "Política" },
                new NoveltyType { ShortName = "Show", Description = "Espectáculo" },
                new NoveltyType { ShortName = "Social", Description = "Social" },
                new NoveltyType { ShortName = "Economy", Description = "Economía" },
                new NoveltyType { ShortName = "Art", Description = "Arte" },
                new NoveltyType { ShortName = "Police", Description = "Policiale" },
                new NoveltyType { ShortName = "Science", Description = "Ciencia" },
                new NoveltyType { ShortName = "Education", Description = "Educación" }
                );


        }
    }
}
