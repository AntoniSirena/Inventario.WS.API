using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JS.Base.WS.API.DBContext;
using SpreadsheetLight;

namespace JS.Base.WS.API.Services.Domain
{
    public class InventoryServices
    {
        private MyDBcontext db;

        public InventoryServices()
        {
            db = new MyDBcontext();
        }

        public void GenerateInventoryExcel(long inventoryId)
        {

            string result = string.Empty;
            string filePath = @"C:\SharedInventario\Files\" + "Products.xlsx";

            var inventory = db.Inventories.Where(x => x.Id == inventoryId).FirstOrDefault();

            SLDocument oSLDocument = new SLDocument();

            System.Data.DataTable dt = new System.Data.DataTable();

            //columns
            dt.Columns.Add("Descripción", typeof(string));
            dt.Columns.Add("Código de sistem", typeof(string));
            dt.Columns.Add("Código de barra", typeof(string));
            dt.Columns.Add("Costo", typeof(decimal));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Cantidad", typeof(decimal));


            //rows
            foreach (var item in inventory.InventoryDetails)
            {
                dt.Rows.Add(item.Product.Description, item.Product.ExternalCode, item.Product.BarCode, item.CurrentCost, item.CurrentPrice, item.Quantity);
            }

            oSLDocument.ImportDataTable(1, 1, dt, true);

            oSLDocument.SaveAs(filePath);

            JS.Utilities.JS_File.DownloadFile(File.ReadAllBytes(filePath), "application/xlsx", "Productos", "xlsx");
        }

    }
}