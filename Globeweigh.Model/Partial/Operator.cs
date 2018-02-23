using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Globeweigh.UI.Touch.Model;

namespace Globeweigh.Model
{
    public partial class Operator
    {
        public long? TimeElapsed { get; set; }
        public int? TimeElapsedId { get; set; }

        public string DisplayName
        {
            get { return FirstName + " " + LastName; }
            
        }

    }
}
