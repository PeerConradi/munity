using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models
{
    public class MunitySetting
    {
        [Key]
        public string SetttingName { get; set; }

        public string SettingValue { get; set; }

        public User.MunityUser SetBy { get; set; }

        public DateTime ChangeDate { get; set; }
    }
}
