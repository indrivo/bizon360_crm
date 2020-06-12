using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Reports.Abstraction.ViewModels.TaskReportViewModels
{
    public class TaskReportViewModel
    {
        public virtual object GroupKeys { get; set; }

        public virtual int Count { get; set; }
    }
}
