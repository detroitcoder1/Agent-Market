using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgentMarket.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    [Table("CSSMappings")]
    public class CSSMapping
    {
        [Key]
        public int Id { get; set; }
        public string PrettyName { get; set; }
        public string CSSName { get; set; }
        public string CSSProperty { get; set; }
        public string CSSUnit { get; set; }

        public virtual ICollection<CSSMappingEntry> CSSMappingEntries { get; set; }
    }

    [Table("CSSMappingEntries")]
    public class CSSMappingEntry
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("CSSMapping")]
        public int CSSMapping_Id { get; set; }
        public string Value { get; set; }
        [ForeignKey("DynamicMenuItem")]
        public short? DynamicMenuItem_Id { get; set; }

        public virtual DynamicMenuItem DynamicMenuItem { get; set; }
        public virtual CSSMapping CSSMapping { get; set; }
    }
}