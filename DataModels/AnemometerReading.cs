using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForecastEvaluator.DataModels
{
    public class AnemometerReading
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReadingID {get; set;}
        public DateOnly ReadingDate { get; set;}
        public TimeOnly Hour {  get; set;}
        public decimal WindSpeed { get; set;}
        public decimal WindGusts { get; set;}
        public int WindDirection { get; set;}
    }
}
