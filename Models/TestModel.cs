using ORM.Attribute;
using System;
using System.Collections.Generic;
using System.Text;

namespace ORM.Models
{
    [Table("Test")]
    public class TestModel
    {
        [Column("ID")]
        public int TaskID { get; set; }

        [Column("DisplayName")]
        public string Name { get; set; }
    }
}
