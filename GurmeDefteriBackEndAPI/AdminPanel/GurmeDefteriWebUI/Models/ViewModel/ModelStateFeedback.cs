namespace GurmeDefteriWebUI.Models.ViewModel
{
    public class ModelStateFeedback
    {
        public ModelStateFeedback(bool _IsValid,string _Message) {
            IsValid= _IsValid;
            Message= _Message;
        }
        public bool IsValid { get; set; }
        public string Message { get; set; }
    }
}
