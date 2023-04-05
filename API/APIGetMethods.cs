using ErgastApi.Client;
using ErgastApi.Requests;
using ErgastApi.Responses.Models.Standings;
using ErgastApi.Responses.Models.RaceInfo;
using ErgastApi.Responses;


namespace FormulaOneStats.API
{
    public class APIGetMethods
    {
        private readonly int limit = 500; // the limit of the data we can receive
        private readonly ErgastClient client = new ErgastClient();

        #region Drivers

        /// <summary>
        ///     Get the info of a driver
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DriverResponse> GetDriverInfo(string id)
        {
            var request = new DriverInfoRequest();

            // Set the limit of data set to retrieve
            request.Limit = limit;

            request.DriverId = id;

            var response = await client.GetResponseAsync(request);

            var driver = response;
            return driver;
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Get the info of a given constructor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ConstructorResponse> GetConstructorInfo(string id)
        {
            var request = new ConstructorInfoRequest();

            // Set the limit of data set to retrieve
            request.Limit = limit;

            request.ConstructorId = id;

            var response = await client.GetResponseAsync(request);

            var constructor = response;
            return constructor;
        }
        #endregion

        #region Circuits

        public async Task<CircuitResponse> GetCircuitInfo(string? circuitId = null)
        {
            var request = new CircuitInfoRequest()
            {
                Limit= limit,
            };

            if (!string.IsNullOrEmpty(circuitId))
            {
                request.CircuitId = circuitId;
            }

            var response = await client.GetResponseAsync(request);

            var constructors = response;

            return constructors;
        }

        #endregion

        #region Seasons 

        /// <summary>
        ///     Gets a list of seasons
        /// </summary>
        /// <returns></returns>
        public async Task<IList<ErgastApi.Responses.Models.Season>> GetSeasonsList()
        {
            var request = new SeasonListRequest();

            // Set the limit of data set to retrieve
            request.Limit = limit;

            var response = await client.GetResponseAsync(request);

            var seasons = response.Seasons;
            return seasons;
        }

        #endregion

        #region Results

        /// <summary>
        ///     Get results for a specific race
        /// </summary>
        /// <param name="year"></param>
        /// <param name="round"></param>
        /// <returns></returns>
        public async Task<RaceResultsResponse> GetRaceResult(string year, int round)
        {
            var request = new RaceResultsRequest()
            {
                Limit = limit,
                Season = year,
                Round = round.ToString(),
            };
            var response = await client.GetResponseAsync(request);
            var result = response;
            return result;
        }

        #endregion

        #region Qualifying

        /// <summary>
        ///     Get the qualifying results
        /// </summary>
        /// <param name="year"></param>
        /// <param name="round"></param>
        /// <returns></returns>
        public async Task<QualifyingResultsResponse> GetQualifyingResult(string year, int round)
        {
            var request = new QualifyingResultsRequest()
            {
                Limit = limit,
                Season = year,
                Round = round.ToString(),
            };
            var response = await client.GetResponseAsync(request);
            var result = response;
            return result;
        }

        #endregion

        #region Standings

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="round"></param>
        /// <returns></returns>
        public async Task<IList<DriverStandingsList>> GetDriverStandingsRace(string year, int? round = null)
        {
            var request = new DriverStandingsRequest()
            {
                Limit = limit,
                Season = year,
                Round = round?.ToString(),
                
            };
            var response = await client.GetResponseAsync(request);
            var driverStandings = response.StandingsLists;
            return driverStandings;
        }

        /// <summary>
        ///     Get a list of constructors that can be filetered by a season and round
        /// </summary>
        /// <param name="year"></param>
        /// <param name="round"></param>
        /// <returns></returns>
        public async Task<IList<ConstructorStandingsList>> GetConstructors(string? year = null, int? round = null)
        {
            var request = new ConstructorStandingsRequest()
            {
                Limit = limit,
            };

            if (!string.IsNullOrEmpty(year))
            {
                request.Season = year;
            }
            if (round.HasValue)
            {
                if (!string.IsNullOrEmpty(request.Season))
                {
                    request.Round = round.Value.ToString();
                }
            }

            var response = await client.GetResponseAsync(request);

            var constructors = response.StandingsLists;

            return constructors;
        }

        #endregion

        #region Finishing Status

        /// <summary>
        ///     get the finishing status
        /// </summary>
        /// <param name="year"></param>
        /// <param name="round"></param>
        /// <returns></returns>
        public async Task<FinishingStatusResponse> GetFinishingStatuses(string? year = null, int? round = null)
        {
            var request = new FinishingStatusRequest()
            {
                Limit = limit,
                Season = year,
                Round = round == null ? null: round.ToString()
            };
            var response = await client.GetResponseAsync(request);
            var driverStandings = response;
            return driverStandings;
        }

        #endregion

        #region Lap Times

        /// <summary>
        ///     Get the lap time for a specific lap
        /// </summary>
        /// <param name="year"></param>
        /// <param name="round"></param>
        /// <param name="lap"></param>
        /// <returns></returns>
        public async Task<LapTimesResponse> GetLapTime(string year, int round, string driverId, int? lap = null)
        {
            var request = new LapTimesRequest()
            {
                Limit = limit,
                Season = year,
                Round = round.ToString(),
                DriverId = driverId
            };
            if (lap != null)
            {
                request.Lap = lap;
            }
            var response = await client.GetResponseAsync(request);
            var driverStandings = response;
            return driverStandings;
        }

        #endregion

        #region Pit Stops

        /// <summary>
        ///     get the information of a specific pit stop
        /// </summary>
        /// <param name="year"></param>
        /// <param name="round"></param>
        /// <param name="stopNum"></param>
        /// <returns></returns>
        public async Task<PitStopsResponse> GetPitStops(string year, int round, int? stopNum = null)
        {
            var request = new PitStopsRequest()
            {
                Limit = limit,
                Season = year,
                Round = round.ToString(),
                PitStop = stopNum
            };
            var response = await client.GetResponseAsync(request);
            var pitStopInfo = response;
            return pitStopInfo;
        }

        #endregion
    }
}
