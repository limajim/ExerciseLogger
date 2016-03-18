using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace LoggingAPI.Models
{
    public class ExerciseSession
    {
        public int Id { get; set; }
        [Required()]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }

        [ForeignKey("ExerciseType")]
        public int ExerciseSessionId { get; set; }

        public virtual ExerciseType ExerciseType { get; set; }


    }
}