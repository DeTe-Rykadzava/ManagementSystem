using System;
using System.Collections.Generic;

namespace Database.Data;

public partial class Report
{
    internal Report()
    {
        
    }
    
    public int Id { get; set; }

    public DateTime CreateDate { get; set; }

    public int? UserId { get; set; }

    public string Info { get; set; } = null!;

    public int TypeId { get; set; }

    public virtual ReportType Type { get; set; } = null!;

    public virtual User? User { get; set; }
}
