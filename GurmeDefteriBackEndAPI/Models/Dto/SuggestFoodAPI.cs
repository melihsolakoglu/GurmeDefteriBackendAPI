﻿namespace GurmeDefteriBackEndAPI.Models.Dto
{
    public class SuggestFoodAPI
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string Id { get; set; }
        public string Category { get; set; }
        public int Score {  get; set; }
        public string Description { get; set; }
        public string ImageBytes { get; set; }
    }
}
