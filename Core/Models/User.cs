using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulkins.Domain.Models
{
    public class User : BaseEntity

    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        public string Surname { get; set; }

    }
}
