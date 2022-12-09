using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using UIPath.HelloActivity.Activities.Code;
using UiPath.HelloActivity.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Code;
using UiPath.Shared.Activities.Localization;
using UiPath.Shared.Activities.Utilities;

namespace UiPath.HelloActivity.Activities;

[LocalizedDisplayName(nameof(Resources.ScopeActivity_DisplayName))]
[LocalizedDescription(nameof(Resources.ScopeActivity_Description))]
public class ScopeActivity : ContinuableAsyncCodeActivity
{
    #region Properties - Everything in this section shows up in the 'Properties' panel in UiPath.

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

    [LocalizedDisplayName(nameof(Resources.ScopeActivity_ScopeId_DisplayName))]
    [LocalizedDescription(nameof(Resources.ScopeActivity_ScopeId_Description))]
    [LocalizedCategory(nameof(Resources.Output_Category))]
    public OutArgument<string> ScopeId { get; set; }

    private Logger _log;
    private readonly bool _debugMode;
    
    #endregion


    #region Constructors

    public ScopeActivity() : this(false)
    {
    }

    public ScopeActivity(bool debugMode)
    {
        _debugMode = debugMode;
        if (_debugMode) return;
        Constraints.Add(ActivityConstraints.HasParentType<ScopeActivity, TestScope>(string.Format(Resources.ValidationScope_Error, Resources.TestScope_DisplayName)));
    }

    #endregion


    #region Protected Methods

    protected override void CacheMetadata(CodeActivityMetadata metadata)
    {

        base.CacheMetadata(metadata);
    }

    // You can think of this as your Main() method for your activity.
    protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
    {
        #region Optional Logging

        var debugLog = DebugLog.Get(context);
        if (!string.IsNullOrEmpty(debugLog))
        {
            _log = new Logger(debugLog);
        }

        #endregion

        #region Get your input values and set them to local variables.

        // Object Container: Use objectContainer.Get<T>() to retrieve objects from the scope
        var objectContainer = context.GetFromContext<IObjectContainer>(TestScope.ParentContainerPropertyTag);
        
        #endregion
        
        #region Add execution logic HERE

        var hex = objectContainer.Get<string>();
        
        #endregion
        
        #region Set any output values here to return to UiPath Studio.

        return (ctx) => 
        {
            ScopeId.Set(ctx, hex);
        };
        
        #endregion
    }

    #endregion
}