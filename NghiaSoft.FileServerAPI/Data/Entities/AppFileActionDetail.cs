using System.ComponentModel.DataAnnotations;

namespace NghiaSoft.FileServerAPI.Data.Entities;

public class AppFileActionDetail
{
    /// <summary>
    /// Mã hành động được ghi lại
    /// </summary>
    [Key] public Guid Id { get; set; }

    /// <summary>
    /// Mã của file
    /// </summary>
    public Guid FileId { get; set; }

    /// <summary>
    /// Loại hành động
    /// U: Upload
    /// D: Download
    /// I: Xem thông tin
    /// </summary>
    public string? ActionType { get; set; }

    /// <summary>
    /// Ngày upload
    /// </summary>
    public DateTime CreationTime { get; set; }
    
    /// <summary>
    /// Tác giả
    /// </summary>
    public string? ActorId { get; set; }
    public string? ActorUserName { get; set; }
}