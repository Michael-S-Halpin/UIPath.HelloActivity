using System;
using System.Activities;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UiPath.HelloActivity.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Code;
using UiPath.Shared.Activities.Localization;
using UiPath.HelloActivity.Enums;

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

    [LocalizedCategory(nameof(Resources.Common_Category))]
    [LocalizedDisplayName(nameof(Resources.DebugLog_DisplayName))]
    [LocalizedDescription(nameof(Resources.DebugLog_Description))]
    public InArgument<string> DebugLog { get; set; }

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

    private Logger _log;
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
        #region Optional Logging

        var debugLog = DebugLog.Get(context);
        if (!string.IsNullOrEmpty(debugLog))
        {
            _log = new Logger(debugLog);
        }

        #endregion
        
        #region Get your input values and set them to local variables.

        var textInput = TextInput.Get(context);
        var filePath = FilePath.Get(context);
        var folderPath = FolderPath.Get(context);
        var name = YourName.Get(context);
        var flagFlip = FlagFlip.Get(context);
        var dataType = DataType.ToString();
        var relativePronoun = RelativePronoun.ToString().Replace( "theother", "the other" );
        
        #endregion
        
        #region Validations

        _log?.Write("Validating...");
        if (string.IsNullOrEmpty(textInput)) throw new NullReferenceException("The property 'Text Input' cannot be null or empty.");
        if (string.IsNullOrEmpty(filePath)) throw new FileNotFoundException("You did not select a file.");
        if (string.IsNullOrEmpty(folderPath)) throw new FileNotFoundException("You did not select a file.");
        if (string.IsNullOrEmpty(name)) throw new NullReferenceException("The property 'Name' cannot be null or empty.");
        _log?.WriteLine("...completed!");

        #endregion

        #region Added execution logic HERE

        _log?.WriteLine("Made it to execution logic.");
        var exists = File.Exists(filePath) ? "exists." : "does not exist!";

        _log?.WriteLine("Compiling return message.");
        var message = $"Hello {name}!\nI see {relativePronoun.ToLower()} file you selected {exists}.\nYour text input was '{textInput}'.\nYou selected {folderPath}.\nYou selected type '{dataType}'";

        _log?.Write("Flag flipping...");
        flagFlip = ! flagFlip;
        _log?.WriteLine("...done.");
        
        #endregion

        #region Set any output values here to return to UiPath Studio.

        _log?.Write("Returning to UiPath.");
        return (ctx) => 
        {
            Message.Set(ctx, message);
            FlagFlip.Set(ctx, flagFlip );
        };
        
        #endregion
    }

    #endregion
}