﻿using System.IO;
using PdfSharp.Fonts;

namespace InvoiceCreator.PDF;
public class InvoiceMaker
{
  const bool _includeBank = true;
  readonly PdfDocument _pdfDoc = new();

  public void PrepareInvoice(
    string coName1,
    string coAdrs1,
    string coName2,
    string coAdrs2,
    int invcNumber,
    string invcDate,
    string pyPeriod,
    string payPrdHrs,
    string jobDescrn,
    string ratePerHr,
    string total_ttl,
    string sub_total,
    string taxRatePc,
    string salesTaxM,
    string grandtotl)
  {
    try
    {
      const string fontname = "Segoe UI";
      const int lineHeight1 = 15, lineHeight2 = 24;

      var dark = XColors.Black;
      var lite = XColors.DodgerBlue;
      var darkb = new XSolidBrush(dark);
      var liteb = new XSolidBrush(lite);
      var litep = new XPen(lite);

      if (GlobalFontSettings.FontResolver == null) // Add a custom font resolver .. only once per run!
        GlobalFontSettings.FontResolver = new MyFontResolver();

      _pdfDoc.Info.Title = "Created with PDFsharp";

      var page = _pdfDoc.AddPage();          // Create an empty page in this document.
      var gfx = XGraphics.FromPdfPage(page);  // Get an XGraphics object for drawing on this page.

      var col1 = page.Width * .10;
      var colA = page.Width * .17;
      var col2 = page.Width * .56;
      var colB = page.Width * .26;
      var col3 = page.Width * .70;
      var colM = page.Width * .76;
      var col9 = page.Width * .90;
      var h = 50.5;
      var pen0 = new XPen(lite, 1);
      var pen1 = new XPen(lite, .2);
      var pen2 = new XPen(dark, 2);
      var rds = 5;
      gfx.DrawEllipse(pen0, col1 + 01, h, rds, rds);
      gfx.DrawEllipse(pen0, col1 + 21, h, rds, rds);
      gfx.DrawEllipse(pen0, col1 + 41, h, rds, rds);
      gfx.DrawString("INVOICE", new XFont(fontname, 36, XFontStyleEx.Regular), liteb, col9, 40, XStringFormats.TopRight);
      h = 85d;
      gfx.DrawLine(litep, col1, h, col9, h);

      var fontCoNm = new XFont(fontname, 16, XFontStyleEx.Regular);
      var fontLabB = new XFont(fontname, 12, XFontStyleEx.Bold | XFontStyleEx.Underline);
      var fontLabl = new XFont(fontname, 11, XFontStyleEx.Regular);
      var fontValu = new XFont(fontname, 11, XFontStyleEx.Bold);
      var fontNndl = new XFont(fontname, 11, XFontStyleEx.Underline);

      h = page.Height * .150;
      h += lineHeight2; gfx.DrawString("     Invoice #    ", fontLabl, liteb, col2, h, XStringFormats.BaseLineRight); gfx.DrawString($"{invcNumber}          ", fontValu, darkb, col2, h, XStringFormats.BaseLineLeft);
      h += lineHeight2; gfx.DrawString("          Date    ", fontLabl, liteb, col2, h, XStringFormats.BaseLineRight); gfx.DrawString($"{invcDate:dd-MMM-yyyy}", fontValu, darkb, col2, h, XStringFormats.BaseLineLeft);
      h += lineHeight2; gfx.DrawString("For the period    ", fontLabl, liteb, col2, h, XStringFormats.BaseLineRight); gfx.DrawString($"{pyPeriod}            ", fontValu, darkb, col2, h, XStringFormats.BaseLineLeft);

      h = page.Height * .320;
      gfx.DrawString(coName1, fontCoNm, darkb, col1, h, XStringFormats.BaseLineLeft);
      gfx.DrawString(coName2, fontCoNm, darkb, col2, h, XStringFormats.BaseLineLeft);
      gfx.DrawString("Invoice to    ", fontLabl, liteb, col2, h, XStringFormats.BaseLineRight);

      printLines(coAdrs1, lineHeight1, fontname, darkb, gfx, col1, h);
      printLines(coAdrs2, lineHeight1, fontname, darkb, gfx, col2, h);

      h = page.Height * .540;
      gfx.DrawString("Description                                                          Hours             Rate", fontLabl, liteb, col1, h, XStringFormats.BaseLineLeft);
      gfx.DrawString("Amount", fontLabl, liteb, col9, h, XStringFormats.BaseLineRight);
      h += lineHeight2;
      gfx.DrawString(jobDescrn, fontLabl, darkb, col1, h, XStringFormats.BaseLineLeft);
      gfx.DrawString(payPrdHrs, fontValu, darkb, page.Width * .52, h, XStringFormats.BaseLineRight);
      gfx.DrawString(ratePerHr, fontValu, darkb, page.Width * .62, h, XStringFormats.BaseLineRight);
      gfx.DrawString(total_ttl, fontValu, darkb, col9, h, XStringFormats.BaseLineRight);

      h += lineHeight2; gfx.DrawString("  Subtotal ", fontLabl, liteb, colM, h, XStringFormats.BaseLineRight); gfx.DrawString(sub_total, fontValu, darkb, col9, h, XStringFormats.BaseLineRight);
      h += lineHeight2; gfx.DrawString("       HST ", fontLabl, liteb, colM, h, XStringFormats.BaseLineRight); gfx.DrawString(taxRatePc, fontValu, darkb, col9, h, XStringFormats.BaseLineRight);
      h += lineHeight2; gfx.DrawString(" Sales Tax ", fontLabl, liteb, colM, h, XStringFormats.BaseLineRight); gfx.DrawString(salesTaxM, fontValu, darkb, col9, h, XStringFormats.BaseLineRight);
      h += lineHeight2; gfx.DrawString("     Other ", fontLabl, liteb, colM, h, XStringFormats.BaseLineRight);
      h += lineHeight2; gfx.DrawString("     Total ", fontLabl, liteb, colM, h, XStringFormats.BaseLineRight); gfx.DrawString(grandtotl, fontValu, darkb, col9, h, XStringFormats.BaseLineRight);

      h = page.Height * .52; gfx.DrawLine(pen1, col1, h, col9, h);
      h += lineHeight2; gfx.DrawLine(pen1, col1, h, col9, h);
      h += lineHeight2; gfx.DrawLine(pen1, col1, h, col9, h);
      h += lineHeight2;
      h += lineHeight2;
      h += lineHeight2;
      h += lineHeight2;
      h += lineHeight2; gfx.DrawLine(pen1, col1, h, col9, h);
      h += lineHeight2;
      h += lineHeight2;

      if (_includeBank)
      {
        gfx.DrawString("Bank Account Details", fontLabB, liteb, colA, h, XStringFormats.BaseLineLeft);                //gfx.DrawString($"{invcNumber}          ", fontValu, darkb, col2, h, XStringFormats.BaseLineLeft);
        h += lineHeight2; gfx.DrawString("              Bank", fontLabl, liteb, colB, h, XStringFormats.BaseLineRight); gfx.DrawString("    TD Canada Trust", fontValu, darkb, colB, h, XStringFormats.BaseLineLeft);
        h += lineHeight2; gfx.DrawString("    Transit Number", fontLabl, liteb, colB, h, XStringFormats.BaseLineRight); gfx.DrawString("    03212          ", fontValu, darkb, colB, h, XStringFormats.BaseLineLeft);
        h += lineHeight2; gfx.DrawString("Institution Number", fontLabl, liteb, colB, h, XStringFormats.BaseLineRight); gfx.DrawString("    004            ", fontValu, darkb, colB, h, XStringFormats.BaseLineLeft);
        h += lineHeight2; gfx.DrawString("    Account Number", fontLabl, liteb, colB, h, XStringFormats.BaseLineRight); gfx.DrawString("    5999741        ", fontValu, darkb, colB, h, XStringFormats.BaseLineLeft);
      }
      else
      {
        h += lineHeight2; gfx.DrawString("                  ", fontLabl, liteb, colB, h, XStringFormats.BaseLineRight); gfx.DrawString("               ", fontValu, darkb, colB, h, XStringFormats.BaseLineLeft);
        h += lineHeight2; gfx.DrawString("                  ", fontLabl, liteb, colB, h, XStringFormats.BaseLineRight); gfx.DrawString("               ", fontValu, darkb, colB, h, XStringFormats.BaseLineLeft);
        h += lineHeight2; gfx.DrawString("                  ", fontLabl, liteb, colB, h, XStringFormats.BaseLineRight); gfx.DrawString("               ", fontValu, darkb, colB, h, XStringFormats.BaseLineLeft);
      }

      h += lineHeight2;
      h += lineHeight2; gfx.DrawLine(pen1, col1, h, col9, h);
    }
    catch (Exception ex) { _ = ex.Log(); }
  }

