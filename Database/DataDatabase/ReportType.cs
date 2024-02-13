using System;
using System.Collections.Generic;

namespace Database.DataDatabase;

public partial class ReportType
{
    internal ReportType()
    {
        
    }

    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
}
