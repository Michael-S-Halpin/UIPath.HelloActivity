# UiPath.HelloActivity
A .NET 6 implementation of custom activities for UiPath using C# 10.  This project follows a similar project layout to what is produced using the UiPath custom activity creator extension for Visual Studio 2019.  The code has been refactored to using C# 10 / .NET 6 conventions. 

# Quick Start

To quickly find and replace the **UiPath.HelloActivity** solution wide to make this project your own please run the following from the project folder ***while the project is not open in your IDE***:

```ps
QuickStart.bat YourNameSpace.YourProject
```
Or alternatively you can find and replace all 'UiPath.HelloActivity' references with YourNameSpace.YourProject including project folders.

Update the references of the following files to the corresponding dlls in your UiPath installation folder:  
• System.Activities  
• System.Activities.Core.Presentation  
• System.Activities.Metadata  
• System.Activities.Presentation  
• UiPath.Workflow

Search for '**NOTE:**' in the code to find helpful comments.

# How To Video
![Training Video](https://i3.ytimg.com/vi/E0fPKq8TNo0/maxresdefault.jpg)
[Click here to watch the training video.](https://youtu.be/E0fPKq8TNo0) \
[Alternative HQ video download click here.](https://drive.google.com/file/d/1QWAcAR70ETybhzB2b-CmfcxWjoiNxEWF/view?usp=sharing)

# Detailed Steps

Clone [this repository](https://github.com/Michael-S-Halpin/UIPath.HelloActivity.git) using your favorite IDE. Then close your IDE so that it does not conflict with the QuickStart.bat script clean up.

Use the QuickStart.bat file to prep your project.
```ps
C:\Users\Your-User-Id\RiderProjects\UIPath.HelloActivity\QuickStart.bat UiPath.HelloActivity YourCompany.YourNameSpace
```

Reopen the cleaned solution in your IDE.

Depending on your UiPath installation path you may need to update your references.  These files are provided with UiPath Studio and are required for activity development.  There are typically 2 possible install paths:
```ps
C:\Users\Your-User-Id\AppData\Local\UiPath
```
Or
```ps
C:\Program Files\UiPath\Studio
```
For the **Activities** project you need to import:
• System.Activities  
• UiPath.Workflow

For the **Activities.Design** project you need to import:
• System.Activities  
• System.Activities.Core.Presentation  
• System.Activities.Metadata  
• System.Activities.Presentation  
• UiPath.Workflow

*Rider Users: Errors may persist before they go away.  So long as your project builds you are fine.*

Remove any unneeded activities. Typically this means if your activity requires a scope you will keep the **ScopeActivity** and the **TestScope** and delete the **TestActivity**. If your activity does not require a scope then keep **TestActivity** and remove **ScopeActivity** and **TestScope**.  Do the same for the **Activities.Design** project.  Remove unneeded designers from the **Designers** folder and **DesignerMetadata.cs** 

Rename the class name of your activity.

Inside your activities you can now add, remove, or rename properties and their localization data.
```c#
// Use this if you want UiPath to enforce requiring this input.
[RequiredArgument] 
// This determines how the property name come out at inside the UiPath IDE.
[LocalizedDisplayName(nameof(Resources.TestActivity_TextInput_DisplayName))]
// This determines the tool tip text that is displayed when property name is moused over.
[LocalizedDescription(nameof(Resources.TestActivity_TextInput_Description))]
// This determines what group to display this property in.
[LocalizedCategory(nameof(Resources.Input_Category))]
```

The **_debugMode** property is used during debug in cases where the UiPath IDE would provide information to the activity.  This flag can be used to let the activity know it needs to generate this info or pull it from an alternate source.

The **_log** property is used to optionally write a log file.  This is useful in cases where you are trying to debug an issue from the UiPath IDE you can use log files to determine which area of your activity is having issues.

You can add code logic inside your **ExecuteAsync** method.  Typically best practices are to instantiate your logger, get local variable from the activity properties, validate your inputs, execute core logic, then return data to UiPath.

The designer project controls the way your activity will lay out in UiPath.

*IMPORTANT: If you have to add any nuget dependencies to your activity make sure you add them to **BOTH** your **Activities** and **Activities.Design** projects to avoid dependency errors while importing your activity into UiPath later on.*

The designer.xaml files will control your activity layout.  There are example is the demonstration xaml files with examples of how to handle different data types, files, folders, etc.  Here are the important considerations: \
• ConverterParameter = `In`, `Out`, or `InOut` depending on your argument type.\
• UseLocationExpression = `True` for Out or InOut arguments otherwise `False` for in arguments\
• Binding Path = `ModelItem.FlagFlip` or `ModelItem.Properties[FlagFlip].PropertyType.GenericTypeArguments[0]` \
• Content and Tooltips use the same localization references as the **Activities** project.

When you are ready to test your code it is usually best to use the **UnitTests** which contains samples of how to call your activities.  Properties are passed in as arguments of `Dictionary<string,object>` enums and other non arguments are set during object declaration.  Returned data needs to be cast into the expected data type.

When you are ready to build a package you can edit your Designer project .csproj file to edit the Nuget package info.  It is also advisable to check your Resources.resx to make sure there is not old localization data present. Once you are ready you can build a package just like you would any other nuget package.

Once you have a nuget package simply point the UiPath package manage to it and import it.  If you need to debug from UiPath you can use the DebugLog property to generate text files that can give you useful debug information.  Generally the last message in your debug file indicates the line before your bugged line.  Once your package is ready upload to your Orchestrator.

# The case for this project.

When we started with UiPath .NET 6 was already the latest LTS framework available from Microsoft.  Not seeing any reason to look backwards we started on .NET 6 looking forward. I soon came to find out that most of the custom activity tools and documentation for creating custom activities in .NET are now backwards facing.  I also found out that getting official support for custom activities from UiPath extremely difficult.  The UiPath custom activities creator tool still relies on Visual Studio 2019 and will only create .NET framework 4.6 projects.  There are guides UiPath makes available for migrating .NET framework projects to .NET 6 but that is going backwards in order to go forwards.  I could not find a simple sample project that was made for the latest .NET and also contained examples of how to use file selector controls and combobox selections.  So after several weeks of piecing together various tutorials and forum posts I decided to assemble my findings here and make it available for who ever else is looking for this information.

# About this project.
This project contains examples of a simple activity (one with no scope required), a scope, and a scoped activity.  Scoped activities are analogous to using statements in C# and are good for keeping items that require a connection to a resource open for the duration of the activities.  
 
### This project also contains examples of the following controls:
• FilePathControl (A simple file selector)  
• ComboboxControl (A combobox with manual text entry enabled)  
• ComboBox (A list only combobox no manual entry allowed)  
• TypePresenter (Allows selection of data types.)

# To Do
• See if we can upgrade resources.resx to something more modern.  
• Adding custom icons to activities.   
• Move FolderPicker.cs to UiPath.Shared.Activities.Designer and test results with multiple activities.  
• Figure out how to call a scope properly as a UnitTest so we can discontinue debug mode pattern.  
*(Currently there is a debugMode variable in each activity and scope. This is used to avoid lines that only need to be executed when running from UiPath Studio and also to execute other lines that would only apply to making sure objects are set for your development environment. Ideally we want to be able to instantiate a scope from code in UnitTest so that we can execute every line of code all the time. I currently do not have time to figure out how to do this. If someone can take this up and figure it out that would be great. Otherwise using this debugMode pattern is how I unit test my activities before importing them into UiPath Studio.)*

# Known Issues
• ~~Out and InOut properties have binding issues in designer xaml files preventing the user from setting the out value through the xaml component which results in a type conversion error in UiPath.~~ 

I was able to get an answer for this from UiPath and this is now fixed!

• Disabled AddValidationError lines in activities because they were causing inconsistent false positives on detecting validation errors.  

• IObjectContainer is known to cause conflicts when using multiple scoped activities within a single project.  A work around I am testing is to remove it from shared activities and move it into the activity code an rename the class to something more specific to that activity so that it does not cause object conflicts.  

• Cannot use check boxes in the activity with 2 way binding causing boxes to not be checked appropriately when the project loads. I have a question out to UiPath inquiring about how we can solve this.
