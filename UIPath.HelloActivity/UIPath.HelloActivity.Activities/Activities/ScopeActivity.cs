using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using UiPath.HelloActivity.Activities.Code;
using UiPath.HelloActivity.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Code;
using UiPath.Shared.Activities.Localization;
using UiPath.Shared.Activities.Utilities;

// ReSharper disable CheckNamespace MemberCanBePrivate.Global UnusedAutoPropertyAccessor.Global

namespace UiPath.HelloActivity.Activities;

/// <summary>
/// This is a sample activity scope for UiPath.
/// </summary>
[LocalizedDisplayName(nameof(Resources.ScopeActivity_DisplayName))]
[LocalizedDescription(nameof(Resources.ScopeActivity_Description))]
public class ScopeActivity : ContinuableAsyncCodeActivity
{
    #region Properties - Everything in this section shows up in the 'Properties' panel in UiPath.

    /// <summary>
    /// ***OPTIONAL*** If set, continue executing the remaining activities even if the current activity has failed.
    /// </summary>
    [LocalizedCategory(nameof(Resources.Common_Category))]
    [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
    [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
    public override InArgument<bool> ContinueOnError { get; set; }

    /// <summary>
    /// ***OPTIONAL*** If set, write debug log to the specified text file. The intention of this property is to be used during/// development, testing and remediation of bugs for this activity.  When testing from UiPath there is no stepping/// through activity code and this allows an activity developer to narrow down where defects are located within the/// code of the activity. It should not be used in production setting to record data pertaining to a particular RPA.
    /// </summary>
    [LocalizedCategory(nameof(Resources.Debug_Category))]
    [LocalizedDisplayName(nameof(Resources.DebugLog_DisplayName))]
    [LocalizedDescription(nameof(Resources.DebugLog_Description))]
    public InArgument<string> DebugLog { get; set; }

    /// <summary>
    /// ***OPTIONAL*** A sample string out argument named ScopeId.
    /// </summary>
    [LocalizedDisplayName(nameof(Resources.ScopeActivity_ScopeId_DisplayName))]
    [LocalizedDescription(nameof(Resources.ScopeActivity_ScopeId_Description))]
    [LocalizedCategory(nameof(Resources.Output_Category))]
    public OutArgument<string> ScopeId { get; set; }

    // ReSharper disable once NotAccessedField.Local
    private Logger _log;
    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly bool _debugMode;
    
    #endregion


    #region Constructors

    /// <summary>
    /// Default constructor instantiates a new instance of this class.
    /// </summary>
    public ScopeActivity() : this(false)
    {
    }

    /// <summary>
    /// If debug mode is set to true the activity may use some alternate logic to fill in gaps missing from UiPath/// in order to function correctly.
    /// </summary>
    /// <param name="debugMode">If true debug logic is applied.</param>    
    public ScopeActivity(bool debugMode)
    {
        _debugMode = debugMode;
        if (_debugMode) return;
        Constraints.Add(ActivityConstraints.HasParentType<ScopeActivity, TestScope>(string.Format(Resources.ValidationScope_Error, Resources.TestScope_DisplayName)));
    }

    #endregion


    #region Protected Methods

    // ReSharper disable once RedundantOverriddenMember
    /// <summary>
    /// Implementing this method is required by UiPath.
    /// </summary>
    /// <param name="metadata">MetaData is provided by UiPath</param>
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