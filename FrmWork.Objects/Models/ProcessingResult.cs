using System.Collections.Generic;

namespace FrmWork.Objects.Models
{
    public class ProcessingResult
    {
        public string Message { get; set; }
        public MessageType MessageType { get; set; } = MessageType.Success;
        public int Timeout { get; set; }
    }
    public class ProcessingResult<TMessage, TMessageType>
    {
        public TMessage Message { get; set; }
        public TMessageType MessageType { get; set; }
        public int Timeout { get; set; }
    }
}