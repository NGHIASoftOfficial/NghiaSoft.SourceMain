using System.ComponentModel.DataAnnotations;

namespace NghiaSoft.FileServerAPI.Data.Entities;

public class AppFileInfo
{
    /// <summary>
    /// Mã file
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Thẻ của file, phân cách bằng dấu phẩy
    /// </summary>
    public string? FileTags { get; set; }

    /// <summary>
    /// The real file name
    /// </summary>
    public string FileName { get; set; }
    
    /// <summary>
    /// Description of file
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The url origin where file uploaded
    /// </summary>
    public string OriginPath { get; set; }

    /// <summary>
    /// Ngày upload
    /// </summary>
    public DateTime CreationTime { get; set; }
    
    /// <summary>
    /// Ngày sửa cuối
    /// </summary>
    public DateTime? ModificationTime { get; set; }

    /// <summary>
    /// Yêu cầu phải đăng nhập mới có thể tải
    /// </summary>
    public bool IsAuthenticateRequired { get; set; }

    /// <summary>
    /// Yêu cầu phải là tác giả mới đưược thao tác
    /// </summary>
    public bool IsOwnedVisibleOnly { get; set; }

    /// <summary>
    /// Tác giả
    /// </summary>
    public string? UploaderId { get; set; }
    public string? UploaderUserName { get; set; }
}