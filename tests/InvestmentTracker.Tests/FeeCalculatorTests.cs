using Xunit;
using FluentAssertions;
using InvestmentTracker.Domain.Services;
using System;

namespace InvestmentTracker.Tests;

public class FeeCalculatorTests
{
    private readonly FeeCalculator _calculator;

    public FeeCalculatorTests()
    {
        _calculator = new FeeCalculator();
    }

    [Fact]
    public void CalculateFee_ShouldCalculateCorrectly_ForOneYear()
    {
        // Arrange
        decimal principal = 1000m;
        decimal rate = 0.1m; // 10%
        DateTime start = new DateTime(2023, 1, 1);
        DateTime end = new DateTime(2024, 1, 1); // 365 days

        // Act
        var fee = _calculator.CalculateFee(principal, rate, start, end);

        // Assert
        fee.Should().Be(100m);
    }

    [Fact]
    public void CalculateFee_ShouldBeZero_ForZeroDays()
    {
        // Arrange
        decimal principal = 1000m;
        decimal rate = 0.1m;
        DateTime start = new DateTime(2023, 1, 1);
        DateTime end = new DateTime(2023, 1, 1);

        // Act
        var fee = _calculator.CalculateFee(principal, rate, start, end);

        // Assert
        fee.Should().Be(0m);
    }

    [Fact]
    public void CalculateFee_ShouldThrow_IfStartDateAfterEndDate()
    {
        // Arrange
        decimal principal = 1000m;
        decimal rate = 0.1m;
        DateTime start = new DateTime(2023, 1, 2);
        DateTime end = new DateTime(2023, 1, 1);

        // Act
        var act = () => _calculator.CalculateFee(principal, rate, start, end);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}
