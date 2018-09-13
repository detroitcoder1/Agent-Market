using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgentMarket.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    [Table("Comments")]
    public class Comment
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("User")]
        public string User_Id { get; set; }
        [ForeignKey("Item")]
        public int Item_Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public System.DateTime PostDate { get; set; }
        public string Text { get; set; }

        public virtual Item Item { get; set; }
        public virtual ApplicationUser User { get; set; }
    }

    [Table("Items")]
    public class Item
    {
        public Item()
        {
            this.Comments = new HashSet<Comment>();
        }

        [Key]
        public int Id { get; set; }
        public string Thumbnail { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public System.DateTime PostDate { get; set; }
        public string Content { get; set; }
        [ForeignKey("DynamicMenuItem")]
        public short DynamicMenuItem_Id { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual DynamicMenuItem DynamicMenuItem { get; set; }
    }


    [Table("Products")]
    public class Product
    {
     [Key]
    public int Id { get; set; }
     public int PartnerId { get; set; }
    public bool FeaturedDeal { get; set; }
    public bool IsActive { get; set; }
    public string Thumbnail { get; set; }
    public string Image { get; set; }
    public string Image2 { get; set; }
    public string Image3 { get; set; }
    public string ProductName { get; set; }
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
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public System.DateTime PostDate { get; set; }
    [ForeignKey("DynamicMenuItem")]
    public short DynamicMenuItem_Id { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
    public virtual DynamicMenuItem DynamicMenuItem { get; set; }
}

    [Table("Languages")]
    public class Language
    {
        public Language()
        {
            this.DynamicMenuItems = new HashSet<DynamicMenuItem>();
            this.StaticMenuItems = new HashSet<StaticMenuItem>();
        }

        [Key]
        public short Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<DynamicMenuItem> DynamicMenuItems { get; set; }
        public virtual ICollection<StaticMenuItem> StaticMenuItems { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }

    [Table("DynamicMenuItems")]
    public class DynamicMenuItem
    {
        public DynamicMenuItem()
        {
            this.Items = new HashSet<Item>();
        }

        [Key]
        public short Id { get; set; }
        public string Title { get; set; }
        public short OrderNo { get; set; }
        [ForeignKey("Language")]
        public short Language_Id { get; set; }

        public virtual Language Language { get; set; }
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<CSSMappingEntry> CSSMappings { get; set; }
    }

    [Table("StaticMenuItems")]
    public class StaticMenuItem
    {
        [Key]
        public short Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public short OrderNo { get; set; }
        [ForeignKey("Language")]
        public short Language_Id { get; set; }

        public virtual Language Language { get; set; }
    }
}