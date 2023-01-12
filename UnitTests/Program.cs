using System.Activities;
using UiPath.HelloActivity.Activities;
using UiPath.HelloActivity.Enums;

namespace UnitTest;

internal static class Program
{
    public static void Main(string[] args)
    {
        UnitTest();
    }
    private static void UnitTest()
    {    
        
        #region Your activity properties get set in this section.
        
        // Anything that is a proper InArgument, OutArgument, or InOutArgument
        // get set here as part of a data dictionary.
        var arguments = new Dictionary<string, object>
        {
            { "FilePath", "Path to a file here." },
            { "FlagFlip", false }
        };

        // Everything else like enums and types get set here and pass directly into the activity
        // and do not need to be retrieved via the .Get(CancellaitonToken) like argument values.
        var obj = new TestActivity(true)
        {
            RelativePronoun = TestEnum.This,
            DataType = typeof(string)
        };
        
        #endregion

        // This line is where ExecuteAsync gets invoked.
        var response = WorkflowInvoker.Invoke(obj, arguments, new TimeSpan(0,0,0, 5));

        // These are where your OutArguments and InOutArgument values are retrieved. 
        var message = response["Message"];
        var flag = response["FlagFlip"];
    }
}