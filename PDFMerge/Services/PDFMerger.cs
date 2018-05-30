using PDFMerge.Model;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFMerge.Services
{
    public class PDFMerger
    {
        public class Results
        {
            public bool Success { get; set; } = true;
            public string ErrorMessage { get; set; } = String.Empty;
        }

        public Results MergePDFs(List<PDFFile> files, string OutputPath)
        {
            try
            {
                using (PdfDocument output = new PdfDocument())
                {
                    foreach (var pdf in files)
                    {
                        using (PdfDocument inDocument = PdfReader.Open(pdf.Path, PdfDocumentOpenMode.Import))
                        {
                            for (int i = 0; i < inDocument.PageCount; i++)
                            {
                                output.AddPage(inDocument.Pages[i]);
                            }
                        }
                    }

                    output.Save(OutputPath);
                }

                return new Results { Success = true };
            }
            catch (Exception e)
            {
                return new Results { Success = false, ErrorMessage = e.Message };
            }
        }

    }
}
