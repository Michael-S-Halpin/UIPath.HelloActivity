# UIPath.HelloActivity
A .NET 6 implementation of custom activities for UIPath using C# 10.  This project follows a similar project layout to what is produced using the UIPath custom activity creator extension for Visual Studio 2019.  The code has been refactored to using C# 10 / .NET 6 conventions. 

# The case for this project.
When we started with UIPath .NET 6 was already the latest LTS framework available from Microsoft.  Not seeing any reason to look backwards we started on .NET 6 looking forward. I soon came to find out that most of the custom activity tools and documentation for creating custom activities in .NET are now backwards facin.  The UIPath custom activities creator tool still relies on Visual Studio 2019 and will only create .NET framework 4.6 projects.  There are guids UIPath makes available for migrating .NET framework projects to .NET 6 but that is going backwards in order to go forwards.  I could not find a simple sample project that was made for the latest .NET and also contained examples of how to use file selector controls and combobox selections.  So after several weeks of piecing together various tutorials and forum posts I decided to assemble my findings here and make it available for who ever else is looking for this information.

# About this project.
This project contains examples of a simple activity (one with no scope required), a scope, and a scopeed activity.  Scoped activities are analagous to using statements in C# and are good for keeping items that require a connection to a resource open for the duration of the activities.  
 
**This project also contains examples of the following controls:**  
• FilePathControl (A simple file selector)  
• ComboboxControl (A combobox with manual text entry enabled)  
• ComboBox (A list only combobox no manual entry allowed)  
• TypePresenter (Allows selection of data types.)  
