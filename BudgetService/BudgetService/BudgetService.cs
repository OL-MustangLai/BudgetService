using System.Runtime.Intrinsics.X86;
using NSubstitute.Core;

namespace BudgetService.BudgetService;

public class BudgetService(IBudgetRepo repo)
{
    public decimal Query(DateTime start, DateTime end)
    {

        if (start > end)
        {
            return 0;
        }

        // var totalDays = (end - start).Days + 1;
        var budgetsList = repo.GetAll();
        var budgetsLookup = budgetsList.ToDictionary(x => x.YearMonth, x => x.Amount);

        var firstMonthBudget = BudgetPerDay(start, budgetsLookup) * (CalculateDays(start) - start.Day+1);

        var EndMonthBudget = BudgetPerDay(end, budgetsLookup) * end.Day;

        return (firstMonthBudget + EndMonthBudget + GetBudgetPerMonth(budgetsList,start,end));
    }

    private decimal GetBudgetPerMonth(List<Budget> budgets, DateTime start, DateTime end)
    {
        var startDate =int.Parse(start.Year.ToString()+start.Month.ToString());
        var endDate =int.Parse(end.Year.ToString()+end.Month.ToString());

        var sum = budgets.Where(x=>startDate<int.Parse(x.YearMonth)&&int.Parse(x.YearMonth)<endDate).Sum(x=>x.Amount);
        return sum;
    }

    private static decimal BudgetPerDay(DateTime dateTime, Dictionary<string, decimal> lookup)
    {
        var budgetPerDay = lookup[dateTime.ToString("yyyyMM")];
        return budgetPerDay / CalculateDays(dateTime);
    }

    private static int CalculateDays(DateTime start)
    {
        return DateTime.DaysInMonth(start.Year, start.Month);
    }
}