namespace GurmeDefteriBackEndAPI.Models.ViewModel
{
    public class AddScoredFoodsRequest
    {
        public string UserEmail { get; set; }
        public string FoodName { get; set; }
        public int Score { get; set; }
    }
}
