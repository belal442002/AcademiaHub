﻿namespace AcademiaHub.Models.Dto.FormDetails
{
    public class FormDetailsGetRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
    }
}
