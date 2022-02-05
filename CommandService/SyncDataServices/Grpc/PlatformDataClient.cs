using AutoMapper;
using CommandService.Models;
using Grpc.Net.Client;
using PlatformService;

namespace CommandService.SyncDataServices.Grpc
{
    public class PlatformDataClient : IPlatformDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public PlatformDataClient(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public IEnumerable<Platform> ReturnAllPlatforms()
        {
            Console.WriteLine($"--> Calling Grpc Service {_configuration["GrpcPlatform"]}");

            var channel = GrpcChannel.ForAddress(_configuration["GrpcPlatform"]);
            var client = new GrpcPlatform.GrpcPlatformClient(channel);
            var request = new GetAllRequest();

            try
            {
                var replay = client.GetAllPlatforms(request);
                return _mapper.Map<IEnumerable<Platform>>(replay.Platform);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not call Grpc Service {ex.Message}");
                return null;
            }

        }
    }
}