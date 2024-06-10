namespace GurmeDefteriBackEndAPI.Services
{
    public class DailyActivityCounterService
    {
        private int _dailyLoginCount;
        private int _dailyFoodRatedCount;
        private int _dailyFoodSuggestedCount;
        private readonly object _lock = new();

        public DailyActivityCounterService()
        {
            _dailyLoginCount = 0;
            _dailyFoodRatedCount = 0;
            _dailyFoodSuggestedCount = 0;
        }

        public void IncrementLoginCount()
        {
            lock (_lock)
            {
                _dailyLoginCount++;
            }
        }

        public void IncrementFoodRatedCount()
        {
            lock (_lock)
            {
                _dailyFoodRatedCount++;
            }
        }

        public void IncrementFoodSuggestedCount()
        {
            lock (_lock)
            {
                _dailyFoodSuggestedCount++;
            }
        }

        public int GetDailyLoginCount()
        {
            return _dailyLoginCount;
        }

        public int GetDailyFoodRatedCount()
        {
            return _dailyFoodRatedCount;
        }

        public int GetDailyFoodSuggestedCount()
        {
            return _dailyFoodSuggestedCount;
        }
    }
}
