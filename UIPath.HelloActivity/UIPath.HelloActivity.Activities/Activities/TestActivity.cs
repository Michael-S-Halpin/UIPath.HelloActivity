using System;
using System.Activities;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UiPath.HelloActivity.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Code;
using UiPath.Shared.Activities.Localization;
using UIPath.HelloActivity.Enums;

namespace UiPath.HelloActivity.Activities;


[LocalizedDisplayName(nameof(Resources.TestActivity_DisplayName))]
[LocalizedDescription(nameof(Resources.TestActivity_Description))]
public class TestActivity : ContinuableAsyncCodeActivity
{
    #region Properties - Everything in this section shows up in the 'Properties' panel in UiPath.

    /// <summary>
    /// If set, continue executing the remaining activities even if the current activity has failed.
    /// </summary>
    [LocalizedCategory(nameof(Resources.Common_Category))]
    [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
    [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
    public override InArgument<bool> ContinueOnError { get; set; }

    [LocalizedDisplayName(nameof(Resources.TestActivity_TextInput_DisplayName))]
    [LocalizedDescription(nameof(Resources.TestActivity_TextInput_Description))]
    [LocalizedCategory(nameof(Resources.Input_Category))]
    public InArgument<string> TextInput { get; set; }

    [LocalizedDisplayName(nameof(Resources.TestActivity_FolderPath_DisplayName))]
    [LocalizedDescription(nameof(Resources.TestActivity_FolderPath_Description))]
    [LocalizedCategory(nameof(Resources.Input_Category))]
    public InArgument<string> FolderPath { get; set; }

    [LocalizedDisplayName(nameof(Resources.TestActivity_FilePath_DisplayName))]
    [LocalizedDescription(nameof(Resources.TestActivity_FilePath_Description))]
    [LocalizedCategory(nameof(Resources.Input_Category))]
    public InArgument<string> FilePath { get; set; }

    [LocalizedDisplayName(nameof(Resources.TestActivity_YourName_DisplayName))]
    [LocalizedDescription(nameof(Resources.TestActivity_YourName_Description))]
    [LocalizedCategory(nameof(Resources.Input_Category))]
    public InArgument<string> YourName { get; set; }

    [LocalizedDisplayName(nameof(Resources.TestActivity_YourName_DisplayName))]
    [LocalizedDescription(nameof(Resources.TestActivity_YourName_Description))]
    [LocalizedCategory(nameof(Resources.Debug_Category))]
    public InArgument<string> LogFile { get; set; }

    [LocalizedDisplayName(nameof(Resources.TestActivity_RelativePronoun_DisplayName))]
    [LocalizedDescription(nameof(Resources.TestActivity_RelativePronoun_Description))]
    [LocalizedCategory(nameof(Resources.Input_Category))]
    public TestEnum RelativePronoun { get; set; }

    [RequiredArgument]
    [LocalizedDisplayName(nameof(Resources.TestActivity_DataType_DisplayName))]
    [LocalizedDescription(nameof(Resources.TestActivity_DataType_Description))]
    [LocalizedCategory(nameof(Resources.Input_Category))]
    public Type DataType { get; set; }

    [LocalizedDisplayName(nameof(Resources.TestActivity_Message_DisplayName))]
    [LocalizedDescription(nameof(Resources.TestActivity_Message_Description))]
    [LocalizedCategory(nameof(Resources.Output_Category))]
    public OutArgument<string> Message { get; set; }
    
    [LocalizedDisplayName(nameof(Resources.TestActivity_FlagFlip_DisplayName))]
    [LocalizedDescription(nameof(Resources.TestActivity_FlagFlip_Description))]
    [LocalizedCategory(nameof(Resources.InputOutput_Category))]
    public InOutArgument<bool> FlagFlip { get; set; }

    private readonly bool _debugMode;
    
    #endregion


    #region Constructors

    public TestActivity() : this(false)
    {
    }

    public TestActivity(bool debugMode)
    {
        _debugMode = debugMode;
        if (debugMode) return;
    }

    #endregion


    #region Protected Methods

    protected override void CacheMetadata(CodeActivityMetadata metadata)
    {
        /* NOTE: This line has been known to cause intermittent false positives validation errors.
         * I recommend doing using the [RequiredArgument] attribute on your properties to enforce required arguments in
         * the UIPath GUI combined with strong validation in the ExecuteAsync method for more consistent results. */
        //if (FilePath == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(FilePath)));

        base.CacheMetadata(metadata);
    }

    // You can think of this as your Main() method for your activity.
    protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
    {
        #region Get your input values and set them to local variables.

        var filePath = FilePath.Get(context);
        var name = YourName.Get(context);
        var flagFlip = FlagFlip.Get(context);
        var dataType = DataType.ToString();
        var relativePronoun = RelativePronoun.ToString().Replace( "theother", "the other" );
        var logFile = LogFile.Get(context);
        Logger logger = null;

        if (!string.IsNullOrEmpty(logFile))
        {
            logger = new Logger(logFile);
        }
        
        #endregion

        #region Added execution logic HERE

        logger?.Write("Made it.");
        var exists = File.Exists(filePath) ? "exists." : "does not exist!";

        var message = $"Hello {name}!\nI see {relativePronoun.ToLower()} file {exists}\nYou selected type '{dataType}'";

        flagFlip = ! flagFlip;
        
        #endregion

        #region Set any output values here to return to UiPath Studio.

        return (ctx) => 
        {
            Message.Set(ctx, message);
            FlagFlip.Set(ctx, flagFlip );
        };
        
        #endregion
    }

    #endregion
}