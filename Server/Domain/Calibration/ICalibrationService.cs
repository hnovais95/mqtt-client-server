using Common.Models;
using System.Threading.Tasks;

namespace Server.Domain
{
    public interface ICalibrationService
    {
        public Task<HumidityPredictionsDTO> GetPredictions(CalibrationRecordsDTO records);
    }
}
