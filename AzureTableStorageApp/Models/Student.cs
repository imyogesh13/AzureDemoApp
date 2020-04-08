﻿using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureTableStorageApp.Models
{
    public class Student : TableEntity
    {
        public Student()
        {

        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public bool IsActive { get; set; }
    }
}