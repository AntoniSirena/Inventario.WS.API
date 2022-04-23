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
            string filePath = @"C:\SharedInventario\Files\" + "guia de entradas.xlsx";

            var inventory = db.Inventories.Where(x => x.Id == inventoryId).FirstOrDefault();

            SLDocument oSLDocument = new SLDocument();

            System.Data.DataTable dt = new System.Data.DataTable();

            //columns
            dt.Columns.Add("merc", typeof(string));
            dt.Columns.Add("descrip", typeof(string));
            dt.Columns.Add("cantidad", typeof(decimal));
            dt.Columns.Add("disponible", typeof(decimal));
            dt.Columns.Add("precio", typeof(decimal));
            dt.Columns.Add("dtasa", typeof(decimal));

            dt.Columns.Add("dvalor", typeof(decimal));
            dt.Columns.Add("campo1", typeof(string));
            dt.Columns.Add("campo2", typeof(string));
            dt.Columns.Add("auxiliard", typeof(string));

            dt.Columns.Add("preciov", typeof(decimal));
            dt.Columns.Add("unidad", typeof(string));
            dt.Columns.Add("factor", typeof(int));
            dt.Columns.Add("imprime", typeof(string));
            dt.Columns.Add("_updated", typeof(string));


            //rows
            foreach (var item in inventory.InventoryDetails)
            {
                dt.Rows.Add(item.Product.ExternalCode, item.Product.Description, item.Quantity, 0, item.CurrentCost, 0, 0, "", "", "", 0, "Unidad", 1, "FALSE", DateTime.Now.ToString("dd/MM/yyyy hh:mm"));
            }

            oSLDocument.ImportDataTable(1, 1, dt, true);

            oSLDocument.SaveAs(filePath);

            JS.Utilities.JS_File.DownloadFile(File.ReadAllBytes(filePath), "application/xlsx", "guia de entradas", "xlsx");
        }

    }
}