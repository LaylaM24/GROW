using System;

namespace Grow.Models
{
    public class ErrorViewModel
    {
        public string ID { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(ID);
    }
}
