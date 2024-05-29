using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection.Metadata;
using System.Reflection;
using System.Xml.Linq;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ClosedXML.Excel;
using System.Text;
using PdfSharpCore;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using Document = DocumentFormat.OpenXml.Wordprocessing.Document;
using DocumentFormat.OpenXml.Office.CustomUI;

namespace SQMS.Utility
{
    public class ExportManager
    {
        public MemoryStream ExportToPdf<T, TModel>(List<T> list, TModel model, string reportTitle, int orientationVal, string[] headers, double headerHeight, double[] columnWidths)
        {
            MemoryStream outputStream = new MemoryStream();
            try
            {
                PdfDocument document = new PdfDocument();

                PdfPage page = document.AddPage();
                XUnit leftMargin = XUnit.FromInch(2);
                XUnit rightMargin = XUnit.FromInch(0.5);

                if (orientationVal == 1)
                    page.Orientation = PageOrientation.Landscape;
                else
                    page.Orientation = PageOrientation.Portrait;

                XGraphics gfx = XGraphics.FromPdfPage(page);

                XFont fontTitle = new XFont("Arial", 16, XFontStyle.Bold);
                XFont fontHeader = new XFont("Arial", 10, XFontStyle.Regular);
                XFont fontData = new XFont("Arial", 9);

                double cellHeight = 20;

                int rowCount = list.Count;
                int columnCount = headers.Length;
                double totalWidth = 0;
                foreach (double width in columnWidths)
                {
                    totalWidth += width;
                }
                double startX = (page.Width - totalWidth) / 2;
                double startY = 50; // Adjust vertically as needed
                double startY2 = 50; // Adjust vertically as needed

                XRect titleRect = new XRect(0, 20, page.Width, 30);
                gfx.DrawString(reportTitle, fontTitle, XBrushes.Black, titleRect, XStringFormats.Center);

                double x = startX;
                double y = startY;
                var properties = typeof(TModel).GetProperties();
                string[] propertyNames = new string[properties.Length];

                for (int i = 0; i < properties.Length; i++)
                {
                    propertyNames[i] = properties[i].Name;
                }
                for (int j = 0; j < columnCount; j++)
                {
                    gfx.DrawRectangle(XPens.Black, x, y, columnWidths[j], headerHeight);
                    string content = headers[j];
                    XSize textSize = gfx.MeasureString(content, fontHeader);
                    double textX = x + (columnWidths[j] - textSize.Width) / 2;
                    double textY = y + (headerHeight - textSize.Height) / 1; // Center vertically

                    if (textSize.Width > columnWidths[j])
                    {
                        List<string> lines = new List<string>();
                        string[] words = content.Split(' ');
                        string currentLine = "";
                        foreach (string word in words)
                        {
                            string testLine = currentLine + word + " ";
                            XSize testSize = gfx.MeasureString(testLine, fontHeader);
                            if (testSize.Width > columnWidths[j])
                            {
                                lines.Add(currentLine);
                                currentLine = word + " ";
                            }
                            else
                            {
                                currentLine = testLine;
                            }
                        }
                        lines.Add(currentLine);

                        double lineHeight = gfx.MeasureString("Test", fontHeader).Height;

                        if (textSize.Height > headerHeight)
                        {
                            textY = y;
                        }
                        else
                        {
                            textY = y + (headerHeight - textSize.Height) / 2;
                        }

                        foreach (string line in lines)
                        {
                            XSize lineSize = gfx.MeasureString(line.Trim(), fontHeader);
                            double lineTextX = x + (columnWidths[j] - lineSize.Width) / 2;
                            gfx.DrawString(line.Trim(), fontHeader, XBrushes.Black, lineTextX, textY);
                            textY += lineHeight;
                        }
                    }
                    else
                    {
                        gfx.DrawString(content, fontHeader, XBrushes.Black, textX, textY);
                    }
                    x += columnWidths[j];
                }
                startY += headerHeight;
                startY2 += headerHeight;

                int currentPageIndex = 0; // Track the current page index
                int itemsPerPage = (int)((gfx.PageSize.Height - startY-30) / cellHeight); // Calculate items per page based on available space

                foreach (var item in list)
                {
                    if (itemsPerPage <= 0)
                    {
                        currentPageIndex++;
                        page = document.AddPage();

                        if (currentPageIndex % 2 == 0)
                        {
                            if (orientationVal == 1)
                                page.Orientation = PageOrientation.Landscape;
                            else
                                page.Orientation = PageOrientation.Portrait;
                        }
                        else
                        {
                            if (orientationVal == 1)
                                page.Orientation = PageOrientation.Landscape;
                            else
                                page.Orientation = PageOrientation.Portrait;
                        }
                        gfx = XGraphics.FromPdfPage(page);
                        startY = startY2; // Reset startY for the new page
                        itemsPerPage = (int)((gfx.PageSize.Height - startY - 30) / cellHeight); // Recalculate items per page
                    }

                    x = startX;
                    double maxRowHeight = cellHeight; // Initialize the max row height for this row

                    for (int j = 0; j < columnCount; j++)
                    {
                        gfx.DrawRectangle(XPens.Black, x, startY, columnWidths[j], maxRowHeight); // Use maxRowHeight instead of cellHeight
                        string content = item.GetType().GetProperty(propertyNames[j]).GetValue(item, null)?.ToString() ?? ""; // Handle null values

                        // Measure the size of the text
                        XSize textSize = gfx.MeasureString(content, fontData);

                        // Check if text width exceeds column width
                        if (textSize.Width > columnWidths[j])
                        {
                            // Calculate the number of lines needed for text wrapping
                            int linesNeeded = (int)Math.Ceiling(textSize.Width / columnWidths[j]);

                            // Split content into multiple lines
                            var words = content.Split(' ');
                            var lines = new List<string>();
                            string line = string.Empty;
                            foreach (var word in words)
                            {
                                if (gfx.MeasureString(line + " " + word, fontData).Width <= columnWidths[j])
                                {
                                    line += (string.IsNullOrEmpty(line) ? "" : " ") + word;
                                }
                                else
                                {
                                    lines.Add(line);
                                    line = word;
                                }
                            }
                            lines.Add(line);

                            // Calculate the total height required for wrapped text
                            double totalTextHeight = textSize.Height * linesNeeded;

                            // Adjust the maxRowHeight if needed
                            if (totalTextHeight > maxRowHeight)
                            {
                                maxRowHeight = totalTextHeight;
                            }

                            // Draw each line of wrapped text
                            double textY = startY + (maxRowHeight - totalTextHeight) / 2;
                            foreach (var wrappedLine in lines)
                            {
                                gfx.DrawString(wrappedLine, fontData, XBrushes.Black, x, textY);
                                textY += textSize.Height;
                            }
                        }
                        else
                        {
                            // Draw single line of text
                            double textX = x + (columnWidths[j] - textSize.Width) / 2;
                            double textY = startY + (maxRowHeight - textSize.Height) / 1;
                            gfx.DrawString(content, fontData, XBrushes.Black, textX, textY);
                        }

                        x += columnWidths[j];
                    }
                    startY += maxRowHeight; // Increase startY by maxRowHeight instead of cellHeight
                    itemsPerPage--;

                    if (itemsPerPage == 0 && currentPageIndex < document.PageCount)
                    {
                        gfx.DrawString((currentPageIndex + 1).ToString(), fontHeader, XBrushes.Black, new XPoint(page.Width - rightMargin.Point, page.Height - 15));
                    }
                }


                //foreach (var item in list)
                //{
                //    if (itemsPerPage <= 0)
                //    {
                //        currentPageIndex++;
                //        page = document.AddPage();

                //        if (currentPageIndex % 2 == 0)
                //        {
                //            if (orientationVal == 1)
                //                page.Orientation = PageOrientation.Landscape;
                //            else
                //                page.Orientation = PageOrientation.Portrait;
                //        }
                //        else
                //        {
                //            if (orientationVal == 1)
                //                page.Orientation = PageOrientation.Landscape;
                //            else
                //                page.Orientation = PageOrientation.Portrait;
                //        }
                //        gfx = XGraphics.FromPdfPage(page);
                //        startY = startY2; // Reset startY for the new page
                //        itemsPerPage = (int)((gfx.PageSize.Height - startY -30) / cellHeight); // Recalculate items per page

                //    }

                //    x = startX;
                //    for (int j = 0; j < columnCount; j++)
                //    {
                //        gfx.DrawRectangle(XPens.Black, x, startY, columnWidths[j], cellHeight);
                //        string content = item.GetType().GetProperty(propertyNames[j]).GetValue(item, null)?.ToString() ?? ""; // Handle null values
                //        XSize textSize = gfx.MeasureString(content, fontData);
                //        double textX = x + (columnWidths[j] - textSize.Width) / 2;
                //        double textY = startY + (cellHeight - textSize.Height) / 1; 
                //        gfx.DrawString(content, fontData, XBrushes.Black, textX, textY);
                //        x += columnWidths[j];
                //    }
                //    startY += cellHeight;
                //    itemsPerPage--; 

                //    if (itemsPerPage == 0 && currentPageIndex < document.PageCount)
                //    {
                //        gfx.DrawString((currentPageIndex + 1).ToString(), fontHeader, XBrushes.Black, new XPoint(page.Width - rightMargin.Point, page.Height - 15));
                //    }                    
                //}
                gfx.DrawString((currentPageIndex + 1).ToString(), fontHeader, XBrushes.Black, new XPoint(page.Width - rightMargin.Point, page.Height - 15));
                document.Save(outputStream, false);
                document.Close();
                outputStream.Position = 0;
            }
            catch (Exception)
            {
                throw;
            }
            return outputStream;
        }
        public MemoryStream ExportToExcel<T, TModel>(List<T> list, TModel model, string reportTitle, int orientationVal, string[] headers, double headerHeight, double[] columnWidths)
        {
            MemoryStream stream = new MemoryStream();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Sheet1");

                worksheet.Cell(1, 1).Value = reportTitle;
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(1, 1).Style.Font.FontSize = 16;

                // Write headers
                int headerRow = 2;
                int headerColumn = 1;
                int columnCount = headers.Length;
                foreach (var header in headers)
                {
                    worksheet.Cell(headerRow, headerColumn).Value = header;
                    worksheet.Cell(headerRow, headerColumn).Style.Font.Bold = true;
                    worksheet.Row(headerRow).Height = headerHeight;
                    headerColumn++;
                }

                var properties = typeof(TModel).GetProperties();
                string[] propertyNames = new string[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    propertyNames[i] = properties[i].Name;
                }
                // Write data
                int dataRow = headerRow + 1;
                foreach (var item in list)
                {
                    int dataColumn = 1;
                    
                    for (int i = 0; i < columnCount; i++)
                    {
                        worksheet.Cell(dataRow, dataColumn).Value = item.GetType().GetProperty(propertyNames[i]).GetValue(item, null)?.ToString() ?? "";
                        dataColumn++;
                    }
                    dataRow++;
                }

                workbook.SaveAs(stream);
            }

