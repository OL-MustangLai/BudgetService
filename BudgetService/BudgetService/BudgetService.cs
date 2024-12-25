namespace BudgetService.BudgetService;

public class BudgetService(IBudgetRepo repo)
{
    public int Query(DateTime start, DateTime end)
    {
        var totalDays = (end - start).Days + 1;
        
        if (totalDays <1)
        {
            return 0;
        }
        
        var budgets = repo.GetAll();

        var daysInMonth = CalculateDays(start);

        var singleOrDefault = budgets.SingleOrDefault(x=>x.YearMonth == start.ToString("yyyyMM"));

        return (int)(singleOrDefault.Amount / daysInMonth * totalDays);
        
    }

    private static int CalculateDays(DateTime start)
    {
        return DateTime.DaysInMonth(start.Year,start.Month);
    }
}