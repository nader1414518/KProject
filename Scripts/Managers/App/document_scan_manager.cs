using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

/* This class is responsible for scanning documents and searching for the similarities online to acquire the importance of it */
public class document_scan_manager : MonoBehaviour
{
    #region Auxilaries
    public static string scan_document(string path)
    {
        using (PdfReader reader = new PdfReader(path))
        {
            StringBuilder text = new StringBuilder();
            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
            }
            return text.ToString();
        }
    }
    #endregion
}
