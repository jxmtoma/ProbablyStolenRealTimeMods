using RealTimeShop;
using System.Reflection;
using System.Runtime.Loader;

internal static class RecoveryChecks
{
    internal static void Run(Action<bool,string> check)
    {
        var bark=new CustomerBarkState();bark.Observe("day",1,false,0);
        check(bark.Warn("day",1,"Ada","en",true),"warning belongs to current customer");
        bark.Observe("day",1,false,5,false);
        check(bark.Text.Length>0,"typing time does not consume readable lifetime");
        bark.Observe("day",1,false,5,true);check(bark.Text.Length>0,"speech readable for six seconds after typing");
        bark.Observe("day",1,false,1,true);check(bark.Text=="","six readable seconds expire speech");
        check(!bark.Warn("day",1,"Ada","en",true),"warning does not repeat for same visit");
        bark.Observe("day",2,true,0);check(bark.Warn("day",2,"Bo","en",false)&&bark.Text=="","obscured warning queues");
        bark.Observe("day",2,false,1);check(bark.Text.Length>0,"queued warning becomes readable");
        bark.Observe("day",2,false,0,true,false);check(bark.Owner==0,"departed visual removes speech immediately");
        bark.Observe("day",3,false,0);bark.Warn("day",3,"Cy","en",true);
        bark.Observe("day",3,true,15,false);check(bark.Owner==0,"wall lifetime bounds obscured speech");
        var shutter=new ClosedShutterLock();shutter.Observe("d",0,1,true);
        check(!shutter.Blocks("d",1)&&shutter.Blocks("d",2)&&shutter.Blocks("other",1),"only observed same-day closure lock is exempt");
        shutter.Clear();check(shutter.Blocks("d",1),"closure exemption clears with session");
        check(!SleepHintPolicy.Blocks("tutorialSleep",0,true)&&SleepHintPolicy.Blocks("tutorialSleep",0,false),"sleep hint exemption requires native readiness");
        check(!SleepHintPolicy.Blocks("tutorialOpenShutterDoor",13,true)&&SleepHintPolicy.Blocks("tutorialOpenShutterDoor",12,true),"only native end-of-day shutter tutorial stage is exempt");
        var facts=new[]{new TableItemFacts(false,true,true,true,true)};
        check(TableOwnershipPolicy.Assess(facts,true).Safe,"proven nested unsold goods can use native cleanup");
        check(!TableOwnershipPolicy.Assess(facts.Select(x=>x with {NestedCustomerGoodsVerified=false}),true).Safe,"unproven nested goods remain protected");
        var ledger=new CustomerGoodsLedger();ledger.Record(1,2,3);ledger.Forget(2);check(!ledger.Known(1,2,3),"purchase invalidates unsold provenance");
        var typing=new CustomerSpeechTyping();typing.Start("A\U0001F600e\u0301",.03,.15,.3,1,true,false);typing.Advance(1);
        check(typing.Complete&&typing.VisibleElements==3&&typing.VisibleCharacters==4,"graphemes reveal atomically while TMP counts Unicode scalars");
        var notices=new NativeNoticeQueue();notices.SetHold("pause");check(notices.Count==0,"routine hold is silent by default");
        notices.SetHold("unsafe closing",true);check(notices.Count==1,"actionable closing hold may announce");
        notices.SetHold("");check(notices.Count==0,"resume clears actionable hold without debug toast");
        string? reference=Environment.GetEnvironmentVariable("RC22_PURE_REFERENCE");
        if(!string.IsNullOrEmpty(reference))Differential(reference,check);
    }

