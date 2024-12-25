using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace BudgetService.BudgetService;

public class Tests
{
    private readonly IBudgetRepo _budgetRepo = Substitute.For<IBudgetRepo>();
    private BudgetService _budgetService;

    [SetUp]
    public void Setup()
    {
        _budgetService = new BudgetService(_budgetRepo);
    }

    [Test]
    public void Query_Month()
    {
        _budgetRepo.GetAll().Returns(
            new List<Budget>
            {
                new()
                {
                    YearMonth = "202411",
                    Amount = 30000
                }
            });

        var start = new DateTime(2024, 11, 1);
        var end = new DateTime(2024, 11, 30);

        var query = _budgetService.Query(start, end);

        query.Should().Be(30000);
    }

    [Test]
    public void Query_Day()
    {
        _budgetRepo.GetAll().Returns(
            new List<Budget>
            {
                new()
                {
                    YearMonth = "202411",
                    Amount = 30000
                }
            });

        var start = new DateTime(2024, 11, 1);
        var end = new DateTime(2024, 11, 1);

        var query = _budgetService.Query(start, end);

        query.Should().Be(1000);
    }

    [Test]
    public void Invalid_Input()
    {
        var start = new DateTime(2024, 11, 2);
        var end = new DateTime(2024, 11, 1);

        var query = _budgetService.Query(start, end);

        query.Should().Be(0);
    }
    
    [Test]
    public void Query_Days_Cross_Month()
    {
        
        _budgetRepo.GetAll().Returns(
        [
            new Budget
            {
                YearMonth = "202411",
                Amount = 30000
            },

            new Budget
            {
                YearMonth = "202412",
                Amount = 62000
            }

        ]);
        var start = new DateTime(2024, 11, 25);
        var end = new DateTime(2024, 12, 3);

        var query = _budgetService.Query(start, end);

        query.Should().Be(12000);
    }
    
    [Test]
    public void Query_Days_Cross_Over_Two_Month()
    {
        
        _budgetRepo.GetAll().Returns(
        [
            
            new Budget
            {
                YearMonth = "202410",
                Amount = 31000
            },
            new Budget
            {
                YearMonth = "202411",
                Amount = 30000
            },

            new Budget
            {
                YearMonth = "202412",
                Amount = 62000
            }

        ]);
        var start = new DateTime(2024, 10, 25);
        var end = new DateTime(2024, 12, 3);

        var query = _budgetService.Query(start, end);

        query.Should().Be(43000);
    }
}