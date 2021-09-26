using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RX;

public class PriorityTests
{
    [Test]
    public void WhenCreateTwoSubscribtions_AndPriorityIsDifferent_ThenCheckOrdering()
    {
        // Arrange.
        var prop = new ReactiveProperty<int>(100);
        List<int> results = new List<int>();

        // Act.

        for (int i = 0; i < 100; i++)
        {
            var temp = i;
            prop.Subscribe(async x => results.Add(100 - temp), true, i);
        }

        prop.Value = 10;

        // Assert.
        for (int i = 0; i < 100; i++)
        {
            results[i].Should().Be(100 - i);
        }
    }
}
