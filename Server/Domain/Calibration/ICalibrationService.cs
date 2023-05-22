using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models;

namespace Server.Domain
{
    public interface ICalibrationService
    {
        public Task<IEnumerable<Double>> GetPredictions(List<CalibrationParamsDTO> calibrationParams);
    }
}
