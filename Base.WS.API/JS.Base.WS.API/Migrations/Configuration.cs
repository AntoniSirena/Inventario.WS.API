namespace JS.Base.WS.API.Migrations
{
    using JS.Base.WS.API.Models;
    using JS.Base.WS.API.Models.Authorization;
    using JS.Base.WS.API.Models.Domain;
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
              new Currency { Contry = "Estados Unidos", ISO_Currency = "D�lar Americano ", ISO_Code = "USD", ISO_Symbol = "$", ISO_Number = 840, IsActive = true }
              );

            context.UserStatus.AddOrUpdate(
                x => x.ShortName,
                new UserStatus { ShortName = "Active", Description = "Activo", IsActive = true, ShowToCustomer = true, CreatorUserId = 1, CreationTime = DateTime.Now, Colour = "btn btn-success" },
                new UserStatus { ShortName = "Inactive", Description = "Inactivo", IsActive = true, ShowToCustomer = true, CreatorUserId = 1, CreationTime = DateTime.Now, Colour = "btn btn-danger" },
                new UserStatus { ShortName = "PendingToActive", Description = "Pendiente de activar", IsActive = true, ShowToCustomer = true, CreatorUserId = 1, CreationTime = DateTime.Now, Colour = "btn btn-warning" },
                new UserStatus { ShortName = "PendingToChangePassword", Description = "Pendiente de cambiar contrase�a", IsActive = true, ShowToCustomer = false, CreatorUserId = 1, CreationTime = DateTime.Now, Colour = "btn btn-primary" }
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
                new UserType { ShortName = "Person", Description = "Cliente", ShowToCustomer = true, IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
                new UserType { ShortName = "Enterprise", Description = "Proveedor", ShowToCustomer = true, IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now }
                );

            context.Genders.AddOrUpdate(
                x => x.ShortName,
                new Gender { ShortName = "M", Description = "Maculino", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
                new Gender { ShortName = "F", Description = "Femenino", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now }
                );

            //Document Types
            context.DocumentTypes.AddOrUpdate(
                x => x.ShortName,
                new DocumentType { ShortName = "C�dula", Description = "C�dula", ShowToCustomer = true, IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
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


            //Schedule Hours
            context.ScheduleHours.AddOrUpdate(
                x => x.Description,
                new ScheduleHour { Description = "1:00", Value = (double)1.00, ShowToCustomer = true },
                new ScheduleHour { Description = "1:30", Value = (double)1.50, ShowToCustomer = true },
                new ScheduleHour { Description = "2:00", Value = (double)2.00, ShowToCustomer = true },
                new ScheduleHour { Description = "2:30", Value = (double)2.50, ShowToCustomer = true },
                new ScheduleHour { Description = "3:00", Value = (double)3.00, ShowToCustomer = true },
                new ScheduleHour { Description = "3:30", Value = (double)3.50, ShowToCustomer = true },
                new ScheduleHour { Description = "4:00", Value = (double)4.00, ShowToCustomer = true },
                new ScheduleHour { Description = "4:30", Value = (double)4.50, ShowToCustomer = true },
                new ScheduleHour { Description = "5:00", Value = (double)5.00, ShowToCustomer = true },
                new ScheduleHour { Description = "5:30", Value = (double)5.50, ShowToCustomer = true },
                new ScheduleHour { Description = "6:00", Value = (double)6.00, ShowToCustomer = true },
                new ScheduleHour { Description = "6:30", Value = (double)6.50, ShowToCustomer = true },
                new ScheduleHour { Description = "7:00", Value = (double)7.00, ShowToCustomer = true },
                new ScheduleHour { Description = "7:30", Value = (double)7.50, ShowToCustomer = true },
                new ScheduleHour { Description = "8:00", Value = (double)8.00, ShowToCustomer = true },
                new ScheduleHour { Description = "8:30", Value = (double)8.50, ShowToCustomer = true },
                new ScheduleHour { Description = "9:00", Value = (double)9.00, ShowToCustomer = true },
                new ScheduleHour { Description = "9:30", Value = (double)9.50, ShowToCustomer = true },
                new ScheduleHour { Description = "10:00", Value = (double)10.00, ShowToCustomer = true },
                new ScheduleHour { Description = "10:30", Value = (double)10.50, ShowToCustomer = true },
                new ScheduleHour { Description = "11:00", Value = (double)11.00, ShowToCustomer = true },
                new ScheduleHour { Description = "11:30", Value = (double)11.50, ShowToCustomer = true },
                new ScheduleHour { Description = "12:00", Value = (double)12.00, ShowToCustomer = true },
                new ScheduleHour { Description = "12:30", Value = (double)12.50, ShowToCustomer = true },
                new ScheduleHour { Description = "13:00", Value = (double)13.00, ShowToCustomer = true },
                new ScheduleHour { Description = "13:30", Value = (double)13.50, ShowToCustomer = true },
                new ScheduleHour { Description = "14:00", Value = (double)14.00, ShowToCustomer = true },
                new ScheduleHour { Description = "14:30", Value = (double)14.50, ShowToCustomer = true },
                new ScheduleHour { Description = "15:00", Value = (double)15.00, ShowToCustomer = true },
                new ScheduleHour { Description = "15:30", Value = (double)15.50, ShowToCustomer = true },
                new ScheduleHour { Description = "16:00", Value = (double)16.00, ShowToCustomer = true },
                new ScheduleHour { Description = "16:30", Value = (double)16.50, ShowToCustomer = true },
                new ScheduleHour { Description = "17:00", Value = (double)17.00, ShowToCustomer = true },
                new ScheduleHour { Description = "17:30", Value = (double)17.50, ShowToCustomer = true },
                new ScheduleHour { Description = "18:00", Value = (double)18.00, ShowToCustomer = true },
                new ScheduleHour { Description = "18:30", Value = (double)18.50, ShowToCustomer = true },
                new ScheduleHour { Description = "19:00", Value = (double)19.00, ShowToCustomer = true },
                new ScheduleHour { Description = "19:30", Value = (double)19.50, ShowToCustomer = true },
                new ScheduleHour { Description = "20:00", Value = (double)20.00, ShowToCustomer = true },
                new ScheduleHour { Description = "20:30", Value = (double)20.50, ShowToCustomer = true },
                new ScheduleHour { Description = "21:00", Value = (double)21.00, ShowToCustomer = true },
                new ScheduleHour { Description = "21:30", Value = (double)21.50, ShowToCustomer = true },
                new ScheduleHour { Description = "22:00", Value = (double)22.00, ShowToCustomer = true },
                new ScheduleHour { Description = "22:30", Value = (double)22.50, ShowToCustomer = true },
                new ScheduleHour { Description = "23:00", Value = (double)23.00, ShowToCustomer = true },
                new ScheduleHour { Description = "23:30", Value = (double)23.50, ShowToCustomer = true },
                new ScheduleHour { Description = "24:00", Value = (double)24.00, ShowToCustomer = true },
                new ScheduleHour { Description = "24:30", Value = (double)24.50, ShowToCustomer = true }

                );


            //NoveltyTypes
            context.NoveltyTypes.AddOrUpdate(
                x => x.ShortName,
                new NoveltyType { ShortName = "Sporty", Description = "Deporte" },
                new NoveltyType { ShortName = "Politics", Description = "Pol�tica" },
                new NoveltyType { ShortName = "Show", Description = "Espect�culo" },
                new NoveltyType { ShortName = "Social", Description = "Social" },
                new NoveltyType { ShortName = "Economy", Description = "Econom�a" },
                new NoveltyType { ShortName = "Art", Description = "Arte" },
                new NoveltyType { ShortName = "Police", Description = "Policiale" },
                new NoveltyType { ShortName = "Science", Description = "Ciencia" },
                new NoveltyType { ShortName = "Education", Description = "Educaci�n" }
                );


        }
    }
}
