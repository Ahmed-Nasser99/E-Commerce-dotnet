﻿namespace Platform.Model
{
    public class SubCategory
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string image { get; set; }

        public Guid categoryid { get; set; }
        public Category category { get; set; }
    }
}
