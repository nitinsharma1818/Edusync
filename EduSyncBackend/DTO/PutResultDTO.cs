using System;

namespace EduSync.DTOs
{
    public class PutResultDto
    {
        public Guid ResultId { get; set; }
        public Guid AssessmentId { get; set; }
        public Guid UserId { get; set; }
        public int Score { get; set; }       // int ki tarah rakha (model ke mutabiq)
        public DateTime AttemptDate { get; set; }
    }
}
