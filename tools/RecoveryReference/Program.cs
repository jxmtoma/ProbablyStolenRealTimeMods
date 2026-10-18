using Mono.Cecil;
using System.Security.Cryptography;
using System.Text.Json;
if(args.Length!=3)throw new ArgumentException("RC22 DLL, pure reference DLL, audit JSON");
using var assembly=AssemblyDefinition.ReadAssembly(args[0]);
var keep=new HashSet<string>{"CustomerBarkState","BarkText","CustomerSpeechTyping","CustomerSpeechVisibility","ClosedShutterLock","SleepHintPolicy","BarkUiPolicy","ClosingBlocker"};
foreach(var type in assembly.MainModule.Types.ToArray())
    if(type.Name!="<Module>" && (type.Namespace!="RealTimeShop" || !keep.Contains(type.Name)))assembly.MainModule.Types.Remove(type);
assembly.Name.Name="Rc22PureReference";
assembly.CustomAttributes.Clear();assembly.MainModule.CustomAttributes.Clear();
// Fail closed: only original mod policy and BCL calls may remain. No native/loader invocation.
IEnumerable<TypeDefinition> Flatten(IEnumerable<TypeDefinition> list)=>list.SelectMany(t=>new[]{t}.Concat(Flatten(t.NestedTypes)));
foreach(var type in Flatten(assembly.MainModule.Types))
{
type.CustomAttributes.Clear();
foreach(var field in type.Fields)field.CustomAttributes.Clear();
foreach(var property in type.Properties)property.CustomAttributes.Clear();
foreach(var method in type.Methods)
{
method.CustomAttributes.Clear();method.MethodReturnType.CustomAttributes.Clear();
foreach(var parameter in method.Parameters)parameter.CustomAttributes.Clear();
}
foreach(var method in type.Methods.Where(m=>m.HasBody))
foreach(var reference in method.Body.Instructions.Select(i=>i.Operand).OfType<MemberReference>())
{
    var ns=reference is TypeReference tr?tr.Namespace:reference.DeclaringType.Namespace;
    if(ns!="RealTimeShop" && !ns.StartsWith("System"))throw new Exception("Non-policy reference: "+reference.FullName);
}
}
assembly.Write(args[1]);
File.WriteAllText(args[2],JsonSerializer.Serialize(new{sourceSha256=Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(args[0]))),referenceSha256=Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(args[1]))),types=keep.Order(),methodology="Original user-owned mod IL restricted to eight pure managed policy types; native/loader types and assembly attributes removed; native call references rejected. Private test fixture, not a game mod or release payload."},new JsonSerializerOptions{WriteIndented=true}));
Console.WriteLine("Extracted eight pure managed policy types; no native calls.");
