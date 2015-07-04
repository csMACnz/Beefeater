properties {
    # build variables
    $framework = "4.5.1"		# .net framework version
    $configuration = "Release"	# build configuration
    $script:version = "1.0.0"
    $script:nugetVersion = "1.0.0"
    $script:runCoverity = $false

    # directories
    $base_dir = . resolve-path .\
    $build_output_dir = "$base_dir\src\Beefeater\bin\$configuration\"
    $test_results_dir = "$base_dir\TestResults\"
    $package_dir = "$base_dir\Package\"
    $archive_dir = "$package_dir" + "Archive"
    $nuget_pack_dir = "$package_dir" + "Pack"

    # files
    $sln_file = "$base_dir\src\Beefeater.sln"
    $nuspec_filename = "Beefeater.nuspec"
    $testOptions = ""
    $script:xunit = "$base_dir\src\packages\xunit.runners.1.9.2\tools\xunit.console.clr4.exe"
    $script:coveralls = "csmacnz.Coveralls.exe"

}

task default

task SetChocolateyPath {
	$script:chocolateyDir = $null
	if ($env:ChocolateyInstall -ne $null) {
		$script:chocolateyDir = $env:ChocolateyInstall;
	} elseif (Test-Path (Join-Path $env:SYSTEMDRIVE Chocolatey)) {
		$script:chocolateyDir = Join-Path $env:SYSTEMDRIVE Chocolatey;
	} elseif (Test-Path (Join-Path ([Environment]::GetFolderPath("CommonApplicationData")) Chocolatey)) {
		$script:chocolateyDir = Join-Path ([Environment]::GetFolderPath("CommonApplicationData")) Chocolatey;
	}

    Write-Output "Chocolatey installed at $script:chocolateyDir";
}

task RestoreNuGetPackages -depends SetChocolateyPath {
    $chocolateyBinDir = Join-Path $script:chocolateyDir -ChildPath "bin";
	$NuGetExe = Join-Path $chocolateyBinDir -ChildPath "NuGet.exe";

    exec { & $NuGetExe restore $sln_file }
}

task GitVersion -depends SetChocolateyPath {
	$chocolateyBinDir = Join-Path $script:chocolateyDir -ChildPath "bin";
	$gitVersionExe = Join-Path $chocolateyBinDir -ChildPath "GitVersion.exe";

    & $gitVersionExe /output buildserver /updateassemblyinfo true /assemblyVersionFormat Major
}

task LocalTestSettings {
    $script:xunit = "$base_dir/src/packages/xunit.runners.1.9.2/tools/xunit.console.clr4.exe"
    $script:testOptions = ""
}

task ResolveCoverallsPath {
    $script:coveralls = (Resolve-Path "src/packages/coveralls.net.*/csmacnz.coveralls.exe").ToString()
}

task AppVeyorEnvironmentSettings {
    if(Test-Path Env:\GitVersion_ClassicVersion) {
        $script:version = $env:GitVersion_ClassicVersion
        echo "version set to $script:version"
    }
    elseif (Test-Path Env:\APPVEYOR_BUILD_VERSION) {
        $script:version = $env:APPVEYOR_BUILD_VERSION
        echo "version set to $script:version"
    }
    if(Test-Path Env:\GitVersion_NuGetVersionV2) {
        $script:nugetVersion = $env:GitVersion_NuGetVersionV2
        echo "nuget version set to $script:nugetVersion"
    }
    elseif (Test-Path Env:\APPVEYOR_BUILD_VERSION) {
        $script:nugetVersion = $env:APPVEYOR_BUILD_VERSION
        echo "nuget version set to $script:nugetVersion"
    }

    $script:xunit = "xunit.console.clr4.exe"
    $script:testOptions = "/appveyor"
}

task clean {
    if (Test-Path $package_dir) {
      Remove-Item $package_dir -r
    }
    if (Test-Path $test_results_dir) {
      Remove-Item $test_results_dir -r
    }
    $archive_filename = "Beefeater.*.zip"
    if (Test-Path $archive_filename) {
      Remove-Item $archive_filename
    }
    $nupkg_filename = "Beefeater.*.nupkg"
    if (Test-Path $nupkg_filename) {
      Remove-Item $nupkg_filename
    }
    exec { msbuild "/t:Clean" "/p:Configuration=$configuration" $sln_file }
}

task build {
    exec { msbuild "/t:Clean;Build" "/p:Configuration=$configuration" $sln_file }
}

task setup-coverity-local {
  $env:APPVEYOR_BUILD_FOLDER = "."
  $env:APPVEYOR_BUILD_VERSION = $script:version
  $env:APPVEYOR_REPO_NAME = "csmacnz/Beefeater"
  "You should have set the COVERITY_TOKEN and COVERITY_EMAILenvironment variable already"
  $env:APPVEYOR_SCHEDULED_BUILD = "True"
}

