﻿namespace ML.Drawls
{
    public class Drawls
    {
        public string Title { get; set; } = "";
        public string QuickContent { get; set; } = "";
        public string? Content { get; set; }
        public string ImageUrl { get; set; } = "";
        public string PostUrl { get; set; } = "";
        public string PostDate { get; set; } = "";
        public DateTime CreatedDate { get; set; } = new DateTime();
    }
}