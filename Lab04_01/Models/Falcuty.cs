namespace Lab04_01.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Falcuty")]
    public partial class Falcuty
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Falcuty()
        {
            Student = new HashSet<Student>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FalcutyID { get; set; }

        [Required]
        [StringLength(200)]
        public string FalcutyName { get; set; }

        public int? TotalProfessor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Student> Student { get; set; }
    }
}