    private static void Differential(string path,Action<bool,string> check)
    {
        var assembly=AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.GetFullPath(path));
        const BindingFlags flags=BindingFlags.Instance|BindingFlags.Static|BindingFlags.NonPublic|BindingFlags.Public;
        Type TypeFor(string name)=>assembly.GetType("RealTimeShop."+name,true)!;
        object New(string name)=>Activator.CreateInstance(TypeFor(name),true)!;
        object? Call(object value,string name,params object?[] args)=>value.GetType().GetMethod(name,flags)!.Invoke(value,args);
        object? Static(string type,string name,params object?[] args)=>TypeFor(type).GetMethod(name,flags)!.Invoke(null,args);
        object? Get(object value,string name)=>value.GetType().GetProperty(name,flags)!.GetValue(value);
        var expected=New("CustomerBarkState");var actual=new CustomerBarkState();
        var random=new Random(220018);string[] locales={"en","fr","de-DE","es","pt_BR","ru","ja","ko","zh-CN","unknown"};
        double[] seconds={0,.001,.25,1,5,6,15,-1,double.NaN,double.PositiveInfinity};
        for(int i=0;i<12000;i++)
        {
            string session="d"+random.Next(3);long visit=random.Next(5);bool safe=random.Next(2)==0;
            string name=i%7==0?" ":"Customer "+visit,locale=locales[random.Next(locales.Length)];
            switch(random.Next(5))
            {
                case 0:double delta=seconds[random.Next(seconds.Length)];bool presented=random.Next(2)==0,visible=random.Next(3)!=0;
                    actual.Observe(session,visit,!safe,delta,presented,visible);Call(expected,"Observe",session,visit,!safe,delta,presented,visible);break;
                case 1:check(actual.Warn(session,visit,name,locale,safe)==(bool)Call(expected,"Warn",session,visit,name,locale,safe)!,"RC22 differential warning return");break;
                case 2:long current=random.Next(3);bool confirmed=random.Next(2)==0;
                    check(actual.Departure(session,visit,current,name,locale,confirmed,safe)==(bool)Call(expected,"Departure",session,visit,current,name,locale,confirmed,safe)!,"RC22 differential departure return");break;
                case 3:actual.Hide();Call(expected,"Hide");break;
                default:actual.Reset();Call(expected,"Reset");break;
            }
            foreach(string property in new[]{"Session","Visit","Owner","Speaker","Line","Text","Departing"})
                check(Equals(Get(actual,property),Get(expected,property)),"RC22 differential speech state "+property);
        }
        foreach(string locale in locales)
        {
            check(BarkText.Warning(locale)==(string)Static("BarkText","Warning",locale)!,"RC22 warning localization "+locale);
            check(BarkText.Departure(locale)==(string)Static("BarkText","Departure",locale)!,"RC22 departure localization "+locale);
        }
        var oldTyping=New("CustomerSpeechTyping");var newTyping=new CustomerSpeechTyping();
        foreach(string text in new[]{"", "Hello, world...?!", "A\U0001F600e\u0301\r\nBye", "你好，世界！", "a\n\nb"})
        foreach(double delay in new[]{0,.03,double.NaN,-1,double.PositiveInfinity})
        foreach(bool punctuation in new[]{true,false})foreach(bool newline in new[]{true,false})
        {
            newTyping.Start(text,delay,.15,.3,2,punctuation,newline);Call(oldTyping,"Start",text,delay,.15,.3,2d,punctuation,newline);
            for(int n=0;n<50;n++)
            {
                double delta=seconds[random.Next(seconds.Length)];
                check(newTyping.Advance(delta)==(bool)Call(oldTyping,"Advance",delta)!,"RC22 differential typing advance");
                foreach(string property in new[]{"Complete","VisibleCharacters","VisibleElements"})
                    check(Equals(Get(newTyping,property),Get(oldTyping,property)),"RC22 differential typing "+property);
            }
        }
        var oldLock=New("ClosedShutterLock");var newLock=new ClosedShutterLock();
        for(int n=0;n<5000;n++)
        {
            string day="d"+random.Next(3);int before=random.Next(-1,4),after=random.Next(-1,4);bool closed=random.Next(2)==0;
            newLock.Observe(day,before,after,closed);Call(oldLock,"Observe",day,before,after,closed);
            for(int value=-1;value<=4;value++)check(newLock.Blocks(day,value)==(bool)Call(oldLock,"Blocks",day,value)!,"RC22 differential shutter lock");
        }
        foreach(string hint in new[]{"tutorialSleep","tutorialNightlyService","tutorialOpenShutterDoor","other",""})
        for(int stage=-1;stage<=20;stage++)foreach(bool ready in new[]{true,false})
            check(SleepHintPolicy.Blocks(hint,stage,ready)==(bool)Static("SleepHintPolicy","Blocks",hint,stage,ready)!,"RC22 differential sleep hint");
        foreach(float blend in new[]{float.NaN,float.PositiveInfinity,-1,0,.49f,.5f,1})
            check(CustomerSpeechVisibility.BeforeShadow(blend)==(bool)Static("CustomerSpeechVisibility","BeforeShadow",blend)!,"RC22 differential customer visibility");
        for(int value=0;value<1024;value++)check(BarkUiPolicy.Blocks((ClosingBlocker)value)==(bool)Static("BarkUiPolicy","Blocks",Enum.ToObject(TypeFor("ClosingBlocker"),value))!,"RC22 differential UI blocker mask");
        Console.WriteLine("RC22 pure-policy differential fixture exercised; no game/native code executed.");
    }
}
