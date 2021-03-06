//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TriviaTraverseApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class VGame
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VGame()
        {
            this.VGameCategories = new HashSet<VGameCategory>();
            this.VGamePlayers = new HashSet<VGamePlayer>();
            this.VGamePlayerSectionQuestions = new HashSet<VGamePlayerSectionQuestion>();
        }
    
        public int VGameId { get; set; }
        public string GameName { get; set; }
        public int GameTypeId { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public int PlayerMax { get; set; }
        public int StepCap { get; set; }
        public bool IsPrivate { get; set; }
        public int GameLength { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
        public bool Deleted { get; set; }
    
        public virtual GameType GameType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VGameCategory> VGameCategories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VGamePlayer> VGamePlayers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VGamePlayerSectionQuestion> VGamePlayerSectionQuestions { get; set; }
    }
}
