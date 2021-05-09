using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductQuoteApp.Models
{
    [MetadataType(typeof(GeographicAreaMetaData))]
    public class GeographicAreaViewModel
    {
        public int GeographicAreaID { get; set; }
        public string Name { get; set; }

        public GeographicAreaViewModel(GeographicArea geographicArea)
        {
            this.GeographicAreaID = geographicArea.GeographicAreaID;
            this.Name = geographicArea.Name;
        }
    }
}