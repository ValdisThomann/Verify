﻿using System;
using System.Threading.Tasks;
using Verify;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

#region ScrubbersSampleXunit
public class ScrubbersSample :
    VerifyBase
{
    public ScrubbersSample(ITestOutputHelper output) :
        base(output)
    {
    }
    [Fact]
    public Task Lines()
    {
        var settings = new VerifySettings();
        settings.ScrubLinesWithReplace(
            replaceLine: line =>
            {
                if (line == "LineE")
                {
                    return "NoMoreLineE";
                }
                return line;
            });
        settings.ScrubLines(removeLine: line => line.Contains("J"));
        settings.ScrubLinesContaining("b", "D");
        settings.ScrubLinesContaining(StringComparison.Ordinal, "H");
        return Verify(
            settings: settings,
            target: @"
LineA
LineB
LineC
LineD
LineE
LineH
LineI
LineJ
");
    }

    [Fact]
    public Task ScrubberAppliedAfterJsonSerialization()
    {
        var target = new ToBeScrubbed
        {
            RowVersion = "0x00000000000007D3"
        };

        var settings = new VerifySettings();
        settings.AddScrubber(
            input => input.Replace("0x00000000000007D3", "TheRowVersion"));
        return Verify(target, settings);
    }
}
#endregion