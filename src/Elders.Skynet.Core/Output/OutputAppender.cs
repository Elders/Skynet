using log4net.Appender;
using log4net.Core;

namespace Elders.Skynet.Core.Output
{
    public class OutputAppender : AppenderSkeleton
    {
        IOutput output;

        public OutputAppender(IOutput output)
        {
            this.output = output;
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            output.Write(loggingEvent.MessageObject.ToString());
            if (loggingEvent.ExceptionObject != null)
                output.Write(loggingEvent.GetExceptionString());
        }
    }
}