﻿using System;
using System.Collections.Generic;

namespace EduSync.Models;

public partial class Assessment
{
    public Guid AssessmentId { get; set; }

    public Guid? CourseId { get; set; }

    public string? Title { get; set; }

    public string? Questions { get; set; }

    public int? MaxScore { get; set; }
}
