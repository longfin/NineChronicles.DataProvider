using System;
using System.Threading.Tasks;
using Libplanet.KeyStore;
using Microsoft.Extensions.Hosting;
using NineChronicles.Headless;
using NineChronicles.Headless.Properties;

namespace NineChronicles.DataProvider.Executable
{
    public class Program
    {
        public static async Task Main()
        {
            var properties = NineChroniclesNodeServiceProperties
                .GenerateLibplanetNodeServiceProperties(
                    appProtocolVersionToken,
                    genesisBlockPath,
                    host,
                    port,
                    swarmPrivateKeyString,
                    minimumDifficulty,
                    storeType,
                    storePath,
                    100,
                    iceServerStrings,
                    peerStrings,
                    trustedAppProtocolVersionSigners,
                    noMiner: true,
                    workers: workers,
                    confirmations: confirmations,
                    maximumTransactions: maximumTransactions,
                    messageTimeout: messageTimeout,
                    tipTimeout: tipTimeout,
                    demandBuffer: demandBuffer,
                    staticPeerStrings: staticPeerStrings
                );
            var nineChroniclesProperties = new NineChroniclesNodeServiceProperties()
            {
                MinerPrivateKey = null,
                Rpc = null,
                Libplanet = properties
            };
            var standaloneContext = new StandaloneContext
            {
                KeyStore = Web3KeyStore.DefaultKeyStore,
            };

            NineChroniclesNodeService nineChroniclesNodeService =
                StandaloneServices.CreateHeadless(
                    nineChroniclesProperties,
                    standaloneContext,
                    blockInterval: blockInterval,
                    reorgInterval: reorgInterval,
                    authorizedMiner: authorizedMiner,
                    txLifeTime: TimeSpan.FromMinutes(txLifeTime));
            IHostBuilder nineChroniclesNodeHostBuilder = Host.CreateDefaultBuilder();
            nineChroniclesNodeHostBuilder =
                    nineChroniclesNodeService.Configure(nineChroniclesNodeHostBuilder);

            await nineChroniclesNodeHostBuilder.RunConsoleAsync();
        }
    }
}
