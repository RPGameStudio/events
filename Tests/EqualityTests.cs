using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RX;

public class EqualityTests
{
    private class Test { }

    [Test]
    public void WhenTryingComparePropAndItem_AndItemIsValueType_ThenShouldBeOk()
    {
        // Arrange.
        var item = 10;
        var property = new ReactiveProperty<int>(10);

        // Act.
        var result = item == property;
        var resultReverse = property == item;

        // Assert.
        result.Should().BeTrue();
        resultReverse.Should().BeTrue();
    }

    [Test]
    public void WhenTryingComparePropAndItem_AndItemIsReferenceType_ThenShouldBeOk()
    {
        // Arrange.
        var item = new Test();
        var property1 = new ReactiveProperty<Test>(item);
        var property2 = new ReactiveProperty<Test>(new Test());

        // Act.
        var result1 = item == property1;
        var result2 = item == property2;

        var result1Reverse = property1 == item;
        var result2Reverse = property2 == item;

        // Assert.

        result1.Should().BeTrue();
        result1Reverse.Should().BeTrue();
        result2.Should().BeFalse();
        result2Reverse.Should().BeFalse();
    }


    [Test]
    public void WhenTryingComparePropAndProp_AndPropsHasSimilarItem_ThenShouldBeTrue()
    {
        // Arrange.
        var item = new Test();
        var property1 = new ReactiveProperty<Test>(item);
        var property2 = new ReactiveProperty<Test>(item);

        // Act.
        var result = property2 == property1;

        // Assert.

        result.Should().BeTrue();
    }


    [Test]
    public void WhenTryingComparePropAndProp_AndPropsHasDifferentItems_ThenShouldBeTrue()
    {
        // Arrange.
        var item = new Test();
        var property1 = new ReactiveProperty<Test>(item);
        var property2 = new ReactiveProperty<Test>(new Test());

        // Act.
        var result = property2 == property1;

        // Assert.

        result.Should().BeFalse();
    }
}
