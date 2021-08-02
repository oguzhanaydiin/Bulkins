using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulkins.Domain.Models
{
    [NotMapped]
    public class CommonResponseModel<T>
    {
        public CommonResponseModel(T items, string state, string errmessage)
        {
            Items = items;
            State = state;
            ErrorMessage = errmessage;
        }
        public T Items { get; set; }
        public string State { get; set; }
        public string ErrorMessage { get; set; }
    }
}

