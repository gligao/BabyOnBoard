using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BabyOnBoard.Models
{
    public class DropdownValues
    {
        public enum ConditionEnum
        {
            New,
            Used
        }

        public enum TypeEnum
        {
            Dressing,
            Feeding,
            Nappy,
            Bedtime,
            Bathtime,
            Playtime,
            Safety,
            Travel
        }
    }
}