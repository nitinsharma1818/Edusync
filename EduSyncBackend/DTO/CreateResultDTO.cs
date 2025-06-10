using System;
using System.ComponentModel.DataAnnotations;

namespace EduSync.DTOs
{
    public class CreateResultDto
    {
        [Required]
        public Guid AssessmentId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public int Score { get; set; }

        [Required]
        public DateTime AttemptDate { get; set; }
    }
}
