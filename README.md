# UiPath.HelloActivity
A .NET 6 implementation of custom activities for UiPath using C# 10.  This project follows a similar project layout to what is produced using the UiPath custom activity creator extension for Visual Studio 2019.  The code has been refactored to using C# 10 / .NET 6 conventions. 

# Quick Start

To quickly find and replace the **UiPath.HelloActivity** solution wide to make this project your own please run the following from the project folder ***while the project is not open in your IDE***:

**QuickStart.bat YourNameSpace.YourProject**

Or alternatively you can find and replace all 'UiPath.HelloActivity' references with YourNameSpace.YourProject including project folders.

Update the references of the following files to the corresponding dlls in your UiPath installation folder:  
• System.Activities  
• System.Activities.Core.Presentation  
• System.Activities.Metadata  
• System.Activities.Presentation  
• UiPath.Workflow

Search for '**NOTE:**' in the code to find helpful comments.

<iframe
width="640"
height="480"
src="https://youtu.be/E0fPKq8TNo0"
frameborder="0"
allow="autoplay; encrypted-media"
allowfullscreen>
</iframe>

# The case for this project.
When we started with UiPath .NET 6 was already the latest LTS framework available from Microsoft.  Not seeing any reason to look backwards we started on .NET 6 looking forward. I soon came to find out that most of the custom activity tools and documentation for creating custom activities in .NET are now backwards facing.  I also found out that getting official support for custom activities from UiPath extremely difficult.  The UiPath custom activities creator tool still relies on Visual Studio 2019 and will only create .NET framework 4.6 projects.  There are guides UiPath makes available for migrating .NET framework projects to .NET 6 but that is going backwards in order to go forwards.  I could not find a simple sample project that was made for the latest .NET and also contained examples of how to use file selector controls and combobox selections.  So after several weeks of piecing together various tutorials and forum posts I decided to assemble my findings here and make it available for who ever else is looking for this information.

# About this project.
This project contains examples of a simple activity (one with no scope required), a scope, and a scopeed activity.  Scoped activities are analagous to using statements in C# and are good for keeping items that require a connection to a resource open for the duration of the activities.  
 
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
*(Currently there is a debugMode variable in each activity and scope. This is used to avoid lines that only need to be executed when running from UiPath Studio and also to execute other lines that would only apply to making sure objects are set for your devleopment environment. Ideally we want to be able to instantiate a scope from code in UnitTest so that we can execute every line of code all the time. I currently do not have time to figure out how to do this. If someone can take this up and figure it out that would be great. Otherwise using this debugMode pattern is how I unit test my activities before importing them into UiPath Studio.)*

# Known Issues
• ~~Out and InOut properties have binding issues in designer xaml files preventing the user from setting the out value through the xaml component which results in a type conversion error in UiPath.~~ 

I was able to get an answer for this from UiPath and this is now fixed!

• Disabled AddValidationError lines in activities because they were causing inconsistent false positives on detecting validation errors.  

• IObjectContainer is known to cause conflicts when using multiple scoped activities within a single project.  A work around I am testing is to remove it from shared activities and move it into the activity code an rename the class to something more specific to that activity so that it does not cause object conflicts.  

• Cannot use check boxes in the activity with 2 way binding causing boxes to not be checked appropriately when the project loads. I have a question out to UiPath inquiring about how we can solve this. 