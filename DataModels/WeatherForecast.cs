using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ForecastEvaluator.DataModels
{
    public class WeatherForecast
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ForecastID { get; set; }  
        public DateOnly RetrievalDate { get; set; }
        public DateOnly ForecastForDate { get; set; }
        public string ModelType { get; set; }
        public ICollection<ForecastDetail> ForecastDetails { get; set; }
    }
}
