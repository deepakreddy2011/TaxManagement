﻿using ExcelMapper;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaxManagement.Infrastructure
{
    public class MuncipalityTaxDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [ExcelIgnore]
        public string Id { get; set; }

        public string Muncipality { get; set; }

        public string Duration { get; set; }

        public int TaxPriority { get; set; }

        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime StartDate { get; set; }

        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime EndDate { get; set; }

        public decimal TaxRate { get; set; }
    }
}
