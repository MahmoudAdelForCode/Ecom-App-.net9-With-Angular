﻿using Ecom.Core.Entites.Product;
using Ecom.Core.Interfaces;
using Ecom.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositries
{
    internal class PhotoRepositry : GenericRepositry<Photo>, IPhotoRepositry
    {
        public PhotoRepositry(AppDbContext context) : base(context)
        {
        }
    }
}
