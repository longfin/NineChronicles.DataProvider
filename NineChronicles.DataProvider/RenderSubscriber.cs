namespace NineChronicles.DataProvider
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Lib9c.Renderer;
    using Microsoft.Extensions.Hosting;
    using Nekoyume.Action;
    using NineChronicles.DataProvider.Store;
    using NineChronicles.Headless;
    using Serilog;

    public class RenderSubscriber : BackgroundService
    {
        private readonly BlockRenderer _blockRenderer;
        private readonly ActionRenderer _actionRenderer;
        private readonly ExceptionRenderer _exceptionRenderer;
        private readonly NodeStatusRenderer _nodeStatusRenderer;

        public RenderSubscriber(
            StandaloneContext ctx,
            MySqlStore mySqlStore
        )
        {
            _blockRenderer = ctx.NineChroniclesNodeService!.BlockRenderer;
            _actionRenderer = ctx.NineChroniclesNodeService!.ActionRenderer;
            _exceptionRenderer = ctx.NineChroniclesNodeService!.ExceptionRenderer;
            _nodeStatusRenderer = ctx.NineChroniclesNodeService!.NodeStatusRenderer;
            MySqlStore = mySqlStore;
        }

        internal MySqlStore MySqlStore { get; }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _actionRenderer.EveryRender<ActionBase>()
                .Subscribe(
                    ev =>
                    {
                        if (ev.Exception != null)
                        {
                            return;
                        }

                        if (ev.Action is HackAndSlash4 action)
                        {
                            Log.Debug("Storing HackAndSlash action in block #{0}", ev.BlockIndex);
                            MySqlStore.StoreAgent(ev.Signer.ToString());
                            MySqlStore.StoreAvatar(
                                action.avatarAddress.ToString(),
                                ev.Signer.ToString());
                            MySqlStore.StoreHackAndSlash(
                                ev.Signer.ToString(),
                                action.avatarAddress.ToString(),
                                action.stageId,
                                action.Result.IsClear
                            );
                            Log.Debug("Stored HackAndSlash action in block #{0}", ev.BlockIndex);
                        }
                    });
            return Task.CompletedTask;
        }
    }
}