            stream.Position = 0;
            return stream;
        }
        public MemoryStream ExportToCSV<T, TModel>(List<T> list, TModel model, string[] headers)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);

            // Write headers
            writer.WriteLine(string.Join(",", headers));

            var properties = typeof(TModel).GetProperties();
            string[] propertyNames = new string[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                propertyNames[i] = properties[i].Name;
            }

            // Write data
            foreach (var item in list)
            {
                List<string> rowData = new List<string>();
                foreach (var propertyName in propertyNames)
                {
                    var propertyValue = item.GetType().GetProperty(propertyName).GetValue(item, null);
                    rowData.Add(propertyValue?.ToString() ?? "");
                }
                writer.WriteLine(string.Join(",", rowData));
            }

            writer.Flush();
            stream.Position = 0;
            return stream;
        }
        public MemoryStream ExportToWord<T, TModel>(List<T> list, TModel model, string reportTitle, int orientationVal, string[] headers, double headerHeight, double[] columnWidths)
        {
            MemoryStream outputStream = new MemoryStream();
            int columnCount = headers.Length;
            var properties = typeof(TModel).GetProperties();
            string[] propertyNames = new string[properties.Length];

            for (int i = 0; i < properties.Length; i++)
            {
                propertyNames[i] = properties[i].Name;
            }

            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(outputStream, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());

                // Add title
                Paragraph titleParagraph = body.AppendChild(new Paragraph());
                Run titleRun = titleParagraph.AppendChild(new Run());
                titleRun.AppendChild(new Text(reportTitle));
                titleRun.RunProperties = new RunProperties(new Bold(), new FontSize() { Val = "32" }); // Adjust font size as needed

                // Add headers
                Table table = new Table();
                TableProperties tableProperties = new TableProperties(
                    new TableBorders(
                        new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                        new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                        new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                        new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                        new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                        new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 }
                    )
                );

                table.AppendChild(tableProperties);

                TableRow headerRow = new TableRow();

                foreach (string header in headers)
                {
                    TableCell headerCell = new TableCell();
                    Paragraph headerParagraph = new Paragraph();
                    Run headerRun = new Run();
                    headerRun.Append(new Text(header));
                    headerParagraph.Append(headerRun);
                    headerCell.Append(headerParagraph);
                    headerRow.Append(headerCell);
                }

                table.Append(headerRow);

                // Add data
                foreach (var item in list)
                {
                    TableRow dataRow = new TableRow();

                    for (int i = 0; i < columnCount; i++)
                    {
                        string content = item.GetType().GetProperty(propertyNames[i]).GetValue(item, null)?.ToString() ?? "";
                        TableCell dataCell = new TableCell();
                        Paragraph dataParagraph = new Paragraph();
                        Run dataRun = new Run();
                        dataRun.Append(new Text(content));
                        dataParagraph.Append(dataRun);
                        dataCell.Append(dataParagraph);
                        dataRow.Append(dataCell);
                    }

                    table.Append(dataRow);
                }

                body.Append(table);
            }

            outputStream.Position = 0;
            return outputStream;
        }
    }
}
