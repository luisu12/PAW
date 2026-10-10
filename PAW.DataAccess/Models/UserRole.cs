using System;
using System.Collections.Generic;

namespace PAW.DataAccess.Models;

public partial class UserRole
{
    // Id can be NULL in database for some existing rows; keep nullable to avoid projection errors.
    public decimal? Id { get; set; }

    public decimal? RoldId { get; set; }

    public decimal? UserId { get; set; }
}
