﻿namespace PicPaySimplify.Models.DTOs
{
    public class TransactionDTO
    {
        public int PayerId { get; set; }
        public int ReceiverId { get; set; }
        public double Value { get; set; }
    }
}
