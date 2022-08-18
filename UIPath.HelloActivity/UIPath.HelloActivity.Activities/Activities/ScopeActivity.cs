using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using UiPath.HelloActivity.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using UiPath.Shared.Activities.Utilities;

namespace UiPath.HelloActivity.Activities;

[LocalizedDisplayName(nameof(Resources.ScopeActivity_DisplayName))]
[LocalizedDescription(nameof(Resources.ScopeActivity_Description))]
public class ScopeActivity : ContinuableAsyncCodeActivity
{
    #region Properties

    /// <summary>
    /// If set, continue executing the remaining activities even if the current activity has failed.
    /// </summary>
    [LocalizedCategory(nameof(Resources.Common_Category))]
    [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
    [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
    public override InArgument<bool> ContinueOnError { get; set; }

    [LocalizedDisplayName(nameof(Resources.ScopeActivity_Test_DisplayName))]
    [LocalizedDescription(nameof(Resources.ScopeActivity_Test_Description))]
    [LocalizedCategory(nameof(Resources.Output_Category))]
    public OutArgument<string> Test { get; set; }

    private bool _debugMode;
    
    #endregion


    #region Constructors

    public ScopeActivity(bool debugMode = false)
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

    protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
    {
        // Object Container: Use objectContainer.Get<T>() to retrieve objects from the scope
        var objectContainer = context.GetFromContext<IObjectContainer>(TestScope.ParentContainerPropertyTag);
        
        ///////////////////////////
        // Add execution logic HERE
        ///////////////////////////
        var hex = objectContainer.Get<string>();
        
        // Outputs
        return (ctx) => 
        {
            Test.Set(ctx, hex);
        };
    }

    #endregion
}