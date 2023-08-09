#pragma warning disable CS8618


using System.ComponentModel.DataAnnotations;
namespace EFLectureNotes.Models;

public class Post
{
    [Key]
    //! PostId =========================
    public int PostId {get; set;}


    //! Topic ========================= 
    [Required]
    [MinLength(2, ErrorMessage = "Must be 2 characters long")]
    [MaxLength(40, ErrorMessage = "No longer than 40 characters long")]
    public string Topic {get; set;}
    

    //! Body ======================== 
    [Required]
    [MinLength(2, ErrorMessage = "Must be 2 characters long")]
    public string Body {get; set;}


    //! ImgaeUrl ======================== 
    [Display(Name = "Enter URL here: ")]
    public string ImageUrl {get; set;}


    //! CreatedAt ======================== 
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    


    //! UpdatedAt ======================== 
    public DateTime UpdatedAt { get; set; } = DateTime.Now;



}
