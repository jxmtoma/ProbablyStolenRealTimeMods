using RealTimeShop;

internal static class WarningDispatchChecks
{
    internal static void Run(Action<bool,string> check)
    {
        check(!CustomerBarkDispatch.ShouldWarn(ClockEvent.None,false),"no patience event cannot dispatch speech");
        check(CustomerBarkDispatch.ShouldWarn(ClockEvent.PatienceWarning,false),"ordinary warning dispatches");
        check(!CustomerBarkDispatch.ShouldWarn(ClockEvent.PatienceWarning|ClockEvent.Dismiss,false),"dismissal suppresses same-frame warning");
        check(!CustomerBarkDispatch.ShouldWarn(ClockEvent.PatienceWarning,true),"protected frame suppresses warning");
        check(!CustomerBarkDispatch.ShouldWarn(ClockEvent.PatienceWarning|ClockEvent.Dismiss,true),"protected dismissal suppresses warning");
        check(CustomerBarkDispatch.ShouldWarn(ClockEvent.PatienceWarning|ClockEvent.OneHour|ClockEvent.FifteenMinutes,false),"unrelated clock notices preserve warning");

        var clock=new ShopClock(new Timing());clock.StartDay("day");
        var frame=new ShopFrame("ordinary",Ready:true,SafeToDismiss:true);
        clock.Advance(0,frame);
        var events=clock.Advance(120,frame);
        check(events.HasFlag(ClockEvent.PatienceWarning)&&events.HasFlag(ClockEvent.Dismiss),"one long active frame crosses warning and dismissal thresholds");
        var bark=new CustomerBarkState();bark.Observe("day",7,false,0);
        if(CustomerBarkDispatch.ShouldWarn(events,frame.Protected))bark.Warn("day",7,"Ada","en",true);
        check(bark.Owner==0&&bark.Text=="","long-frame dismissal never creates transient warning");
        check(bark.Departure("day",7,0,"Ada","en",true,true),"confirmed dismissal can show departure without a warning first");
        check(bark.Departing&&bark.Owner==7,"departure keeps the departing customer's ownership");
        bark.Observe("day",0,false,3,false);
        check(bark.Line.Length>0,"departure typing time does not consume readable lifetime");
        bark.Observe("day",0,false,5,true);
        check(bark.Line.Length>0,"departure has six readable seconds after presentation");
        bark.Observe("day",0,false,1,true);
        check(bark.Owner==0,"departure expires after full readable duration");

        bark.Reset();bark.Observe("day",8,false,0);
        if(CustomerBarkDispatch.ShouldWarn(ClockEvent.PatienceWarning,true))bark.Warn("day",8,"Bo","en",true);
        check(bark.Owner==0,"protected dispatch has no speech side effect");
        check(bark.Warn("day",8,"Bo","en",true),"suppressed dispatch does not consume per-visit warning eligibility");
        bark.Observe("day",8,false,2,false);
        bark.Observe("day",8,true,1,true);
        bark.Observe("day",8,false,1,true);
        bark.Observe("day",8,false,5,true);
        check(bark.Line.Length>0,"typing and obscured time do not prematurely consume readable warning");
        bark.Observe("day",8,false,1,true);
        check(bark.Owner==0,"resumed readable warning expires normally");

        bark.Reset();bark.Observe("day",9,false,0);bark.Warn("day",9,"Cy","en",true);
        bark.Observe("day",9,false,14.99,false);check(bark.Owner==9,"typing can retain speech below wall-age cap");
        bark.Observe("day",9,false,.02,false);check(bark.Owner==0,"fifteen-second cap bounds speech even while typing");
        bark.Observe("day",10,false,0);bark.Warn("day",10,"Dee","en",true);
        bark.Observe("day",10,false,0,true,false);check(bark.Owner==0,"customer visual departure clears speech before its timer expires");
        check(!bark.Warn("day",10,"Dee","en",true),"visual cleanup does not repeat the same visit warning");
    }
}
