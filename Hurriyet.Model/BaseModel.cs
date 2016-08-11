using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hurriyet.Model
{
    [Serializable]
    public abstract class BaseModel
    {

        /// <summary>
        /// obje uniq rakam
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// silindi bilgisi
        /// </summary>
        [Column(TypeName = "bit")]
        public bool Deleted { get; set; }

        /// <summary>
        /// olusturulma tarihi
        /// </summary>
        [Column(TypeName = "datetime2")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// guncellenme tarihi
        /// </summary>
        [Column(TypeName = "datetime2")]
        public DateTime UpdatedDate { get; set; }
    }

}
