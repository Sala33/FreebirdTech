using System;

namespace FreebirdTech.Models
{
    /// <summary>
    /// This class represents the Error Object <see cref="ErrorViewModel"/> which can be used for exhibiting error messages.
    /// </summary>
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
