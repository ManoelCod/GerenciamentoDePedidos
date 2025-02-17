﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Application.DTOs
{
    public class ItemOrderDto
    {
        public int Quantity { get; set; }
        public string Stock { get; set; }
        public string Category { get; set; }
        public bool Status { get; set; }
        public string Id { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
