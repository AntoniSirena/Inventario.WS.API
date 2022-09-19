using System;
using System.IO;
using System.Linq;
using JS.Base.WS.API.DBContext;
using JS.Base.WS.API.DTO.Domain;
using SpreadsheetLight;
using iTextSharp.text;
using iTextSharp.text.pdf;

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


        public void GenerateInventoryDetailsPDF()
        {

            var inventories = db.Inventories.Where(x => x.IsActive == true && x.IsDeleted == false).Select(y => new InventoryDTO()
            {
                Id = y.Id,
                Status = y.InventoryStatus.Description,
                StatuShortName = y.InventoryStatus.ShortName,
                StatusColour = y.InventoryStatus.Colour,
                UserName = y.User.UserName,
                Description = y.Description,
                OpenDate = y.OpenDateFormatted,
                ClosedDate = y.ClosedDateFormatted,

            }).ToList();

            foreach (var item in inventories)
            {
                var details = db.InventoryDetails.Where(x => x.InventoryId == item.Id).ToList();

                if (details.Count > 0)
                {
                    item.TotalAmount = details.Sum(y => y.TotalAmount);
                }
            }

            decimal sumary = 0;

            foreach(var item in inventories)
            {
                sumary += item.TotalAmount;
            }


            var inventoryDetails = db.InventoryDetails.Where(x => x.IsActive == true && x.IsDeleted == false).Select(y => new ProductDTO()
            {
                Id = y.Product.Id,
                InventoryDetailId = y.Id,
                Description = y.Product.Description,
                ExternalCode = y.Product.ExternalCode,
                BarCode = y.Product.BarCode,
                OldCost = y.OldCost,
                OldPrice = y.OldPrice,
                Cost = y.CurrentCost,
                Price = y.CurrentPrice,
                Reference = y.Product.Reference,
                Existence = y.Product.Existence,
                Quantity = y.Quantity,
                TotalAmount = y.TotalAmount,
                Difference = y.Difference,
                UserName = y.User.UserName

            }).OrderBy(x => x.UserName).ToList();


            string businessName = Global.Constants.ConfigurationParameter.BusinessName;
            string businessAddress = Global.Constants.ConfigurationParameter.BusinessAddress;


            //Create pdf
            MemoryStream ms = new MemoryStream();

            Document document = new Document(PageSize.LETTER, 20, 20, 20, 20);
            PdfWriter pdfW = PdfWriter.GetInstance(document, ms);

            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            var suubTitleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);

            var labelFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            var labelFontDetail = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
            var labelFontValue = FontFactory.GetFont(FontFactory.HELVETICA, 9);


            //Document open
            document.Open();


            Paragraph clBusinessName = new Paragraph(businessName, titleFont);
            clBusinessName.Alignment = Element.ALIGN_CENTER;

            Paragraph clBusinessAddress = new Paragraph(businessAddress, suubTitleFont);
            clBusinessAddress.Alignment = Element.ALIGN_CENTER;

            document.Add(Chunk.NEWLINE);
            document.Add(clBusinessName);
            document.Add(clBusinessAddress);

            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);


            //Inventories
            Paragraph titleDetail = new Paragraph("Inventarios", labelFont);
            titleDetail.Alignment = Element.ALIGN_CENTER;
            document.Add(titleDetail);
            document.Add(new Paragraph("\n"));

            PdfPTable tbInventory = new PdfPTable(5);
            tbInventory.WidthPercentage = 100;
            float[] ddetailWidth = new float[] { 30f, 20f, 20f, 15f, 15f };
            tbInventory.SetWidths(ddetailWidth);

            PdfPCell clDescription = new PdfPCell(new Phrase("Descripción", labelFontDetail));
            clDescription.HorizontalAlignment = Element.ALIGN_LEFT;
            clDescription.BorderWidth = 1;

            PdfPCell clOpenDate = new PdfPCell(new Phrase("Fecha de apertura", labelFontDetail));
            clOpenDate.HorizontalAlignment = Element.ALIGN_LEFT;
            clOpenDate.BorderWidth = 1;

            PdfPCell clClosed = new PdfPCell(new Phrase("Fecha de cierre", labelFontDetail));
            clClosed.HorizontalAlignment = Element.ALIGN_LEFT;
            clClosed.BorderWidth = 1;

            PdfPCell clTotalAmoun = new PdfPCell(new Phrase("Valor del inventario", labelFontDetail));
            clTotalAmoun.HorizontalAlignment = Element.ALIGN_LEFT;
            clTotalAmoun.BorderWidth = 1;

            PdfPCell clUser = new PdfPCell(new Phrase("Usuario", labelFontDetail));
            clUser.HorizontalAlignment = Element.ALIGN_LEFT;
            clUser.BorderWidth = 1;


            tbInventory.AddCell(clDescription);
            tbInventory.AddCell(clOpenDate);
            tbInventory.AddCell(clClosed);
            tbInventory.AddCell(clTotalAmoun);
            tbInventory.AddCell(clUser);


            foreach (var item in inventories)
            {
                clDescription = new PdfPCell(new Phrase(item.Description, labelFontValue));
                clDescription.HorizontalAlignment = Element.ALIGN_LEFT;
                clDescription.BorderWidth = 1;

                clOpenDate = new PdfPCell(new Phrase(item.OpenDate, labelFontValue));
                clOpenDate.HorizontalAlignment = Element.ALIGN_LEFT;
                clOpenDate.BorderWidth = 1;

                clClosed = new PdfPCell(new Phrase(item.ClosedDate, labelFontValue));
                clClosed.HorizontalAlignment = Element.ALIGN_LEFT;
                clClosed.BorderWidth = 1;

                clTotalAmoun = new PdfPCell(new Phrase(item.TotalAmount.ToString(), labelFontValue));
                clTotalAmoun.HorizontalAlignment = Element.ALIGN_LEFT;
                clTotalAmoun.BorderWidth = 1;

                clUser = new PdfPCell(new Phrase(item.UserName, labelFontValue));
                clUser.HorizontalAlignment = Element.ALIGN_LEFT;
                clUser.BorderWidth = 1;

                tbInventory.AddCell(clDescription);
                tbInventory.AddCell(clOpenDate);
                tbInventory.AddCell(clClosed);
                tbInventory.AddCell(clTotalAmoun);
                tbInventory.AddCell(clUser);
            }


            document.Add(tbInventory);
            document.Add(new Paragraph("\n"));


            //Summary
            PdfPTable tbSummary = new PdfPTable(2);
            tbSummary.WidthPercentage = 50;
            float[] summaryWidths = new float[] { 35f, 15f };
            tbSummary.SetWidths(summaryWidths);
            tbSummary.HorizontalAlignment = Element.ALIGN_RIGHT;


            PdfPCell clSummaryDescription = new PdfPCell(new Phrase("Valor general del inventario:", labelFont));
            clSummaryDescription.HorizontalAlignment = Element.ALIGN_LEFT;
            clSummaryDescription.BorderWidth = 1;

            PdfPCell clSummaryValue = new PdfPCell(new Phrase(sumary.ToString(), labelFontValue));
            clSummaryValue.HorizontalAlignment = Element.ALIGN_RIGHT;
            clSummaryValue.BorderWidth = 1;

            tbSummary.AddCell(clSummaryDescription);
            tbSummary.AddCell(clSummaryValue);


            document.Add(tbSummary);
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));



            //Items count quantity
            PdfPTable tbCountQuantity = new PdfPTable(2);
            tbCountQuantity.WidthPercentage = 50;
            float[] countQuantityWidths = new float[] { 35f, 15f };
            tbCountQuantity.SetWidths(countQuantityWidths);
            tbCountQuantity.HorizontalAlignment = Element.ALIGN_RIGHT;


            PdfPCell clItemCountQuantityDescription = new PdfPCell(new Phrase("Total de items contados:", labelFont));
            clItemCountQuantityDescription.HorizontalAlignment = Element.ALIGN_LEFT;
            clItemCountQuantityDescription.BorderWidth = 1;

            PdfPCell clItemCountQuantityValue = new PdfPCell(new Phrase(inventoryDetails.Count().ToString(), labelFontValue));
            clItemCountQuantityValue.HorizontalAlignment = Element.ALIGN_RIGHT;
            clItemCountQuantityValue.BorderWidth = 1;

            tbCountQuantity.AddCell(clItemCountQuantityDescription);
            tbCountQuantity.AddCell(clItemCountQuantityValue);

            document.Add(tbCountQuantity);
            document.Add(new Paragraph("\n"));


            //Inventory details
            PdfPTable tbInventoryDetails = new PdfPTable(8);
            tbInventoryDetails.WidthPercentage = 100;
            float[] dinventoryDetailWidth = new float[] { 10f, 25f, 10f, 10f, 10f, 10f, 15f, 10f };
            tbInventoryDetails.SetWidths(dinventoryDetailWidth);


            PdfPCell clCodeDetail = new PdfPCell(new Phrase("Código", labelFontDetail));
            clCodeDetail.HorizontalAlignment = Element.ALIGN_LEFT;
            clCodeDetail.BorderWidth = 1;

            PdfPCell clDescriptionDetail = new PdfPCell(new Phrase("Descripción", labelFontDetail));
            clDescriptionDetail.HorizontalAlignment = Element.ALIGN_LEFT;
            clDescriptionDetail.BorderWidth = 1;

            PdfPCell clQuantityDetail = new PdfPCell(new Phrase("Cantidad actual", labelFontDetail));
            clQuantityDetail.HorizontalAlignment = Element.ALIGN_LEFT;
            clQuantityDetail.BorderWidth = 1;

            PdfPCell clQuantityCountDetail = new PdfPCell(new Phrase("Cantidad contada", labelFontDetail));
            clQuantityCountDetail.HorizontalAlignment = Element.ALIGN_LEFT;
            clQuantityCountDetail.BorderWidth = 1;

            PdfPCell clDifferenceDetail = new PdfPCell(new Phrase("Diferencia", labelFontDetail));
            clDifferenceDetail.HorizontalAlignment = Element.ALIGN_LEFT;
            clDifferenceDetail.BorderWidth = 1;

            PdfPCell clCostDetail = new PdfPCell(new Phrase("Costo", labelFontDetail));
            clCostDetail.HorizontalAlignment = Element.ALIGN_LEFT;
            clCostDetail.BorderWidth = 1;

            PdfPCell clTotalAmounDetail = new PdfPCell(new Phrase("Valor del inventario", labelFontDetail));
            clTotalAmounDetail.HorizontalAlignment = Element.ALIGN_LEFT;
            clTotalAmounDetail.BorderWidth = 1;

            PdfPCell clUserDetail = new PdfPCell(new Phrase("Usuario", labelFontDetail));
            clUserDetail.HorizontalAlignment = Element.ALIGN_LEFT;
            clUserDetail.BorderWidth = 1;

            tbInventoryDetails.AddCell(clCodeDetail);
            tbInventoryDetails.AddCell(clDescriptionDetail);
            tbInventoryDetails.AddCell(clQuantityDetail);
            tbInventoryDetails.AddCell(clQuantityCountDetail);
            tbInventoryDetails.AddCell(clDifferenceDetail);
            tbInventoryDetails.AddCell(clCostDetail);
            tbInventoryDetails.AddCell(clTotalAmounDetail);
            tbInventoryDetails.AddCell(clUserDetail);


            foreach (var item in inventoryDetails)
            {
                clCodeDetail = new PdfPCell(new Phrase(item.ExternalCode, labelFontValue));
                clCodeDetail.HorizontalAlignment = Element.ALIGN_LEFT;
                clCodeDetail.BorderWidth = 1;

                clDescriptionDetail = new PdfPCell(new Phrase(item.Description, labelFontValue));
                clDescriptionDetail.HorizontalAlignment = Element.ALIGN_LEFT;
                clDescriptionDetail.BorderWidth = 1;

                clQuantityDetail = new PdfPCell(new Phrase(item.Existence.ToString(), labelFontValue));
                clQuantityDetail.HorizontalAlignment = Element.ALIGN_LEFT;
                clQuantityDetail.BorderWidth = 1;

                clQuantityCountDetail = new PdfPCell(new Phrase(item.Quantity.ToString(), labelFontValue));
                clQuantityCountDetail.HorizontalAlignment = Element.ALIGN_LEFT;
                clQuantityCountDetail.BorderWidth = 1;

                clDifferenceDetail = new PdfPCell(new Phrase(item.Difference.ToString(), labelFontValue));
                clDifferenceDetail.HorizontalAlignment = Element.ALIGN_LEFT;
                clDifferenceDetail.BorderWidth = 1;

                clCostDetail = new PdfPCell(new Phrase(item.Cost.ToString(), labelFontValue));
                clCostDetail.HorizontalAlignment = Element.ALIGN_LEFT;
                clCostDetail.BorderWidth = 1;

                clTotalAmounDetail = new PdfPCell(new Phrase(item.TotalAmount.ToString(), labelFontValue));
                clTotalAmounDetail.HorizontalAlignment = Element.ALIGN_LEFT;
                clTotalAmounDetail.BorderWidth = 1;

                clUserDetail = new PdfPCell(new Phrase(item.UserName, labelFontValue));
                clUserDetail.HorizontalAlignment = Element.ALIGN_LEFT;
                clUserDetail.BorderWidth = 1;

                tbInventoryDetails.AddCell(clCodeDetail);
                tbInventoryDetails.AddCell(clDescriptionDetail);
                tbInventoryDetails.AddCell(clQuantityDetail);
                tbInventoryDetails.AddCell(clQuantityCountDetail);
                tbInventoryDetails.AddCell(clDifferenceDetail);
                tbInventoryDetails.AddCell(clCostDetail);
                tbInventoryDetails.AddCell(clTotalAmounDetail);
                tbInventoryDetails.AddCell(clUserDetail);
            }


            document.Add(tbInventoryDetails);



            document.Close();
            //Document close


            byte[] bytesPDF = ms.ToArray();

            ms = new MemoryStream();
            ms.Write(bytesPDF, 0, bytesPDF.Length);
            ms.Position = 0;

            //Download file
            JS.Utilities.JS_File.DownloadFile(bytesPDF, "application/pdf", "Reporte de inventarios", "pdf");

        }

    }
}