using SyncWithSerilog.Logging.Events;
using System;

namespace SyncWithSerilog.Logging.FormatProviders
{
    public class EventFormatter : IFormatProvider, ICustomFormatter
    {
        public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            return arg switch
            {
                Event @event => format switch
                {
                    "l" => Format(@event),
                    "L" => Format(@event),
                    _ => @event.ToString(format)
                },

                IFormattable formattable
                    => formattable.ToString(format, formatProvider),

                _ => arg?.ToString() ?? string.Empty
            };

            static string Format(Event @event) => @event switch
            {
                Event.SyncStarted => "Synchronization started",
                Event.UploadSucceeded => "Upload succeeded",
                Event.UploadFailed => "Upload failed",
                Event.SyncEnded => "Synchronization ended",
                _ => @event.ToString()
            };
        }

        public object? GetFormat(Type? formatType)
            => (formatType == typeof(ICustomFormatter)) ? this : null;
    }
}
