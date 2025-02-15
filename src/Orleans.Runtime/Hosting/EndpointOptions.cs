using System.Net;
using System.Net.Sockets;

using Orleans.Runtime;
using Orleans.Runtime.Configuration;

namespace Orleans.Configuration
{
    /// <summary>
    /// Configures the Silo endpoint options
    /// </summary>
    public class EndpointOptions
    {
        private IPAddress advertisedIPAddress;
        public const int DEFAULT_SILO_PORT = 11111;
        private int siloPort;

        public EndpointOptions()
        {
            siloPort = DEFAULT_SILO_PORT;
        }

        /// <summary>
        /// The IP address used for clustering.
        /// </summary>
        public IPAddress AdvertisedIPAddress
        {
            get => advertisedIPAddress;
            set
            {
                if (value is null)
                {
                    throw new OrleansConfigurationException(
                        $"No listening address specified. Use {nameof(Hosting.ISiloBuilder)}.{nameof(Hosting.EndpointOptionsExtensions.ConfigureEndpoints)}(...) "
                        + $"to configure endpoints and ensure that {nameof(AdvertisedIPAddress)} is set.");
                }

                if (value == IPAddress.Any
                    || value == IPAddress.IPv6Any
                    || value == IPAddress.None
                    || value == IPAddress.IPv6None)
                {
                    throw new OrleansConfigurationException(
                        $"Invalid value specified for {nameof(AdvertisedIPAddress)}. The value was {value}");
                }

                advertisedIPAddress = value;
            }
        }

        /// <summary>
        /// The port this silo uses for silo-to-silo communication.
        /// </summary>
        public int SiloPort
        {
            get => siloPort;
            set
            {
                if (value == 0)
                {
                    throw new OrleansConfigurationException(
                        $"No listening port specified. Use {nameof(Hosting.ISiloBuilder)}.{nameof(Hosting.EndpointOptionsExtensions.ConfigureEndpoints)}(...) "
                        + $"to configure endpoints and ensure that {nameof(SiloPort)} is set.");
                }

                siloPort = value;
            }
        }        

        /// <summary>
        /// The port this silo uses for silo-to-client (gateway) communication. Specify 0 to disable gateway functionality.
        /// </summary>
        public int GatewayPort { get; set; } = DEFAULT_GATEWAY_PORT;
        public const int DEFAULT_GATEWAY_PORT = 30000;

        /// <summary>
        /// The endpoint used to listen for silo to silo communication. 
        /// If not set will default to <see cref="AdvertisedIPAddress"/> + <see cref="SiloPort"/>
        /// </summary>
        public IPEndPoint SiloListeningEndpoint { get; set; }

        /// <summary>
        /// The endpoint used to listen for silo to silo communication. 
        /// If not set will default to <see cref="AdvertisedIPAddress"/> + <see cref="GatewayPort"/>
        /// </summary>
        public IPEndPoint GatewayListeningEndpoint { get; set; }
    }
}