task test-coverity -depends setup-coverity-local, coverity

task coverity -precondition { return $env:APPVEYOR_SCHEDULED_BUILD -eq "True" }{
  $coverityFileName = "Beefeater.coverity.$script:nugetVersion.zip"
  $PublishCoverity = (Resolve-Path ".\src\packages\PublishCoverity.*\PublishCoverity.exe").ToString()

  & cov-build --dir cov-int msbuild "/t:Clean;Build" "/p:Configuration=$configuration" $sln_file
  
  & $PublishCoverity compress -o $coverityFileName
  
  & $PublishCoverity publish -t $env:COVERITY_TOKEN -e $env:COVERITY_EMAIL -z $coverityFileName -d "AppVeyor scheduled build ($env:APPVEYOR_BUILD_VERSION)." --codeVersion $script:nugetVersion
}

task coverage -depends LocalTestSettings, build, coverage-only

task coverage-only {
    $opencover = (Resolve-Path ".\src\packages\OpenCover.*\OpenCover.Console.exe").ToString()
    exec { & $opencover -register:user -target:$script:xunit "-targetargs:""src\Beefeater.Tests\bin\$Configuration\Beefeater.Tests.dll"" /noshadow $script:testOptions" -filter:"+[Beefeater*]*" -output:BeefeaterCoverage.xml }
}

task test-coveralls -depends coverage, ResolveCoverallsPath {
    exec { & $coveralls --opencover -i BeefeaterCoverage.xml --dryrun -o coverallsTestOutput.json --repoToken "NOTAREALTOKEN" }
}

task coveralls -depends ResolveCoverallsPath -precondition { return -not $env:APPVEYOR_PULL_REQUEST_NUMBER }{
    exec { & $coveralls --opencover -i BeefeaterCoverage.xml --repoToken $env:COVERALLS_REPO_TOKEN --commitId $env:APPVEYOR_REPO_COMMIT --commitBranch $env:APPVEYOR_REPO_BRANCH --commitAuthor $env:APPVEYOR_REPO_COMMIT_AUTHOR --commitEmail $env:APPVEYOR_REPO_COMMIT_AUTHOR_EMAIL --commitMessage $env:APPVEYOR_REPO_COMMIT_MESSAGE --jobId $env:APPVEYOR_JOB_ID }
}

task archive -depends build, archive-only

task archive-only {
    $archive_filename = "Beefeater.$script:nugetVersion.zip"

    mkdir $archive_dir

    cp "$build_output_dir\Beefeater.dll" "$archive_dir"
    cp "$build_output_dir\Beefeater.ExternalAnnotations.xml" "$archive_dir"

    Write-Zip -Path "$archive_dir\*" -OutputPath $archive_filename
}

task pack -depends build, pack-only

task pack-only -depends SetChocolateyPath {

    mkdir $nuget_pack_dir
    cp "$nuspec_filename" "$nuget_pack_dir"

    #Profile 328 goes into nuget sub folder "\portable-net4+sl5+netcore45+wpa81+wp8+MonoAndroid1+MonoTouch1"
    mkdir "$nuget_pack_dir\lib"
    mkdir "$nuget_pack_dir\lib\portable-net4+sl5+netcore45+wpa81+wp8+MonoAndroid1+MonoTouch1"
    cp "$build_output_dir\Beefeater.dll" "$nuget_pack_dir\lib\portable-net4+sl5+netcore45+wpa81+wp8+MonoAndroid1+MonoTouch1"
    cp "$build_output_dir\Beefeater.ExternalAnnotations.xml" "$nuget_pack_dir\lib\portable-net4+sl5+netcore45+wpa81+wp8+MonoAndroid1+MonoTouch1"

    $Spec = [xml](get-content "$nuget_pack_dir\$nuspec_filename")
    $Spec.package.metadata.version = ([string]$Spec.package.metadata.version).Replace("{Version}", $script:nugetVersion)
    $Spec.Save("$nuget_pack_dir\$nuspec_filename")
    
    $chocolateyBinDir = Join-Path $script:chocolateyDir -ChildPath "bin";
	$NuGetExe = Join-Path $chocolateyBinDir -ChildPath "NuGet.exe";

    exec { & $NuGetExe pack "$nuget_pack_dir\$nuspec_filename" }
}

task postbuild -depends pack, archive, coverage-only, coveralls

task appveyor-install -depends GitVersion, RestoreNuGetPackages

task appveyor-build -depends build

task appveyor-test -depends AppVeyorEnvironmentSettings, postbuild, coverity
