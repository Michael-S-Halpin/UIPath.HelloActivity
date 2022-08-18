using System.Activities;
using UiPath.HelloActivity.Activities;
using UIPath.HelloActivity.Enums;

namespace UnitTest;

internal static class Program
{
    public static void Main(string[] args)
    {
        UnitTest();
    }
    private static void UnitTest()
    {    
        var arguments = new Dictionary<string, object>
        {
            { "FilePath", "Path to a file here." },
            { "FlagFlip", false }
        };

        var obj = new TestActivity
        {
            RelativePronoun = TestEnum.This,
            DataType = typeof(string)
        };

        var response = WorkflowInvoker.Invoke(obj, arguments, new TimeSpan(0,0,0, 5));

        var message = response["Message"];
        var flag = response["FlagFlip"];
    }
}