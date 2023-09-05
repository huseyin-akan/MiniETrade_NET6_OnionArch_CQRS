﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Domain.Entities.Common
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate{ get; set; }
        public virtual DateTime UpdatedDate { get; set; }
        public virtual DateTime? DeletedDate { get; set; }
    }
}
