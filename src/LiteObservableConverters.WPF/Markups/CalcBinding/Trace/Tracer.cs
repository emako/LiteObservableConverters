using System.Diagnostics;

namespace LiteObservableConverters.CalcBinding.Trace;

public sealed class Tracer(TraceComponent component)
{
    static Tracer()
    {
        _sourceSwitch = new SourceSwitch("CalcBindingTraceLevel", $"{SourceLevels.Off}");
        _traceSource = new TraceSource("CalcBindingTracer")
        {
            Switch = _sourceSwitch
        };
    }

    [Conditional("DEBUG")]
    public void TraceDebug(string str)
    {
        Trace(TraceEventType.Verbose, str);
    }

    public void TraceInformation(string str)
    {
        Trace(TraceEventType.Information, str);
    }

    public void TraceError(string str)
    {
        Trace(TraceEventType.Error, str);
    }

    public static TraceListenerCollection Listeners => _traceSource.Listeners;

    private void Trace(TraceEventType level, string str)
    {
        if (_sourceSwitch.ShouldTrace(level))
        {
            _traceSource.TraceData(level, 0, $"{_componentName}: {str}");
        }
    }

    private readonly string _componentName = component.ToString();
    private static readonly SourceSwitch _sourceSwitch;
    private static readonly TraceSource _traceSource;
}