  public void SaveAndViewPdfFile(string pdfFilename)
  {
    try
    {
      _pdfDoc.Save(pdfFilename);
      _ = Process.Start(@"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe", $"\"{pdfFilename}\"");
    }
    catch (Exception ex2) { _ = ex2.Log(pdfFilename); throw; }
  }

  static void printLines(string coAdrs1, int lineHeight1, string fontname, XSolidBrush dark, XGraphics gfx, double x, double h)
  {
    var font = new XFont(fontname, 11, XFontStyleEx.Regular);
    foreach (var line in coAdrs1.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
    {
      h += lineHeight1; gfx.DrawString(line, font, dark, x, h, XStringFormats.BaseLineLeft);
    }
  }

  // Custom font resolver class
  public class MyFontResolver : IFontResolver
  {
    public byte[] GetFont(string faceName)
    {
      using (var ms = new MemoryStream())
      {
        // Replace the path with the actual path to the Arial font file
        using (var fs = File.OpenRead(@"C:\Windows\Fonts\segoeui.ttf"))
        {
          fs.CopyTo(ms);
        }
        return ms.ToArray();
      }
    }

    public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
      // Return the font family name and style
      return new FontResolverInfo("Arial", isBold, isItalic);
    }
  }
}
/*
                Bank  TD Canada Trust
         Branch code  03212
      Account Number  5999741
  Designation Number  7919
*/
