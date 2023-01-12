@echo off
powershell -Command "(gc README.md) -replace 'UiPath.HelloActivity', '%1' | Out-File -encoding ASCII README.md
powershell -Command "(gc UiPath.HelloActivity.sln) -replace 'UiPath.HelloActivity', '%1' | Out-File -encoding ASCII UiPath.HelloActivity.sln
powershell -Command "(gc UiPath.HelloActivity\UiPath.HelloActivity.Activities\UiPath.HelloActivity.Activities.csproj) -replace 'UiPath.HelloActivity', '%1' | Out-File -encoding ASCII UiPath.HelloActivity\UiPath.HelloActivity.Activities\UiPath.HelloActivity.Activities.csproj
powershell -Command "(gc UiPath.HelloActivity\UiPath.HelloActivity.Activities\Properties\SharedResources.cs) -replace 'UiPath.HelloActivity', '%1' | Out-File -encoding ASCII UiPath.HelloActivity\UiPath.HelloActivity.Activities\Properties\SharedResources.cs
powershell -Command "(gc UiPath.HelloActivity\UiPath.HelloActivity.Activities.Design\Properties\Resources.resx) -replace 'UiPath.HelloActivity', '%1' | Out-File -encoding ASCII UiPath.HelloActivity\UiPath.HelloActivity.Activities.Design\Properties\Resources.resx
powershell -Command "(gc UiPath.HelloActivity\UiPath.HelloActivity.Activities.Design\Properties\SharedResources.cs) -replace 'UiPath.HelloActivity', '%1' | Out-File -encoding ASCII UiPath.HelloActivity\UiPath.HelloActivity.Activities.Design\Properties\SharedResources.cs
powershell -Command "(gc UiPath.HelloActivity\UiPath.HelloActivity.Activities\Properties\Resources.Designer.cs) -replace 'UiPath.HelloActivity', '%1' | Out-File -encoding ASCII UiPath.HelloActivity\UiPath.HelloActivity.Activities\Properties\Resources.Designer.cs
powershell -Command "(gc UiPath.HelloActivity\UiPath.HelloActivity.Activities\Activities\ScopeActivity.cs) -replace 'UiPath.HelloActivity', '%1' | Out-File -encoding ASCII UiPath.HelloActivity\UiPath.HelloActivity.Activities\Activities\ScopeActivity.cs
powershell -Command "(gc UiPath.HelloActivity\UiPath.HelloActivity.Activities\Activities\TestActivity.cs) -replace 'UiPath.HelloActivity', '%1' | Out-File -encoding ASCII UiPath.HelloActivity\UiPath.HelloActivity.Activities\Activities\TestActivity.cs
powershell -Command "(gc UiPath.HelloActivity\UiPath.HelloActivity.Activities\Activities\TestScope.cs) -replace 'UiPath.HelloActivity', '%1' | Out-File -encoding ASCII UiPath.HelloActivity\UiPath.HelloActivity.Activities\Activities\TestScope.cs
powershell -Command "(gc UiPath.HelloActivity\UiPath.HelloActivity.Activities\Enums\TestEnum.cs) -replace 'UiPath.HelloActivity', '%1' | Out-File -encoding ASCII UiPath.HelloActivity\UiPath.HelloActivity.Activities\Enums\TestEnum.cs
powershell -Command "(gc UiPath.HelloActivity\UiPath.HelloActivity.Activities\Code\ObjectContainer.cs) -replace 'UiPath.HelloActivity', '%1' | Out-File -encoding ASCII UiPath.HelloActivity\UiPath.HelloActivity.Activities\Code\ObjectContainer.cs
powershell -Command "(gc UiPath.HelloActivity\UiPath.HelloActivity.Activities\Code\IObjectContainer.cs) -replace 'UiPath.HelloActivity', '%1' | Out-File -encoding ASCII UiPath.HelloActivity\UiPath.HelloActivity.Activities\Code\IObjectContainer.cs
powershell -Command "(gc UiPath.HelloActivity\UiPath.HelloActivity.Activities.Design\DesignerMetadata.cs) -replace 'UiPath.HelloActivity', '%1' | Out-File -encoding ASCII UiPath.HelloActivity\UiPath.HelloActivity.Activities.Design\DesignerMetadata.cs
powershell -Command "(gc UiPath.HelloActivity\UiPath.HelloActivity.Activities.Design\UiPath.HelloActivity.Activities.Design.csproj) -replace 'UiPath.HelloActivity', '%1' | Out-File -encoding ASCII UiPath.HelloActivity\UiPath.HelloActivity.Activities.Design\UiPath.HelloActivity.Activities.Design.csproj
powershell -Command "(gc UiPath.HelloActivity\UiPath.HelloActivity.Activities.Design\Designers\ScopeActivityDesigner.xaml) -replace 'UiPath.HelloActivity', '%1' | Out-File -encoding ASCII UiPath.HelloActivity\UiPath.HelloActivity.Activities.Design\Designers\ScopeActivityDesigner.xaml
powershell -Command "(gc UiPath.HelloActivity\UiPath.HelloActivity.Activities.Design\Designers\ScopeActivityDesigner.xaml.cs) -replace 'UiPath.HelloActivity', '%1' | Out-File -encoding ASCII UiPath.HelloActivity\UiPath.HelloActivity.Activities.Design\Designers\ScopeActivityDesigner.xaml.cs
powershell -Command "(gc UiPath.HelloActivity\UiPath.HelloActivity.Activities.Design\Designers\TestActivityDesigner.xaml) -replace 'UiPath.HelloActivity', '%1' | Out-File -encoding ASCII UiPath.HelloActivity\UiPath.HelloActivity.Activities.Design\Designers\TestActivityDesigner.xaml
powershell -Command "(gc UiPath.HelloActivity\UiPath.HelloActivity.Activities.Design\Designers\TestActivityDesigner.xaml.cs) -replace 'UiPath.HelloActivity', '%1' | Out-File -encoding ASCII UiPath.HelloActivity\UiPath.HelloActivity.Activities.Design\Designers\TestActivityDesigner.xaml.cs
powershell -Command "(gc UiPath.HelloActivity\UiPath.HelloActivity.Activities.Design\Designers\TestScopeDesigner.xaml) -replace 'UiPath.HelloActivity', '%1' | Out-File -encoding ASCII UiPath.HelloActivity\UiPath.HelloActivity.Activities.Design\Designers\TestScopeDesigner.xaml
powershell -Command "(gc UiPath.HelloActivity\UiPath.HelloActivity.Activities.Design\Designers\TestScopeDesigner.xaml.cs) -replace 'UiPath.HelloActivity', '%1' | Out-File -encoding ASCII UiPath.HelloActivity\UiPath.HelloActivity.Activities.Design\Designers\TestScopeDesigner.xaml.cs
powershell -Command "(gc UiPath.HelloActivity\UiPath.HelloActivity.Activities.Design\Properties\Resources.Designer.cs) -replace 'UiPath.HelloActivity', '%1' | Out-File -encoding ASCII UiPath.HelloActivity\UiPath.HelloActivity.Activities.Design\Properties\Resources.Designer.cs
powershell -Command "(gc UnitTests\Program.cs) -replace 'UiPath.HelloActivity', '%1' | Out-File -encoding ASCII UnitTests\Program.cs
powershell -Command "(gc UnitTests\UnitTests.csproj) -replace 'UiPath.HelloActivity', '%1' | Out-File -encoding ASCII UnitTests\UnitTests.csproj
cd UiPath.HelloActivity\UiPath.HelloActivity.Activities
rename UiPath.HelloActivity.Activities.csproj %1.Activities.csproj
cd ..\UiPath.HelloActivity.Activities.Design
rename UiPath.HelloActivity.Activities.Design.csproj %1.Activities.Design.csproj
cd ..\..
rename UiPath.HelloActivity.sln %1.sln
cd UiPath.HelloActivity
rename UiPath.HelloActivity.Activities %1.Activities
rename UiPath.HelloActivity.Activities.Design %1.Activities.Design
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
rename UiPath.HelloActivity %1
cd ..
rename UiPath.HelloActivity %1
del %1\QuickStart.bat
@echo on