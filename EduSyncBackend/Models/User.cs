﻿using System;
using System.Collections.Generic;

namespace EduSync.Models;

public partial class User
{
    public Guid UserId { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Role { get; set; }

    public string? PasswordHash { get; set; }
}
