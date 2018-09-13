using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AgentMarket.Models
{
    public class AddItemViewModel
    {
        public int Id { get; set; }
        public string Thumbnail { get; set; }
        //[Required, FileExtensions(Extensions = "jpg, png, gif", ErrorMessage = "Specify a valid image file.")]
        public HttpPostedFileBase Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public System.DateTime PostDate { get; set; }
        public string Content { get; set; }
        public short DynamicMenuItem_Id { get; set; }
    }

    public class AddProductViewModel
    {
        public int Id { get; set; }
        public int PartnerId { get; set; }
        public bool FeaturedDeal { get; set; }
        public bool IsActive { get; set; }
        public string Thumbnail { get; set; }
        public string ProductName { get; set; }
        public HttpPostedFileBase Image { get; set; }
        public HttpPostedFileBase Image2 { get; set; }
        public HttpPostedFileBase Image3 { get; set; }
        [DisplayName("Hook Description")]
        public string DescriptionHook1 { get; set; }
        [DisplayName("Main Description")]
        public string MainDescription { get; set; }
        public string BulletPoint1 { get; set; }
        public string BulletPoint2 { get; set; }
        public string BulletPoint3 { get; set; }
        public string BulletPoint4 { get; set; }
        public string BulletPoint5 { get; set; }
        public string BulletPoint6 { get; set; }
        public string DescriptionHook2 { get; set; }
        public string DescriptionLong { get; set; }
        public float OriginalPrice { get; set; }
        public float SalePrice { get; set; }
        public System.DateTime PostDate { get; set; }
        public short DynamicMenuItem_Id { get; set; }
    }
}