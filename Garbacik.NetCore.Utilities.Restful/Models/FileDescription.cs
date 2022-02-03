using Microsoft.AspNetCore.Http;
using System.IO;

namespace Garbacik.NetCore.Utilities.Restful.Models;

public class FileDescription
{
    public byte[] FileBytes { get; private set; }
    public string FileName { get; private set; }

    public FileDescription(byte[] fileBytes, string fileName)
    {
        FileBytes = fileBytes;
        FileName = fileName;
    }

    public FileDescription(IFormFile formFile)
    {
        FileName = formFile.FileName;

        if (formFile.Length > 0)
        {
            using (var ms = new MemoryStream())
            {
                formFile.CopyTo(ms);
                FileBytes = ms.ToArray();
            }
        }
    }
}