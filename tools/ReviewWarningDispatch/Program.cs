using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Text.Json;

if(args.Length!=3)throw new ArgumentException("RC.22 DLL, candidate DLL, report JSON");
using var original=ModuleDefinition.ReadModule(args[0]);
using var candidate=ModuleDefinition.ReadModule(args[1]);
var originalUpdate=original.Types.Single(t=>t.FullName=="RealTimeShop.Plugin").Methods.Single(m=>m.Name=="OnLateUpdate");
var update=candidate.Types.Single(t=>t.FullName=="RealTimeShop.Plugin").Methods.Single(m=>m.Name=="OnLateUpdate");
var policy=candidate.Types.Single(t=>t.FullName=="RealTimeShop.CustomerBarkDispatch").Methods.Single(m=>m.Name=="ShouldWarn");
bool IsCall(Instruction i,string type,string name)=>i.Operand is MethodReference m&&m.DeclaringType.FullName==type&&m.Name==name;
var referenceInstructions=originalUpdate.Body.Instructions;
int referenceWarning=referenceInstructions.Select((i,n)=>(i,n)).Single(x=>IsCall(x.i,"RealTimeShop.Plugin","CustomerWarning")).n;
int referenceStart=-1;
for(int n=referenceWarning-1;n>=0;n--)
 if(referenceInstructions[n].OpCode.Code==Code.Ldc_I4_4&&n+2<referenceWarning&&IsCall(referenceInstructions[n+2],"System.Enum","HasFlag")){referenceStart=n-2;break;}
if(referenceStart<0)throw new Exception("RC.22 warning entry was not recognized");
var instructions=update.Body.Instructions;
var dispatchSites=instructions.Select((i,n)=>(i,n)).Where(x=>IsCall(x.i,"RealTimeShop.CustomerBarkDispatch","ShouldWarn")).ToArray();
if(dispatchSites.Length!=1)throw new Exception("Production dispatcher must occur exactly once");
int dispatch=dispatchSites[0].n;
if(dispatch<3||!IsCall(instructions[dispatch-1],"RealTimeShop.ShopFrame","get_Protected"))throw new Exception("Dispatcher must consume the current frame's protected flag");
var eventLoad=instructions[dispatch-3];
if(eventLoad.Operand is not VariableDefinition variable||variable.VariableType.FullName!="RealTimeShop.ClockEvent")throw new Exception("Dispatcher must consume the clock event local");
if(instructions[dispatch+1].OpCode.Code is not (Code.Brfalse or Code.Brfalse_S)||!IsCall(instructions[dispatch+3],"RealTimeShop.Plugin","CustomerWarning"))throw new Exception("Warning call must be conditional on dispatcher result");
if(instructions[dispatch+1].Operand is not Instruction skip||instructions.IndexOf(skip)<=dispatch+3)throw new Exception("False dispatch must skip the warning call");
if(instructions.Count(i=>IsCall(i,"RealTimeShop.Plugin","CustomerWarning"))!=1)throw new Exception("Unexpected additional production warning call");

bool Evaluate(MethodDefinition method,int start,int? warningIndex,int events,bool protectedFrame)
{
 var code=method.Body.Instructions;var stack=new Stack<object>();int position=start;
 bool Truth(object value)=>value is bool b?b:Convert.ToInt64(value)!=0;
 int Local(Instruction i)=>i.Operand is VariableDefinition v?v.Index:i.OpCode.Code switch{Code.Ldloc_0=>0,Code.Ldloc_1=>1,Code.Ldloc_2=>2,Code.Ldloc_3=>3,_=>throw new Exception("Unknown local")};
 for(int steps=0;steps<1000;steps++)
 {
  if(warningIndex.HasValue&&position>warningIndex.Value)return false;
  var i=code[position++];
  if(warningIndex==position-1)return true;
  switch(i.OpCode.Code)
  {
   case Code.Nop:case Code.Box:break;
   case Code.Ldarg_0:stack.Push(warningIndex.HasValue?new object():events);break;
   case Code.Ldarg_1:stack.Push(protectedFrame);break;
   case Code.Ldloc_0:case Code.Ldloc_1:case Code.Ldloc_2:case Code.Ldloc_3:case Code.Ldloc:case Code.Ldloc_S:
    stack.Push(method.Body.Variables[Local(i)].VariableType.FullName=="RealTimeShop.ClockEvent"?events:new object());break;
   case Code.Ldc_I4_0:stack.Push(0);break;
   case Code.Ldc_I4_1:stack.Push(1);break;
   case Code.Ldc_I4_4:stack.Push(4);break;
   case Code.Ldc_I4_8:stack.Push(8);break;
   case Code.Ldstr:stack.Push((string)i.Operand);break;
   case Code.Ceq:var right=stack.Pop();var left=stack.Pop();stack.Push(Truth(left)==Truth(right));break;
   case Code.Brfalse:case Code.Brfalse_S:if(!Truth(stack.Pop()))position=code.IndexOf((Instruction)i.Operand);break;
   case Code.Brtrue:case Code.Brtrue_S:if(Truth(stack.Pop()))position=code.IndexOf((Instruction)i.Operand);break;
   case Code.Br:case Code.Br_S:position=code.IndexOf((Instruction)i.Operand);break;
   case Code.Ret:return Truth(stack.Pop());
   case Code.Call:case Code.Callvirt:
    var call=(MethodReference)i.Operand;
    if(call.DeclaringType.FullName=="System.Enum"&&call.Name=="HasFlag") {int flag=Convert.ToInt32(stack.Pop());int value=Convert.ToInt32(stack.Pop());stack.Push((value&flag)==flag);}
    else if(call.DeclaringType.FullName=="RealTimeShop.ShopFrame"&&call.Name=="get_Protected"){stack.Pop();stack.Push(protectedFrame);}
    else if(call.DeclaringType.FullName=="MelonLoader.MelonBase"&&call.Name=="get_LoggerInstance"){stack.Pop();stack.Push(new object());}
    else if(call.DeclaringType.FullName=="MelonLoader.MelonLogger/Instance"&&call.Name=="Msg"){stack.Pop();stack.Pop();}
    else throw new Exception("Unsupported call; no execution allowed: "+call.FullName);
    break;
   default:throw new Exception("Unsupported IL: "+i);
  }
 }
 throw new Exception("Predicate step limit exceeded");
}
int cases=0;
for(int events=0;events<256;events++)foreach(bool protectedFrame in new[]{false,true})
{
 bool expected=Evaluate(originalUpdate,referenceStart,referenceWarning,events,protectedFrame);
 bool actual=Evaluate(policy,0,null,events,protectedFrame);
 if(actual!=expected)throw new Exception($"Warning mismatch events={events}, protected={protectedFrame}");
 cases++;
}
var report=new{passed=true,predicateComparisons=cases,productionWiringVerified=true,referenceWarningStart=referenceStart,referenceWarningCall=referenceWarning,methodology="Read-only, bounded interpretation of the original RC.22 warning branch and candidate pure dispatcher, plus compiled Plugin.OnLateUpdate call/branch wiring checks. Supports only boolean/enum operations and substitutes diagnostic logging; no mod, loader or native method executes."};
File.WriteAllText(args[2],JsonSerializer.Serialize(report,new JsonSerializerOptions{WriteIndented=true}));
Console.WriteLine($"PASS: {cases} RC.22 IL warning predicate comparisons; compiled production dispatch wiring verified.");
