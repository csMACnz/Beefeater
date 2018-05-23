Beefeater
=========
Guard your methods' Ins and Outs. 

<img align="right" width="256px" height="256px" src="http://img.csmac.nz/Beefeater-256.svg">

[![License](http://img.shields.io/:license-mit-blue.svg)](http://csmacnz.mit-license.org)
[![NuGet](https://img.shields.io/nuget/v/Beefeater.svg)](https://www.nuget.org/packages/Beefeater)
[![NuGet](https://buildstats.info/nuget/Beefeater)](https://www.nuget.org/packages/Beefeater)
[![Source Browser](https://img.shields.io/badge/Browse-Source-green.svg)](http://sourcebrowser.io/Browse/csMACnz/Beefeater)
[![Badges](http://img.shields.io/:badges-14/14-ff6799.svg)](https://github.com/badges/badgerbadgerbadger)

[![Stories in Ready](https://badge.waffle.io/csmacnz/Beefeater.png?label=ready&title=Ready)](https://waffle.io/csmacnz/Beefeater)
[![Stories in progress](https://badge.waffle.io/csmacnz/Beefeater.png?label=in%20progress&title=In%20Progress)](https://waffle.io/csmacnz/Beefeater)
[![Issue Stats](http://www.issuestats.com/github/csMACnz/Beefeater/badge/pr)](http://www.issuestats.com/github/csMACnz/Beefeater)
[![Issue Stats](http://www.issuestats.com/github/csMACnz/Beefeater/badge/issue)](http://www.issuestats.com/github/csMACnz/Beefeater)

[![AppVeyor Build status](https://img.shields.io/appveyor/ci/MarkClearwater/Beefeater.svg)](https://ci.appveyor.com/project/MarkClearwater/Beefeater)
[![Travis Build Status](https://img.shields.io/travis/csMACnz/Beefeater.svg)](https://travis-ci.org/csMACnz/Beefeater)

[![Coverage Status](https://img.shields.io/coveralls/csMACnz/Beefeater.svg)](https://coveralls.io/r/csMACnz/Beefeater?branch=master)
[![codecov.io](http://codecov.io/github/csMACnz/Beefeater/coverage.svg?branch=master)](http://codecov.io/github/csMACnz/Beefeater?branch=master)
[![Coverity Scan Build Status](https://img.shields.io/coverity/scan/5462.svg)](https://scan.coverity.com/projects/5462)

This library contains helpers to add semantics to the optionality of your parameters and results from method calls.

This library is built on `NetStandard1.0` for maximum compatability.

Nuget
-----

You can install the nuget package using `Install-Package Beefeater` or by heading to the [Nuget Package Page](https://www.nuget.org/packages/Beefeater).


Examples
--------

```
public Option<string> Modify(NotNull<string> first, Option<string> second)
{
    return second.Match(
        v => first + v,
        () => Option<string>.None);
}

var x = Modify("Hello", "World");
var x = Modify("Hello", null);
```

----

```
public enum ErrorResult
{
    UnknownError,
    FileNotFound,
    Unauthorized
}
public Result<bool, ErrorResult> Create(NotNull<string> filePath, Option<string> second)
{
    FileStream stream;
    try
    {
        stream = File.OpenWrite(filePath);
    }
    catch (UnauthorizedAccessException ex)
    {
        return ErrorResult.Unauthorized;
    }
    catch (FileNotFoundException ex)
    {
        return ErrorResult.FileNotFound;
    }
    catch (Exception ex)
    {
        return ErrorResult.UnknownError;
    }
    using (stream)
    {
        return second.Match(
            v =>
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.WriteLine(v);
                }
                return true;
            },
            () => false);
    }
}
```

---

```
public Either<long, double> DivideByTwo(int aNumber)
{
    if (aNumber % 2 == 0)
        return aNumber / 2;
    return aNumber / (double)2;
}

var x = DivideByTwo(10);
var x = DivideByTwo(5);
```
