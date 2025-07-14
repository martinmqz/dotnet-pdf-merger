using System;
using System.IO;
using System.Linq;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace PdfFolderMerger
{
  class Program
  {
    static void Main(string[] args)
    {
      // Example usage:
      // dotnet run "C:\MyFolder" "C:\MyFolder\Combined.pdf"

      if (args.Length != 2)
      {
        Console.WriteLine("Usage: PdfFolderMerger <InputDirectory> <OutputPdfPath>");
        return;
      }

      string inputDirectory = args[0];
      string outputPdfPath = args[1];

      if (!Directory.Exists(inputDirectory))
      {
        Console.WriteLine($"Error: Directory does not exist: {inputDirectory}");
        return;
      }

      string[] pdfFiles = Directory
          .EnumerateFiles(inputDirectory, "*.pdf", SearchOption.AllDirectories)
          .OrderBy(f => f)
          .ToArray();

      if (pdfFiles.Length == 0)
      {
        Console.WriteLine("No PDF files found.");
        return;
      }

      Console.WriteLine($"Found {pdfFiles.Length} PDF files. Combining into {outputPdfPath}...");

      try
      {
        CombinePdfs(pdfFiles, outputPdfPath);
        Console.WriteLine($"Success! Combined PDF saved to: {outputPdfPath}");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error while combining PDFs: {ex.Message}");
      }
    }

    static void CombinePdfs(string[] pdfFiles, string outputFilePath)
    {
      using (PdfDocument outputDocument = new PdfDocument())
      {
        foreach (string pdfFile in pdfFiles)
        {
          Console.WriteLine($"Adding: {pdfFile}");

          using (PdfDocument inputDocument = PdfReader.Open(pdfFile, PdfDocumentOpenMode.Import))
          {
            for (int idx = 0; idx < inputDocument.PageCount; idx++)
            {
              outputDocument.AddPage(inputDocument.Pages[idx]);
            }
          }
        }

        outputDocument.Save(outputFilePath);
      }
    }
  }
}
