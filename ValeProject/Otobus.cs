//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ValeProject
{
    using System;
    using System.Collections.Generic;
    
    public partial class Otobus
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Otobus()
        {
            this.Sefer = new HashSet<Sefer>();
        }
    
        public int OtobusID { get; set; }
        public string Plaka { get; set; }
        public Nullable<int> KoltukSayisi { get; set; }
        public string Marka { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sefer> Sefer { get; set; }
    }
}
