using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AccessData.Entities
{
    [Table("MoveDetail ", Schema = "dbo")]
    public class MoveDetail
    {
        [Key]
        public int IdMoveDetail { get; set; }
        public int IdMove { get; set; }
        public int Value { get; set; }

        public int Position { get; set; }

        [ForeignKey("IdMove")]
        public Move Move { get; set; }

    }
}
