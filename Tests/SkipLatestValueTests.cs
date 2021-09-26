using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RX;

public class SkipLatestValueTests
{
    [Test]
    public void WhenSubscribe_AndSkipLatestValue_ThenShouldntBeFirstOnNext()
    {
        // Arrange.
        var p = new ReactiveProperty<int>();
        var result = -1;
        // Act.
        p.Subscribe(async x => result = 1, true);

        // Assert.
        result.Should().Be(-1);
    }


    [Test]
    public void WhenSubscribe_AndNotSkipLatestValue_ThenShouldBeExecutedFirstOnNext()
    {
        // Arrange.
        var p = new ReactiveProperty<int>();
        var result = -1;
        // Act.
        p.Subscribe(async x => result = 1, false);

        // Assert.
        result.Should().Be(1);
    }
}
