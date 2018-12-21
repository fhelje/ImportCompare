#r "paket:
nuget Fake.DotNet.Cli
nuget Fake.IO.FileSystem
nuget Fake.Core.Target
nuget Fake.JavaScript.Npm //"
#load ".fake/build.fsx/intellisense.fsx"
open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators
open Fake.JavaScript

Target.create "Clean" (fun _ ->
    !! "src/**/bin"
    ++ "src/**/obj"
    |> Shell.cleanDirs 
)

Target.create "BuildFrontEnd"(fun _ ->
    // npm install
    
    Npm.install (fun o ->
        { o with
            WorkingDirectory = "./src/front_end/"
        }
    )
    Npm.run "build" (fun o ->
        { o with
            WorkingDirectory = "./src/front_end/"
        }
    )
    // Copy files from build -> wwwroot
)

Target.create "Build" (fun _ ->
    !! "src/**/*.*proj"
    |> Seq.iter (DotNet.build id)
)

Target.create "All" ignore

"Clean"
  ==> "BuildFrontEnd"
  ==> "Build"
  ==> "All"

Target.runOrDefault "All"
