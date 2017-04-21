@echo off
xcopy /y /s /e "$(SolutionDir)\IPeople.Connect.DataAccess\bin\Debug" "\\buildserver\Release_Writable\Assemblies\IPeople.Connect"