using AElf.CSharp.Core;

namespace PixiePointsApp.Processors;

public abstract class PixiePointsProcessorBase<TEvent> : ProcessorBase<TEvent> where TEvent : IEvent<TEvent>, new()
{
    public override string GetContractAddress(string chainId)
    {
        return chainId switch
        {
            ChainIdConstants.MainNetSideChainId => ContractAddressConstants.MainNetPixiePointsAddress,
            ChainIdConstants.TestNetSideChainId => ContractAddressConstants.TestNetPixiePointsAddress,
            _ => string.Empty
        };
    }
}