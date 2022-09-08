@echo off
powershell -Command "(gc README.md) -replace 'UIPath.HelloActivity', '%1' | Out-File -encoding ASCII README.md
powershell -Command "(gc UIPath.HelloActivity.sln) -replace 'UIPath.HelloActivity', '%1' | Out-File -encoding ASCII UIPath.HelloActivity.sln
powershell -Command "(gc UIPath.HelloActivity\UIPath.HelloActivity.Activities\UIPath.HelloActivity.Activities.csproj) -replace 'UIPath.HelloActivity', '%1' | Out-File -encoding ASCII UIPath.HelloActivity\UIPath.HelloActivity.Activities\UIPath.HelloActivity.Activities.csproj
powershell -Command "(gc UIPath.HelloActivity\UIPath.HelloActivity.Activities\Properties\SharedResources.cs) -replace 'UIPath.HelloActivity', '%1' | Out-File -encoding ASCII UIPath.HelloActivity\UIPath.HelloActivity.Activities\Properties\SharedResources.cs
powershell -Command "(gc UIPath.HelloActivity\UIPath.HelloActivity.Activities.Design\Properties\Resources.resx) -replace 'UIPath.HelloActivity', '%1' | Out-File -encoding ASCII UIPath.HelloActivity\UIPath.HelloActivity.Activities.Design\Properties\Resources.resx
powershell -Command "(gc UIPath.HelloActivity\UIPath.HelloActivity.Activities.Design\Properties\SharedResources.cs) -replace 'UIPath.HelloActivity', '%1' | Out-File -encoding ASCII UIPath.HelloActivity\UIPath.HelloActivity.Activities.Design\Properties\SharedResources.cs
powershell -Command "(gc UIPath.HelloActivity\UIPath.HelloActivity.Activities\Properties\Resources.Designer.cs) -replace 'UIPath.HelloActivity', '%1' | Out-File -encoding ASCII UIPath.HelloActivity\UIPath.HelloActivity.Activities\Properties\Resources.Designer.cs
powershell -Command "(gc UIPath.HelloActivity\UIPath.HelloActivity.Activities\Activities\ScopeActivity.cs) -replace 'UIPath.HelloActivity', '%1' | Out-File -encoding ASCII UIPath.HelloActivity\UIPath.HelloActivity.Activities\Activities\ScopeActivity.cs
powershell -Command "(gc UIPath.HelloActivity\UIPath.HelloActivity.Activities\Activities\TestActivity.cs) -replace 'UIPath.HelloActivity', '%1' | Out-File -encoding ASCII UIPath.HelloActivity\UIPath.HelloActivity.Activities\Activities\TestActivity.cs
powershell -Command "(gc UIPath.HelloActivity\UIPath.HelloActivity.Activities\Activities\TestScope.cs) -replace 'UIPath.HelloActivity', '%1' | Out-File -encoding ASCII UIPath.HelloActivity\UIPath.HelloActivity.Activities\Activities\TestScope.cs
powershell -Command "(gc UIPath.HelloActivity\UIPath.HelloActivity.Activities\Enums\TestEnum.cs) -replace 'UIPath.HelloActivity', '%1' | Out-File -encoding ASCII UIPath.HelloActivity\UIPath.HelloActivity.Activities\Enums\TestEnum.cs
powershell -Command "(gc UIPath.HelloActivity\UIPath.HelloActivity.Activities.Design\DesignerMetadata.cs) -replace 'UIPath.HelloActivity', '%1' | Out-File -encoding ASCII UIPath.HelloActivity\UIPath.HelloActivity.Activities.Design\DesignerMetadata.cs
powershell -Command "(gc UIPath.HelloActivity\UIPath.HelloActivity.Activities.Design\UIPath.HelloActivity.Activities.Design.csproj) -replace 'UIPath.HelloActivity', '%1' | Out-File -encoding ASCII UIPath.HelloActivity\UIPath.HelloActivity.Activities.Design\UIPath.HelloActivity.Activities.Design.csproj
powershell -Command "(gc UIPath.HelloActivity\UIPath.HelloActivity.Activities.Design\Designers\ScopeActivityDesigner.xaml) -replace 'UIPath.HelloActivity', '%1' | Out-File -encoding ASCII UIPath.HelloActivity\UIPath.HelloActivity.Activities.Design\Designers\ScopeActivityDesigner.xaml
powershell -Command "(gc UIPath.HelloActivity\UIPath.HelloActivity.Activities.Design\Designers\ScopeActivityDesigner.xaml.cs) -replace 'UIPath.HelloActivity', '%1' | Out-File -encoding ASCII UIPath.HelloActivity\UIPath.HelloActivity.Activities.Design\Designers\ScopeActivityDesigner.xaml.cs
powershell -Command "(gc UIPath.HelloActivity\UIPath.HelloActivity.Activities.Design\Designers\TestActivityDesigner.xaml) -replace 'UIPath.HelloActivity', '%1' | Out-File -encoding ASCII UIPath.HelloActivity\UIPath.HelloActivity.Activities.Design\Designers\TestActivityDesigner.xaml
powershell -Command "(gc UIPath.HelloActivity\UIPath.HelloActivity.Activities.Design\Designers\TestActivityDesigner.xaml.cs) -replace 'UIPath.HelloActivity', '%1' | Out-File -encoding ASCII UIPath.HelloActivity\UIPath.HelloActivity.Activities.Design\Designers\TestActivityDesigner.xaml.cs
powershell -Command "(gc UIPath.HelloActivity\UIPath.HelloActivity.Activities.Design\Designers\TestScopeDesigner.xaml) -replace 'UIPath.HelloActivity', '%1' | Out-File -encoding ASCII UIPath.HelloActivity\UIPath.HelloActivity.Activities.Design\Designers\TestScopeDesigner.xaml
powershell -Command "(gc UIPath.HelloActivity\UIPath.HelloActivity.Activities.Design\Designers\TestScopeDesigner.xaml.cs) -replace 'UIPath.HelloActivity', '%1' | Out-File -encoding ASCII UIPath.HelloActivity\UIPath.HelloActivity.Activities.Design\Designers\TestScopeDesigner.xaml.cs
powershell -Command "(gc UIPath.HelloActivity\UIPath.HelloActivity.Activities.Design\Properties\Resources.Designer.cs) -replace 'UIPath.HelloActivity', '%1' | Out-File -encoding ASCII UIPath.HelloActivity\UIPath.HelloActivity.Activities.Design\Properties\Resources.Designer.cs
powershell -Command "(gc UIPath.HelloActivity\UIPath.HelloActivity.Activities.Design\Imports\FolderPicker.cs) -replace 'UIPath.HelloActivity', '%1' | Out-File -encoding ASCII UIPath.HelloActivity\UIPath.HelloActivity.Activities.Design\Imports\FolderPicker.cs
powershell -Command "(gc UnitTests\Program.cs) -replace 'UIPath.HelloActivity', '%1' | Out-File -encoding ASCII UnitTests\Program.cs
powershell -Command "(gc UnitTests\UnitTests.csproj) -replace 'UIPath.HelloActivity', '%1' | Out-File -encoding ASCII UnitTests\UnitTests.csproj
cd UIPath.HelloActivity\UIPath.HelloActivity.Activities
rename UIPath.HelloActivity.Activities.csproj %1.Activities.csproj
cd ..\UIPath.HelloActivity.Activities.Design
rename UIPath.HelloActivity.Activities.Design.csproj %1.Activities.Design.csproj
cd ..\..
rename UIPath.HelloActivity.sln %1.sln
cd UIPath.HelloActivity
rename UIPath.HelloActivity.Activities %1.Activities
rename UIPath.HelloActivity.Activities.Design %1.Activities.Design
cd ..
if exist .git goto :removegit
goto :endif
:removegit 
cd .git
del *.* /s /f /q
cd ..
rd /q /s .git
:endif
del License.txt
del README.md
rename UIPath.HelloActivity %1
cd ..
rename UIPath.HelloActivity %1
del %1\QuickStart.bat
@echo on