namespace JS.Base.WS.API.Migrations
{
    using JS.Base.WS.API.Models;
    using JS.Base.WS.API.Models.Authorization;
    using JS.Base.WS.API.Models.Domain;
    using JS.Base.WS.API.Models.Domain.PurchaseTransaction;
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


            //Appointment Statuses
            context.AppointmentStatuses.AddOrUpdate(
                x => x.ShortName,
                new AppointmentStatus { ShortName = "Pending", Description = "Pendiente", ShowToCustomer = true, Colour = "btn btn-info" },
                new AppointmentStatus { ShortName = "OnHold", Description = "En espera", ShowToCustomer = true, Colour = "btn btn-warning" },
                new AppointmentStatus { ShortName = "InProcess", Description = "En proceso", ShowToCustomer = true, Colour = "btn btn-success" },
                new AppointmentStatus { ShortName = "Finished", Description = "Finalizada", ShowToCustomer = true, Colour = "btn btn-primary" },
                new AppointmentStatus { ShortName = "Cancelled", Description = "Cancelada", ShowToCustomer = true, Colour = "btn btn-danger" }
                );


            //Purchase Transaction Statuses
            context.PurchaseTransactionStatus.AddOrUpdate(
                x => x.ShortName,
                new PurchaseTransactionStatus { ShortName = "PendingToDelivery", Description = "Pendiente de entraga", ShowToCustomer = true, Colour = "btn btn-info", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
                new PurchaseTransactionStatus { ShortName = "Delivered", Description = "En espera", ShowToCustomer = true, Colour = "btn btn-info", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
                new PurchaseTransactionStatus { ShortName = "InProcess", Description = "En proceso", ShowToCustomer = true, Colour = "btn btn-success", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
                new PurchaseTransactionStatus { ShortName = "PendingToPay", Description = "Pendiente de pago", ShowToCustomer = true, Colour = "btn btn-primary", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
                new PurchaseTransactionStatus { ShortName = "Payed", Description = "Pagada", ShowToCustomer = true, Colour = "btn btn-primary", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
                new PurchaseTransactionStatus { ShortName = "Cancelled", Description = "Cancelada", ShowToCustomer = true, Colour = "btn btn-danger", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now }
                );

            //Purchase Transaction Types
            context.PurchaseTransactionTypes.AddOrUpdate(
                x => x.ShortName,
                new PurchaseTransactionType { ShortName = "Request", Description = "Solicitud", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
                new PurchaseTransactionType { ShortName = "Order", Description = "Orden", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now }
                );

            //Article conditions
            context.ArticleConditions.AddOrUpdate(
                x => x.ShortName,
                new ArticleCondition { ShortName = "New", Description = "Nuevo", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
                new ArticleCondition { ShortName = "Used", Description = "Usado", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
                new ArticleCondition { ShortName = "NA", Description = "No aplica", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now }
                );


            //Product Types
            context.ProductTypes.AddOrUpdate(
                x => x.ShortName,
                new ProductType { ShortName = "Product", Description = "Producto", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now },
                new ProductType { ShortName = "Service", Description = "Servicio", IsActive = true, CreatorUserId = userId, CreationTime = DateTime.Now }
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

            //Comapy Categories
            context.CompanyCategories.AddOrUpdate(
                x => x.ShortName,
                new CompanyCategory { ShortName = "Food", Description = "Comida", IsActive = true, CreationTime = DateTime.Now, CreatorUserId = userId },
                new CompanyCategory { ShortName = "ProvisionsMerchant", Description = "Comerciante y Detallista de Provisione", IsActive = true, CreationTime = DateTime.Now, CreatorUserId = userId },
                new CompanyCategory { ShortName = "Technology", Description = "Tecnolog�a: (Computadora, Sistema, Seguridad y Celular)", IsActive = true, CreationTime = DateTime.Now, CreatorUserId = userId },
                new CompanyCategory { ShortName = "Pharmacy", Description = "Farmacia", IsActive = true, CreationTime = DateTime.Now, CreatorUserId = userId },
                new CompanyCategory { ShortName = "HardwareStoreReplacement", Description = "Ferreter�a y Repuesto", IsActive = true, CreationTime = DateTime.Now, CreatorUserId = userId },
                new CompanyCategory { ShortName = "BeautyStyle", Description = "Salon, U�a y Barber�", IsActive = true, CreationTime = DateTime.Now, CreatorUserId = userId },
                new CompanyCategory { ShortName = "Workshops", Description = "Taller: (Mec�nica, Soldadura & Herrer�a, Puerta & Ventana y Tapicer�a)", IsActive = true, CreationTime = DateTime.Now, CreatorUserId = userId },
                new CompanyCategory { ShortName = "Driver", Description = "Taxi y Delivery", IsActive = true, CreationTime = DateTime.Now, CreatorUserId = userId },
                new CompanyCategory { ShortName = "Auto", Description = "RentCar y Dealer", IsActive = true, CreationTime = DateTime.Now, CreatorUserId = userId },
                new CompanyCategory { ShortName = "CivilEngineer", Description = "Ingenier�a Civil: (Agrimensura, Construcci�n, Electricidad y Plomer�a)", IsActive = true, CreationTime = DateTime.Now, CreatorUserId = userId },
                new CompanyCategory { ShortName = "HotelCottage", Description = "Hotel y Casa de campo", IsActive = true, CreationTime = DateTime.Now, CreatorUserId = userId },
                new CompanyCategory { ShortName = "Entertainment", Description = "Entretenimiento: (Club Night, Billar, Drink y Piscina)", IsActive = true, CreationTime = DateTime.Now, CreatorUserId = userId },
                new CompanyCategory { ShortName = "Lawyer", Description = "Abogado", IsActive = true, CreationTime = DateTime.Now, CreatorUserId = userId },
                new CompanyCategory { ShortName = "Store", Description = "Tienda", IsActive = true, CreationTime = DateTime.Now, CreatorUserId = userId },
                new CompanyCategory { ShortName = "Accounting", Description = "Contabilidad", IsActive = true, CreationTime = DateTime.Now, CreatorUserId = userId },
                new CompanyCategory { ShortName = "GraphicDesign", Description = "Dise�o: (Gr�fico, de Moda y m�s)", IsActive = true, CreationTime = DateTime.Now, CreatorUserId = userId },
                new CompanyCategory { ShortName = "Nursery", Description = "Vivero", IsActive = true, CreationTime = DateTime.Now, CreatorUserId = userId },
                new CompanyCategory { ShortName = "Health", Description = "Salud: (Clinicas y Hospitales)", IsActive = true, CreationTime = DateTime.Now, CreatorUserId = userId }

                );


            //Market types
            context.MarketTypes.AddOrUpdate(
                x => x.ShortName,
                new MarketType { ShortName = "Sell", Description = "Vender", IsActive = true },
                new MarketType { ShortName = "Rent", Description = "Rentar", IsActive = true },
                new MarketType { ShortName = "Donate", Description = "Donar", IsActive = true }
                );


            //Article categories
            //context.ArticleCategories.AddOrUpdate(
            //    x => x.ShortName,
            //    new ArticleCategory { ShortName = "Accesorios para Veh�culos", Description = "Accesorios para Veh�culos", IsActive = true },
            //    new ArticleCategory { ShortName = "Agro", Description = "Agro", IsActive = true },
            //    new ArticleCategory { ShortName = "Arte y Antig�edades", Description = "Arte y Antig�edades", IsActive = true },
            //    new ArticleCategory { ShortName = "Carros, Motos y Otros", Description = "Carros, Motos y Otros", IsActive = true },
            //    new ArticleCategory { ShortName = "Celulares y Tel�fonos", Description = "Celulares y Tel�fonos", IsActive = true },
            //    new ArticleCategory { ShortName = "Coleccionables y Hobbies", Description = "Coleccionables y Hobbies", IsActive = true },
            //    new ArticleCategory { ShortName = "Computaci�n", Description = "Computaci�n", IsActive = true },
            //    new ArticleCategory { ShortName = "Consolas y Videojuegos", Description = "Consolas y Videojuegos", IsActive = true },
            //    new ArticleCategory { ShortName = "C�maras Digitales y Foto", Description = "C�maras Digitales y Foto", IsActive = true },
            //    new ArticleCategory { ShortName = "Deportes y Fitness", Description = "Deportes y Fitness", IsActive = true },
            //    new ArticleCategory { ShortName = "Electr�nica Audio Video", Description = "Electr�nica Audio Video", IsActive = true },
            //    new ArticleCategory { ShortName = "Hogar y Electrodom�sticos", Description = "Hogar y Electrodom�sticos", IsActive = true },
            //    new ArticleCategory { ShortName = "Inmuebles", Description = "Inmuebles", IsActive = true },
            //    new ArticleCategory { ShortName = "Instrumentos Musicales", Description = "Instrumentos Musicales", IsActive = true },
            //    new ArticleCategory { ShortName = "Juegos y Juguetes", Description = "Juegos y Juguetes", IsActive = true },
            //    new ArticleCategory { ShortName = "Libros, Revistas y Comics", Description = "Libros, Revistas y Comics", IsActive = true },
            //    new ArticleCategory { ShortName = "M�sica y Pel�culas", Description = "M�sica y Pel�culas", IsActive = true },
            //    new ArticleCategory { ShortName = "Ropa, Relojes y Lentes", Description = "Ropa, Relojes y Lentes", IsActive = true },
            //    new ArticleCategory { ShortName = "Servicios", Description = "Servicios", IsActive = true },
            //    new ArticleCategory { ShortName = "Otras categor�as", Description = "Otras categor�as", IsActive = true },
            //    new ArticleCategory { ShortName = "Carnes", Description = "Carnes", IsActive = true }
            //    );


            ////Article sub categories
            //context.ArticleSubCategories.AddOrUpdate(
            // x => x.ShortName,
            //new ArticleSubCategory { ShortName = "Acc.para Motos y Cuatrimotos", Description = "Acc.para Motos y Cuatrimotos", CategoryId = 1, IsActive = true },
            //new ArticleSubCategory { ShortName = "Acc.y Repuestos N�uticos", Description = "Acc.y Repuestos N�uticos", CategoryId = 1, IsActive = true },
            //new ArticleSubCategory { ShortName = "Accesorios de Auto y Camioneta", Description = "Accesorios de Auto y Camioneta", CategoryId = 1, IsActive = true },
            //new ArticleSubCategory { ShortName = "Accesorios para Camiones", Description = "Accesorios para Camiones", CategoryId = 1, IsActive = true },
            //new ArticleSubCategory { ShortName = "Aros y Tazas", Description = "Aros y Tazas", CategoryId = 1, IsActive = true },
            //new ArticleSubCategory { ShortName = "Audio para Veh�culos", Description = "Audio para Veh�culos", CategoryId = 1, IsActive = true },
            //new ArticleSubCategory { ShortName = "GNC", Description = "GNC", CategoryId = 1, IsActive = true },
            //new ArticleSubCategory { ShortName = "Gomas", Description = "Gomas", CategoryId = 1, IsActive = true },
            //new ArticleSubCategory { ShortName = "Herramientas", Description = "Herramientas", CategoryId = 1, IsActive = true },
            //new ArticleSubCategory { ShortName = "Limpieza de Veh�culos", Description = "Limpieza de Veh�culos", CategoryId = 1, IsActive = true },
            //new ArticleSubCategory { ShortName = "Navegadores GPS para Veh�culos", Description = "Navegadores GPS para Veh�culos", CategoryId = 1, IsActive = true },
            //new ArticleSubCategory { ShortName = "Performance", Description = "Performance", CategoryId = 1, IsActive = true },
            //new ArticleSubCategory { ShortName = "Repuestos Autos y Camionetas", Description = "Repuestos Autos y Camionetas", CategoryId = 1, IsActive = true },
            //new ArticleSubCategory { ShortName = "Repuestos de L�nea Pesada", Description = "Repuestos de L�nea Pesada", CategoryId = 1, IsActive = true },
            //new ArticleSubCategory { ShortName = "Repuestos para Motos", Description = "Repuestos para Motos", CategoryId = 1, IsActive = true },
            //new ArticleSubCategory { ShortName = "Seguridad Vehicular", Description = "Seguridad Vehicular", CategoryId = 1, IsActive = true },
            //new ArticleSubCategory { ShortName = "Servicio Programado", Description = "Servicio Programado", CategoryId = 1, IsActive = true },
            //new ArticleSubCategory { ShortName = "Tuning", Description = "Tuning", CategoryId = 1, IsActive = true },
            //new ArticleSubCategory { ShortName = "Otros", Description = "Otros", CategoryId = 1, IsActive = true },

            //new ArticleSubCategory { ShortName = "Animales", Description = "Animales", CategoryId = 2, IsActive = true },
            //new ArticleSubCategory { ShortName = "Generadores de Energ�a", Description = "Generadores de Energ�a", CategoryId = 2, IsActive = true },
            //new ArticleSubCategory { ShortName = "Infraestructura Rural", Description = "Infraestructura Rural", CategoryId = 2, IsActive = true },
            //new ArticleSubCategory { ShortName = "Insumos Agr�colas", Description = "Insumos Agr�colas", CategoryId = 2, IsActive = true },
            //new ArticleSubCategory { ShortName = "Insumos Ganaderos", Description = "Insumos Ganaderos", CategoryId = 2, IsActive = true },
            //new ArticleSubCategory { ShortName = "M�quinas", Description = "M�quinas", CategoryId = 2, IsActive = true },
            //new ArticleSubCategory { ShortName = "Repuestos Maquinaria Agr�cola", Description = "Repuestos Maquinaria Agr�cola", CategoryId = 2, IsActive = true },
            //new ArticleSubCategory { ShortName = "Otros", Description = "Otros", CategoryId = 2, IsActive = true },

            //new ArticleSubCategory { ShortName = "Antig�edades y Colecciones", Description = "Antig�edades y Colecciones", CategoryId = 3, IsActive = true },
            //new ArticleSubCategory { ShortName = "Arte, Librer�a y Mercer�a", Description = "Arte, Librer�a y Mercer�a", CategoryId = 3, IsActive = true },
            //new ArticleSubCategory { ShortName = "Artesan�as", Description = "Artesan�as", CategoryId = 3, IsActive = true },

            //new ArticleSubCategory { ShortName = "Autos de Colecci�n", Description = "Autos de Colecci�n", CategoryId = 4, IsActive = true },
            //new ArticleSubCategory { ShortName = "Autos y Camionetas", Description = "Autos y Camionetas", CategoryId = 4, IsActive = true },
            //new ArticleSubCategory { ShortName = "Camiones", Description = "Camiones", CategoryId = 4, IsActive = true },
            //new ArticleSubCategory { ShortName = "Colectivos", Description = "Colectivos", CategoryId = 4, IsActive = true },
            //new ArticleSubCategory { ShortName = "Motos", Description = "Motos", CategoryId = 4, IsActive = true },
            //new ArticleSubCategory { ShortName = "N�utica", Description = "N�utica", CategoryId = 4, IsActive = true },
            //new ArticleSubCategory { ShortName = "Tractores", Description = "Tractores", CategoryId = 4, IsActive = true },
            //new ArticleSubCategory { ShortName = "Otros Veh�culos", Description = "Otros Veh�culos", CategoryId = 4, IsActive = true },
            //new ArticleSubCategory { ShortName = "Bicicletas", Description = "Bicicletas", CategoryId = 4, IsActive = true },

            //new ArticleSubCategory { ShortName = "Accesorios para Celulares", Description = "Accesorios para Celulares", CategoryId = 5, IsActive = true },
            //new ArticleSubCategory { ShortName = "Celulares y Smartphones", Description = "Celulares y Smartphones", CategoryId = 5, IsActive = true },
            //new ArticleSubCategory { ShortName = "Lentes de Realidad Virtual", Description = "Lentes de Realidad Virtual", CategoryId = 5, IsActive = true },
            //new ArticleSubCategory { ShortName = "Radiofrecuencia", Description = "Radiofrecuencia", CategoryId = 5, IsActive = true },
            //new ArticleSubCategory { ShortName = "Repuestos para Celulares", Description = "Repuestos para Celulares", CategoryId = 5, IsActive = true },
            //new ArticleSubCategory { ShortName = "Smartwatches y Accesorios", Description = "Smartwatches y Accesorios", CategoryId = 5, IsActive = true },
            //new ArticleSubCategory { ShortName = "Tarifadores y Cabinas", Description = "Tarifadores y Cabinas", CategoryId = 5, IsActive = true },
            //new ArticleSubCategory { ShortName = "Telefon�a IP", Description = "Telefon�a IP", CategoryId = 5, IsActive = true },
            //new ArticleSubCategory { ShortName = "Tel�fonos", Description = "Tel�fonos", CategoryId = 5, IsActive = true },
            //new ArticleSubCategory { ShortName = "Otros", Description = "Otros", CategoryId = 5, IsActive = true },


            //new ArticleSubCategory { ShortName = "Otros", Description = "Otros", CategoryId = 6, IsActive = true },

            //new ArticleSubCategory { ShortName = "Accesorios de Antiest�tica", Description = "Accesorios de Antiest�tica", CategoryId = 7, IsActive = true },
            //new ArticleSubCategory { ShortName = "Accesorios para PC Gaming", Description = "Accesorios para PC Gaming", CategoryId = 7, IsActive = true },
            //new ArticleSubCategory { ShortName = "Almacenamiento", Description = "Almacenamiento", CategoryId = 7, IsActive = true },
            //new ArticleSubCategory { ShortName = "Cables y Hubs USB", Description = "Cables y Hubs USB", CategoryId = 7, IsActive = true },
            //new ArticleSubCategory { ShortName = "Cajas, Sobres y Porta CDs", Description = "Cajas, Sobres y Porta CDs", CategoryId = 7, IsActive = true },
            //new ArticleSubCategory { ShortName = "Componentes de PC", Description = "Componentes de PC", CategoryId = 7, IsActive = true },
            //new ArticleSubCategory { ShortName = "Conectividad y Redes", Description = "Conectividad y Redes", CategoryId = 7, IsActive = true },
            //new ArticleSubCategory { ShortName = "Estabilizadores y UPS", Description = "Estabilizadores y UPS", CategoryId = 7, IsActive = true },
            //new ArticleSubCategory { ShortName = "Impresi�n", Description = "Impresi�n", CategoryId = 7, IsActive = true },
            //new ArticleSubCategory { ShortName = "Laptops y Accesorios", Description = "Laptops y Accesorios", CategoryId = 7, IsActive = true },
            //new ArticleSubCategory { ShortName = "Lectores y Scanners", Description = "Lectores y Scanners", CategoryId = 7, IsActive = true },
            //new ArticleSubCategory { ShortName = "Limpieza y Cuidado de PCs", Description = "Limpieza y Cuidado de PCs", CategoryId = 7, IsActive = true },
            //new ArticleSubCategory { ShortName = "Monitores y Accesorios", Description = "Monitores y Accesorios", CategoryId = 7, IsActive = true },
            //new ArticleSubCategory { ShortName = "Palms y Handhelds", Description = "Palms y Handhelds", CategoryId = 7, IsActive = true },
            //new ArticleSubCategory { ShortName = "PC de Escritorio", Description = "PC de Escritorio", CategoryId = 7, IsActive = true },
            //new ArticleSubCategory { ShortName = "Perif�ricos de PC", Description = "Perif�ricos de PC", CategoryId = 7, IsActive = true },
            //new ArticleSubCategory { ShortName = "Proyectores y Pantallas", Description = "Proyectores y Pantallas", CategoryId = 7, IsActive = true },
            //new ArticleSubCategory { ShortName = "Software", Description = "Software", CategoryId = 7, IsActive = true },
            //new ArticleSubCategory { ShortName = "Tablets y Accesorios", Description = "Tablets y Accesorios", CategoryId = 7, IsActive = true },
            //new ArticleSubCategory { ShortName = "Otros", Description = "Otros", CategoryId = 7, IsActive = true },

            //new ArticleSubCategory { ShortName = "Accesorios para Consolas", Description = "Accesorios para Consolas", CategoryId = 8, IsActive = true },
            //new ArticleSubCategory { ShortName = "Accesorios para PC Gaming", Description = "Accesorios para PC Gaming", CategoryId = 8, IsActive = true },
            //new ArticleSubCategory { ShortName = "Consolas", Description = "Consolas", CategoryId = 8, IsActive = true },
            //new ArticleSubCategory { ShortName = "Flippers y Arcade", Description = "Flippers y Arcade", CategoryId = 8, IsActive = true },
            //new ArticleSubCategory { ShortName = "Repuestos para Consolas", Description = "Repuestos para Consolas", CategoryId = 8, IsActive = true },
            //new ArticleSubCategory { ShortName = "Videojuegos", Description = "Videojuegos", CategoryId = 8, IsActive = true },
            //new ArticleSubCategory { ShortName = "Otros", Description = "Otros", CategoryId = 8, IsActive = true },

            //new ArticleSubCategory { ShortName = "Accesorios para C�maras", Description = "Accesorios para C�maras", CategoryId = 9, IsActive = true },
            //new ArticleSubCategory { ShortName = "�lbumes y Portarretratos", Description = "�lbumes y Portarretratos", CategoryId = 9, IsActive = true },
            //new ArticleSubCategory { ShortName = "Cables", Description = "Cables", CategoryId = 9, IsActive = true },
            //new ArticleSubCategory { ShortName = "C�maras", Description = "C�maras", CategoryId = 9, IsActive = true },
            //new ArticleSubCategory { ShortName = "C�maras de Video", Description = "C�maras de Video", CategoryId = 9, IsActive = true },
            //new ArticleSubCategory { ShortName = "Drones y Accesorios", Description = "Drones y Accesorios", CategoryId = 9, IsActive = true },
            //new ArticleSubCategory { ShortName = "Lentes y Filtros", Description = "Lentes y Filtros", CategoryId = 9, IsActive = true },
            //new ArticleSubCategory { ShortName = "Repuestos para C�maras", Description = "Repuestos para C�maras", CategoryId = 9, IsActive = true },
            //new ArticleSubCategory { ShortName = "Revelado y Laboratorio", Description = "Revelado y Laboratorio", CategoryId = 9, IsActive = true },
            //new ArticleSubCategory { ShortName = "Telescopios y Binoculares", Description = "Telescopios y Binoculares", CategoryId = 9, IsActive = true },
            //new ArticleSubCategory { ShortName = "Otros", Description = "Otros", CategoryId = 9, IsActive = true },


            //new ArticleSubCategory { ShortName = "Aerobics y Fitness", Description = "Aerobics y Fitness", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Artes Marciales y Boxeo", Description = "Artes Marciales y Boxeo", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "B�dminton", Description = "B�dminton", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Basketball", Description = "Basketball", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Buceo", Description = "Buceo", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Camping y Pesca", Description = "Camping y Pesca", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Canoas, Kayaks e Inflables", Description = "Canoas, Kayaks e Inflables", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Ciclismo", Description = "Ciclismo", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Equitaci�n y Polo", Description = "Equitaci�n y Polo", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Esgrima", Description = "Esgrima", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Esqui y Snowboard", Description = "Esqui y Snowboard", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "F�tbol", Description = "F�tbol", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "F�tbol Americano", Description = "F�tbol Americano", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Golf", Description = "Golf", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Handball", Description = "Handball", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Hockey", Description = "Hockey", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Juegos de Sal�n", Description = "Juegos de Sal�n", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Kitesurf", Description = "Kitesurf", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Monopatines y Scooters", Description = "Monopatines y Scooters", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Monta�ismo y Trekking", Description = "Monta�ismo y Trekking", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Nataci�n", Description = "Nataci�n", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Paintball", Description = "Paintball", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Parapente", Description = "Parapente", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Pat�n, Gimnasia y Danza", Description = "Pat�n, Gimnasia y Danza", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Pilates y Yoga", Description = "Pilates y Yoga", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Puls�metros y Cron�metros", Description = "Puls�metros y Cron�metros", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Ropa Deportiva", Description = "Ropa Deportiva", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Rugby", Description = "Rugby", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Skateboard y Sandboard", Description = "Skateboard y Sandboard", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Slackline", Description = "Slackline", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Softball y Beisbol", Description = "Softball y Beisbol", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Suplementos y Shakers", Description = "Suplementos y Shakers", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Surf y Bodyboard", Description = "Surf y Bodyboard", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Tenis, Paddle y Squash", Description = "Tenis, Paddle y Squash", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Tiro Deportivo", Description = "Tiro Deportivo", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Voley", Description = "Voley", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Wakeboard y Esqui Acu�tico", Description = "Wakeboard y Esqui Acu�tico", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Windsurf", Description = "Windsurf", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Zapatillas", Description = "Zapatillas", CategoryId = 10, IsActive = true },
            //new ArticleSubCategory { ShortName = "Otros", Description = "Otros", CategoryId = 10, IsActive = true },

            //new ArticleSubCategory { ShortName = "Accesorios para Audio y Video", Description = "Accesorios para Audio y Video", CategoryId = 11, IsActive = true },
            //new ArticleSubCategory { ShortName = "Accesorios para TV", Description = "Accesorios para TV", CategoryId = 11, IsActive = true },
            //new ArticleSubCategory { ShortName = "Audio", Description = "Audio", CategoryId = 11, IsActive = true },
            //new ArticleSubCategory { ShortName = "Cables", Description = "Cables", CategoryId = 11, IsActive = true },
            //new ArticleSubCategory { ShortName = "Componentes Electr�nicos", Description = "Componentes Electr�nicos", CategoryId = 11, IsActive = true },
            //new ArticleSubCategory { ShortName = "Controles Remotos", Description = "Controles Remotos", CategoryId = 11, IsActive = true },
            //new ArticleSubCategory { ShortName = "Drones y Accesorios", Description = "Drones y Accesorios", CategoryId = 11, IsActive = true },
            //new ArticleSubCategory { ShortName = "Fundas y Bolsos", Description = "Fundas y Bolsos", CategoryId = 11, IsActive = true },
            //new ArticleSubCategory { ShortName = "Media Streaming", Description = "Media Streaming", CategoryId = 11, IsActive = true },
            //new ArticleSubCategory { ShortName = "Pilas y Cargadores", Description = "Pilas y Cargadores", CategoryId = 11, IsActive = true },
            //new ArticleSubCategory { ShortName = "Proyectores y Pantallas", Description = "Proyectores y Pantallas", CategoryId = 11, IsActive = true },
            //new ArticleSubCategory { ShortName = "Repuestos para TV", Description = "Repuestos para TV", CategoryId = 11, IsActive = true },
            //new ArticleSubCategory { ShortName = "Televisores", Description = "Televisores", CategoryId = 11, IsActive = true },
            //new ArticleSubCategory { ShortName = "Video", Description = "Video", CategoryId = 11, IsActive = true },
            //new ArticleSubCategory { ShortName = "Otros", Description = "Otros", CategoryId = 11, IsActive = true },

            //new ArticleSubCategory { ShortName = "Electrodom�sticos", Description = "Electrodom�sticos", CategoryId = 12, IsActive = true },
            //new ArticleSubCategory { ShortName = "Hogar, Muebles y Jard�n", Description = "Hogar, Muebles y Jard�n", CategoryId = 12, IsActive = true },

            //new ArticleSubCategory { ShortName = "Apartamentos", Description = "Apartamentos", CategoryId = 13, IsActive = true },
            //new ArticleSubCategory { ShortName = "Casas", Description = "Casas", CategoryId = 13, IsActive = true },
            //new ArticleSubCategory { ShortName = "Fincas", Description = "Fincas", CategoryId = 13, IsActive = true },
            //new ArticleSubCategory { ShortName = "Locales - Tiendas", Description = "Locales - Tiendas", CategoryId = 13, IsActive = true },
            //new ArticleSubCategory { ShortName = "Oficinas", Description = "Oficinas", CategoryId = 13, IsActive = true },
            //new ArticleSubCategory { ShortName = "Solares", Description = "Solares", CategoryId = 13, IsActive = true },
            //new ArticleSubCategory { ShortName = "Terrenos", Description = "Terrenos", CategoryId = 13, IsActive = true },
            //new ArticleSubCategory { ShortName = "Otras Propiedades", Description = "Otras Propiedades", CategoryId = 13, IsActive = true },

            //new ArticleSubCategory { ShortName = "Bater�as y Percusi�n", Description = "Bater�as y Percusi�n", CategoryId = 14, IsActive = true },
            //new ArticleSubCategory { ShortName = "Equipos de DJ y Accesorios", Description = "Equipos de DJ y Accesorios", CategoryId = 14, IsActive = true },
            //new ArticleSubCategory { ShortName = "Estudio de Grabaci�n", Description = "Estudio de Grabaci�n", CategoryId = 14, IsActive = true },
            //new ArticleSubCategory { ShortName = "Instrumentos de Cuerdas", Description = "Instrumentos de Cuerdas", CategoryId = 14, IsActive = true },
            //new ArticleSubCategory { ShortName = "Instrumentos de Viento", Description = "Instrumentos de Viento", CategoryId = 14, IsActive = true },
            //new ArticleSubCategory { ShortName = "Metr�nomos", Description = "Metr�nomos", CategoryId = 14, IsActive = true },
            //new ArticleSubCategory { ShortName = "Micr�fonos y Amplificadores", Description = "Micr�fonos y Amplificadores", CategoryId = 14, IsActive = true },
            //new ArticleSubCategory { ShortName = "Parlantes y Bafles", Description = "Parlantes y Bafles", CategoryId = 14, IsActive = true },
            //new ArticleSubCategory { ShortName = "Partituras y Letras", Description = "Partituras y Letras", CategoryId = 14, IsActive = true },
            //new ArticleSubCategory { ShortName = "Pedales y Accesorios", Description = "Pedales y Accesorios", CategoryId = 14, IsActive = true },
            //new ArticleSubCategory { ShortName = "Teclados y Pianos", Description = "Teclados y Pianos", CategoryId = 14, IsActive = true },
            //new ArticleSubCategory { ShortName = "Otros", Description = "Otros", CategoryId = 14, IsActive = true },


            //new ArticleSubCategory { ShortName = "Armas y Lanzadores de Juguete", Description = "Armas y Lanzadores de Juguete", CategoryId = 15, IsActive = true },
            //new ArticleSubCategory { ShortName = "Bloques y Construcci�n", Description = "Bloques y Construcci�n", CategoryId = 15, IsActive = true },
            //new ArticleSubCategory { ShortName = "Casas y Carpas para Ni�os", Description = "Casas y Carpas para Ni�os", CategoryId = 15, IsActive = true },
            //new ArticleSubCategory { ShortName = "Dibujo, Pintura y Manualidades", Description = "Dibujo, Pintura y Manualidades", CategoryId = 15, IsActive = true },
            //new ArticleSubCategory { ShortName = "Electr�nicos para Ni�os", Description = "Electr�nicos para Ni�os", CategoryId = 15, IsActive = true },
            //new ArticleSubCategory { ShortName = "Figuritas, �lbumes y Cromos", Description = "Figuritas, �lbumes y Cromos", CategoryId = 15, IsActive = true },
            //new ArticleSubCategory { ShortName = "Hobbies", Description = "Hobbies", CategoryId = 15, IsActive = true },
            //new ArticleSubCategory { ShortName = "Instrumentos Musicales", Description = "Instrumentos Musicales", CategoryId = 15, IsActive = true },
            //new ArticleSubCategory { ShortName = "Juegos de Agua y Playa", Description = "Juegos de Agua y Playa", CategoryId = 15, IsActive = true },
            //new ArticleSubCategory { ShortName = "Juegos de Mesa y Cartas", Description = "Juegos de Mesa y Cartas", CategoryId = 15, IsActive = true },
            //new ArticleSubCategory { ShortName = "Juegos de Plaza y Aire Libre", Description = "Juegos de Plaza y Aire Libre", CategoryId = 15, IsActive = true },
            //new ArticleSubCategory { ShortName = "Juegos de Sal�n", Description = "Juegos de Sal�n", CategoryId = 15, IsActive = true },
            //new ArticleSubCategory { ShortName = "Juguetes Antiestr�s e Ingenio", Description = "Juguetes Antiestr�s e Ingenio", CategoryId = 15, IsActive = true },
            //new ArticleSubCategory { ShortName = "Juguetes de Bromas", Description = "Juguetes de Bromas", CategoryId = 15, IsActive = true },
            //new ArticleSubCategory { ShortName = "Juguetes de Oficios", Description = "Juguetes de Oficios", CategoryId = 15, IsActive = true },
            //new ArticleSubCategory { ShortName = "Juguetes para Beb�s", Description = "Juguetes para Beb�s", CategoryId = 15, IsActive = true },
            //new ArticleSubCategory { ShortName = "Mesas y Sillas para Ni�os", Description = "Mesas y Sillas para Ni�os", CategoryId = 15, IsActive = true },
            //new ArticleSubCategory { ShortName = "Mu�ecas y Mu�ecos", Description = "Mu�ecas y Mu�ecos", CategoryId = 15, IsActive = true },
            //new ArticleSubCategory { ShortName = "Patines y Patinetas", Description = "Patines y Patinetas", CategoryId = 15, IsActive = true },
            //new ArticleSubCategory { ShortName = "Peloteros y Castillos", Description = "Peloteros y Castillos", CategoryId = 15, IsActive = true },
            //new ArticleSubCategory { ShortName = "Peluches", Description = "Peluches", CategoryId = 15, IsActive = true },
            //new ArticleSubCategory { ShortName = "T�teres y Marionetas", Description = "T�teres y Marionetas", CategoryId = 15, IsActive = true },
            //new ArticleSubCategory { ShortName = "Veh�culos de Juguete", Description = "Veh�culos de Juguete", CategoryId = 15, IsActive = true },
            //new ArticleSubCategory { ShortName = "Veh�culos para Ni�os", Description = "Veh�culos para Ni�os", CategoryId = 15, IsActive = true },
            //new ArticleSubCategory { ShortName = "Otros", Description = "Otros", CategoryId = 15, IsActive = true },


            //new ArticleSubCategory { ShortName = "Cat�logos", Description = "Cat�logos", CategoryId = 16, IsActive = true },
            //new ArticleSubCategory { ShortName = "Comics", Description = "Comics", CategoryId = 16, IsActive = true },
            //new ArticleSubCategory { ShortName = "Libros", Description = "Libros", CategoryId = 16, IsActive = true },
            //new ArticleSubCategory { ShortName = "Manga", Description = "Manga", CategoryId = 16, IsActive = true },
            //new ArticleSubCategory { ShortName = "Revistas", Description = "Revistas", CategoryId = 16, IsActive = true },
            //new ArticleSubCategory { ShortName = "Otros", Description = "Otros", CategoryId = 16, IsActive = true },


            //new ArticleSubCategory { ShortName = "Cursos", Description = "Cursos", CategoryId = 17, IsActive = true },
            //new ArticleSubCategory { ShortName = "M�sica", Description = "M�sica", CategoryId = 17, IsActive = true },
            //new ArticleSubCategory { ShortName = "Pel�culas", Description = "Pel�culas", CategoryId = 17, IsActive = true },
            //new ArticleSubCategory { ShortName = "Series de TV", Description = "Series de TV", CategoryId = 17, IsActive = true },
            //new ArticleSubCategory { ShortName = "Otros", Description = "Otros", CategoryId = 17, IsActive = true },



            //new ArticleSubCategory { ShortName = "Accesorios de Moda", Description = "Accesorios de Moda", CategoryId = 18, IsActive = true },
            //new ArticleSubCategory { ShortName = "Blusas", Description = "Blusas", CategoryId = 18, IsActive = true },
            //new ArticleSubCategory { ShortName = "Calzado", Description = "Calzado", CategoryId = 18, IsActive = true },
            //new ArticleSubCategory { ShortName = "Camisas", Description = "Camisas", CategoryId = 18, IsActive = true },
            //new ArticleSubCategory { ShortName = "Camisetas, Musculosas y Polos", Description = "Camisetas, Musculosas y Polos", CategoryId = 18, IsActive = true },
            //new ArticleSubCategory { ShortName = "Chaquetas", Description = "Chaquetas", CategoryId = 18, IsActive = true },
            //new ArticleSubCategory { ShortName = "Enterizos", Description = "Enterizos", CategoryId = 18, IsActive = true },
            //new ArticleSubCategory { ShortName = "Equipaje, Bolsos y Carteras", Description = "Equipaje, Bolsos y Carteras", CategoryId = 18, IsActive = true },
            //new ArticleSubCategory { ShortName = "Faldas", Description = "Faldas", CategoryId = 18, IsActive = true },
            //new ArticleSubCategory { ShortName = "Leggings", Description = "Leggings", CategoryId = 18, IsActive = true },
            //new ArticleSubCategory { ShortName = "Lotes de Ropa", Description = "Lotes de Ropa", CategoryId = 18, IsActive = true },
            //new ArticleSubCategory { ShortName = "Pantalones y Jeans", Description = "Pantalones y Jeans", CategoryId = 18, IsActive = true },
            //new ArticleSubCategory { ShortName = "Ropa de Danza", Description = "Ropa de Danza", CategoryId = 18, IsActive = true },
            //new ArticleSubCategory { ShortName = "Ropa Deportiva", Description = "Ropa Deportiva", CategoryId = 18, IsActive = true },
            //new ArticleSubCategory { ShortName = "Ropa Interior y de Dormir", Description = "Ropa Interior y de Dormir", CategoryId = 18, IsActive = true },
            //new ArticleSubCategory { ShortName = "Ropa y Calzado para Beb�s", Description = "Ropa y Calzado para Beb�s", CategoryId = 18, IsActive = true },
            //new ArticleSubCategory { ShortName = "Saquitos, Sweaters y Chalecos", Description = "Saquitos, Sweaters y Chalecos", CategoryId = 18, IsActive = true },
            //new ArticleSubCategory { ShortName = "Shorts", Description = "Shorts", CategoryId = 18, IsActive = true },
            //new ArticleSubCategory { ShortName = "Sudaderas y Hoodies", Description = "Sudaderas y Hoodies", CategoryId = 18, IsActive = true },
            //new ArticleSubCategory { ShortName = "Trajes", Description = "Trajes", CategoryId = 18, IsActive = true },
            //new ArticleSubCategory { ShortName = "Uniformes", Description = "Uniformes", CategoryId = 18, IsActive = true },
            //new ArticleSubCategory { ShortName = "Vestidos", Description = "Vestidos", CategoryId = 18, IsActive = true },
            //new ArticleSubCategory { ShortName = "Vestidos de Ba�o", Description = "Vestidos de Ba�o", CategoryId = 18, IsActive = true },
            //new ArticleSubCategory { ShortName = "Otros", Description = "Otros", CategoryId = 18, IsActive = true },


            //new ArticleSubCategory { ShortName = "Belleza e Higiene Personal", Description = "Belleza e Higiene Personal", CategoryId = 19, IsActive = true },
            //new ArticleSubCategory { ShortName = "Ense�anza", Description = "Ense�anza", CategoryId = 19, IsActive = true },
            //new ArticleSubCategory { ShortName = "Fiestas y Eventos", Description = "Fiestas y Eventos", CategoryId = 19, IsActive = true },
            //new ArticleSubCategory { ShortName = "Gastronom�a", Description = "Gastronom�a", CategoryId = 19, IsActive = true },
            //new ArticleSubCategory { ShortName = "Mantenimiento del Hogar", Description = "Mantenimiento del Hogar", CategoryId = 19, IsActive = true },
            //new ArticleSubCategory { ShortName = "Reparaciones e Instalaciones", Description = "Reparaciones e Instalaciones", CategoryId = 19, IsActive = true },
            //new ArticleSubCategory { ShortName = "Transporte General", Description = "Transporte General", CategoryId = 19, IsActive = true },
            //new ArticleSubCategory { ShortName = "Viajes y Turismo", Description = "Viajes y Turismo", CategoryId = 19, IsActive = true },
            //new ArticleSubCategory { ShortName = "Otros Servicios", Description = "Otros Servicios", CategoryId = 19, IsActive = true },
            //new ArticleSubCategory { ShortName = "Adultos", Description = "Adultos", CategoryId = 20, IsActive = true },
            //new ArticleSubCategory { ShortName = "Alimentos y Bebidas", Description = "Alimentos y Bebidas", CategoryId = 20, IsActive = true },
            //new ArticleSubCategory { ShortName = "Animales y Mascotas", Description = "Animales y Mascotas", CategoryId = 20, IsActive = true },
            //new ArticleSubCategory { ShortName = "Art�culos para Fumadores", Description = "Art�culos para Fumadores", CategoryId = 20, IsActive = true },
            //new ArticleSubCategory { ShortName = "Beb�s", Description = "Beb�s", CategoryId = 20, IsActive = true },
            //new ArticleSubCategory { ShortName = "Belleza y Cuidado Personal", Description = "Belleza y Cuidado Personal", CategoryId = 20, IsActive = true },
            //new ArticleSubCategory { ShortName = "Coberturas Extendidas", Description = "Coberturas Extendidas", CategoryId = 20, IsActive = true },
            //new ArticleSubCategory { ShortName = "Criptomonedas", Description = "Criptomonedas", CategoryId = 20, IsActive = true },
            //new ArticleSubCategory { ShortName = "Esoterismo", Description = "Esoterismo", CategoryId = 20, IsActive = true },
            //new ArticleSubCategory { ShortName = "Gift Cards", Description = "Gift Cards", CategoryId = 20, IsActive = true },
            //new ArticleSubCategory { ShortName = "Herramientas y Construcci�n", Description = "Herramientas y Construcci�n", CategoryId = 20, IsActive = true },
            //new ArticleSubCategory { ShortName = "Hornos Crematorios", Description = "Hornos Crematorios", CategoryId = 20, IsActive = true },
            //new ArticleSubCategory { ShortName = "Industrias y Oficinas", Description = "Industrias y Oficinas", CategoryId = 20, IsActive = true },
            //new ArticleSubCategory { ShortName = "Insumos para Tatuajes", Description = "Insumos para Tatuajes", CategoryId = 20, IsActive = true },
            //new ArticleSubCategory { ShortName = "Joyas y Relojes", Description = "Joyas y Relojes", CategoryId = 20, IsActive = true },
            //new ArticleSubCategory { ShortName = "Kits de Criminal�stica", Description = "Kits de Criminal�stica", CategoryId = 20, IsActive = true },
            //new ArticleSubCategory { ShortName = "Licencias para Taxis", Description = "Licencias para Taxis", CategoryId = 20, IsActive = true },
            //new ArticleSubCategory { ShortName = "Mangas de Viento", Description = "Mangas de Viento", CategoryId = 20, IsActive = true },
            //new ArticleSubCategory { ShortName = "Pirotecnia", Description = "Pirotecnia", CategoryId = 20, IsActive = true },
            //new ArticleSubCategory { ShortName = "Recuerdos, Cotill�n y Fiestas", Description = "Recuerdos, Cotill�n y Fiestas", CategoryId = 20, IsActive = true },
            //new ArticleSubCategory { ShortName = "Salud y Equipamiento M�dico", Description = "Salud y Equipamiento M�dico", CategoryId = 20, IsActive = true },
            //new ArticleSubCategory { ShortName = "Otros", Description = "Otros", CategoryId = 20, IsActive = true },

            //new ArticleSubCategory { ShortName = "Marisco", Description = "Marisco", CategoryId = 21, IsActive = true },
            //new ArticleSubCategory { ShortName = "Res", Description = "Res", CategoryId = 21, IsActive = true },
            //new ArticleSubCategory { ShortName = "Ave", Description = "Ave", CategoryId = 21, IsActive = true },
            //new ArticleSubCategory { ShortName = "Cerdo", Description = "Cerdo", CategoryId = 21, IsActive = true },
            //new ArticleSubCategory { ShortName = "Conejo", Description = "Conejo", CategoryId = 21, IsActive = true },
            //new ArticleSubCategory { ShortName = "Chivo", Description = "Chivo", CategoryId = 21, IsActive = true }

            //);


        }
    }
}
