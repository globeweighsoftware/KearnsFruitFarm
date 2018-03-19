using System;
using System.Data.Entity;
using System.Linq;

namespace Globeweigh.Model.Custom
{

    public class BatchLoginOperator
    {
        
        public int id { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? BatchId { get; set; }
        public long? TimeElapsedTicks { get; set; }
        public int? TimeElapsedId { get; set; }
    }


}
