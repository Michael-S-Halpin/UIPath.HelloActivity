using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using System.Activities.Statements;
using System.ComponentModel;
using UIPath.HelloActivity.Activities.Code;
using UiPath.HelloActivity.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Code;
using UiPath.Shared.Activities.Localization;

namespace UiPath.HelloActivity.Activities;

[LocalizedDisplayName(nameof(Resources.TestScope_DisplayName))]
[LocalizedDescription(nameof(Resources.TestScope_Description))]
public class TestScope : ContinuableAsyncNativeActivity
{
    #region Properties - Everything in this section shows up in the 'Properties' panel in UiPath.

    [Browsable(false)]
    public ActivityAction<IObjectContainer> Body { get; set; }

    /// <summary>
    /// If set, continue executing the remaining activities even if the current activity has failed.
    /// </summary>
    [LocalizedCategory(nameof(Resources.Common_Category))]
    [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
    [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
    public override InArgument<bool> ContinueOnError { get; set; }

    [LocalizedCategory(nameof(Resources.Common_Category))]
    [LocalizedDisplayName(nameof(Resources.DebugLog_DisplayName))]
    [LocalizedDescription(nameof(Resources.DebugLog_Description))]
    public InArgument<string> DebugLog { get; set; }

    [LocalizedDisplayName(nameof(Resources.TestScope_TestString_DisplayName))]
    [LocalizedDescription(nameof(Resources.TestScope_TestString_Description))]
    [LocalizedCategory(nameof(Resources.Input_Category))]
    public InArgument<string> TestString { get; set; }

    [LocalizedDisplayName(nameof(Resources.TestScope_OtherString_DisplayName))]
    [LocalizedDescription(nameof(Resources.TestScope_OtherString_Description))]
    [LocalizedCategory(nameof(Resources.Output_Category))]
    public OutArgument<string> OtherString { get; set; }

    // A tag used to identify the scope in the activity context
    internal static string ParentContainerPropertyTag => "ScopeActivity";

    // Object Container: Add strongly-typed objects here and they will be available in the scope's child activities.
    private readonly IObjectContainer _objectContainer;

    private Logger _log;
    private readonly bool _debugMode;
    
    #endregion


    #region Constructors

    public TestScope(IObjectContainer objectContainer, bool debugMode = false)
    {
        _debugMode = debugMode;
        if (_debugMode) return;
        
        _objectContainer = objectContainer;

        Body = new ActivityAction<IObjectContainer>
        {
            Argument = new DelegateInArgument<IObjectContainer> (ParentContainerPropertyTag),
            Handler = new Sequence { DisplayName = Resources.Do }
        };
    }

    public TestScope() : this(new ObjectContainer())
    {

    }

    #endregion


    #region Protected Methods

    protected override void CacheMetadata(NativeActivityMetadata metadata)
    {
        /* NOTE: This line has been known to cause intermittent false positives validation errors.
         * I recommend doing using the [RequiredArgument] attribute on your properties to enforce required arguments in
         * the UIPath GUI combined with strong validation in the ExecuteAsync method for more consistent results. */
        //if (TestString == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(TestString)));

        base.CacheMetadata(metadata);
    }

    // You can think of this as your Main() method for your scope.
    protected override async Task<Action<NativeActivityContext>> ExecuteAsync(NativeActivityContext  context, CancellationToken cancellationToken)
    {
        #region Optional Logging

        var debugLog = DebugLog.Get(context);
        if (!string.IsNullOrEmpty(debugLog))
        {
            _log = new Logger(debugLog);
        }

        #endregion
        
        #region Get your input values and set them to local variables.

        var teststring = TestString.Get(context);
        var rnd = new Random(DateTime.Now.Millisecond);
        var nbr = rnd.Next(0, int.MaxValue);
        var hex = nbr.ToString("x");
        
        #endregion

        #region Add execution logic HERE
        
        _objectContainer.Add(hex);
        var otherString = teststring + "\nHex Id: " + hex;
        
        #endregion
        
        #region Set any output values here to return to UiPath Studio.
        
        return (ctx) => {
            // Schedule child activities
            if (Body != null)
				ctx.ScheduleAction(Body, _objectContainer, OnCompleted, OnFaulted);

            // Outputs
            OtherString.Set(ctx, otherString);
        };
        
        #endregion
    }

    #endregion


    #region Events

    private void OnFaulted(NativeActivityFaultContext faultContext, Exception propagatedException, ActivityInstance propagatedFrom)
    {
        faultContext.CancelChildren();
        Cleanup();
    }

    private void OnCompleted(NativeActivityContext context, ActivityInstance completedInstance)
    {
        Cleanup();
    }

    #endregion


    #region Helpers
    
    private void Cleanup()
    {
        var disposableObjects = _objectContainer.Where(o => o is IDisposable);
        foreach (var obj in disposableObjects)
        {
            if (obj is IDisposable dispObject)
                dispObject.Dispose();
        }
        _objectContainer.Clear();
    }

    #endregion
}