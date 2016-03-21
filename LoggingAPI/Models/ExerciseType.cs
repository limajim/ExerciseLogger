

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoggingAPI.Models
{
    public class ExerciseType
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        [Required()]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }

        public IEnumerable<ExerciseSession> ExerciseSessions { set; get; }

    }
}