using System.Text.RegularExpressions;

namespace NghiaSoft.FileServerAPI.Common;

public static class FileProcessHelper
{
    public static string NormalizeFileName(string fileName)
    {
        // Loại bỏ các ký tự không hợp lệ từ tên tệp
        var cleanFileName = Path.GetFileNameWithoutExtension(fileName);
        cleanFileName =
            Regex.Replace(cleanFileName, @"[^\w\-]", ""); // Loại bỏ các ký tự không phải chữ, số, gạch ngang

        // Lấy phần mở rộng của tệp
        var fileExtension = Path.GetExtension(fileName);

        // Tạo tên tệp duy nhất trong thư mục bằng cách thêm số duy nhất vào cuối tên
        var uniqueFileName = $"{cleanFileName}_{DateTime.UtcNow.Ticks}";
        return uniqueFileName + fileExtension;
    }
}