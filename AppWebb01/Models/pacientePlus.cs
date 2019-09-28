namespace AppWebb01.Models
{
    using System;
    using System.Collections.Generic;
    using AppWebb01.Models;

    public partial class pacientePlus
    {
        

        public int paciente_id { get; set; }
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public string direccion { get; set; }
        public string estado { get; set; }
        public string ciudad { get; set; }
        public Nullable<System.DateTime> fecha_nac { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
        public Nullable<int> edad { get; set; }
        public int numtrat { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<reservacion> reservacion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tratamiento> tratamiento { get; set; }
    }
}