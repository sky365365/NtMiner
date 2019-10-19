﻿using NTMiner.Core;
using NTMiner.Vms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NTMiner {
    public partial class AppContext {
        public class CoinKernelViewModels : ViewModelBase {
            public static readonly CoinKernelViewModels Instance = new CoinKernelViewModels();

            private readonly Dictionary<Guid, CoinKernelViewModel> _dicById = new Dictionary<Guid, CoinKernelViewModel>();
            private CoinKernelViewModels() {
#if DEBUG
                Write.Stopwatch.Restart();
#endif
                VirtualRoot.CreateEventPath<ServerContextReInitedEvent>("ServerContext刷新后刷新VM内存", LogEnum.DevConsole,
                    action: message => {
                        _dicById.Clear();
                        Init();
                    });
                VirtualRoot.CreateEventPath<ServerContextVmsReInitedEvent>("ServerContext的VM集刷新后刷新视图界面", LogEnum.DevConsole,
                    action: message => {
                        OnPropertyChanged(nameof(AllCoinKernels));
                    });
                EventPath<CoinKernelAddedEvent>("添加了币种内核后刷新VM内存", LogEnum.DevConsole,
                    action: (message) => {
                        var coinKernelVm = new CoinKernelViewModel(message.Source);
                        _dicById.Add(message.Source.GetId(), coinKernelVm);
                        OnPropertyChanged(nameof(AllCoinKernels));
                        CoinViewModel coinVm;
                        if (AppContext.Instance.CoinVms.TryGetCoinVm((Guid)message.Source.CoinId, out coinVm)) {
                            coinVm.OnPropertyChanged(nameof(CoinViewModel.CoinKernel));
                            coinVm.OnPropertyChanged(nameof(CoinViewModel.CoinKernels));
                            coinVm.OnPropertyChanged(nameof(NTMiner.Vms.CoinViewModel.IsSupported));
                        }
                        var kernelVm = coinKernelVm.Kernel;
                        if (kernelVm != null) {
                            kernelVm.OnPropertyChanged(nameof(kernelVm.CoinKernels));
                            kernelVm.OnPropertyChanged(nameof(kernelVm.CoinVms));
                            kernelVm.OnPropertyChanged(nameof(kernelVm.SupportedCoinVms));
                            kernelVm.OnPropertyChanged(nameof(kernelVm.SupportedCoins));
                        }
                    });
                EventPath<CoinKernelUpdatedEvent>("更新了币种内核后刷新VM内存", LogEnum.DevConsole,
                    action: (message) => {
                        CoinKernelViewModel entity = _dicById[message.Source.GetId()];
                        var supportedGpu = entity.SupportedGpu;
                        Guid dualCoinGroupId = entity.DualCoinGroupId;
                        entity.Update(message.Source);
                        if (supportedGpu != entity.SupportedGpu) {
                            var coinKernels = AllCoinKernels.Where(a => a.KernelId == entity.Id);
                            foreach (var coinKernel in coinKernels) {
                                CoinViewModel coinVm;
                                if (AppContext.Instance.CoinVms.TryGetCoinVm(coinKernel.CoinId, out coinVm)) {
                                    coinVm.OnPropertyChanged(nameof(coinVm.IsSupported));
                                    coinVm.OnPropertyChanged(nameof(coinVm.CoinKernels));
                                }
                            }
                            var kernelVm = entity.Kernel;
                            kernelVm.OnPropertyChanged(nameof(kernelVm.CoinKernels));
                        }
                    });
                EventPath<CoinKernelRemovedEvent>("移除了币种内核后刷新VM内存", LogEnum.DevConsole,
                    action: (message) => {
                        CoinKernelViewModel coinKernelVm;
                        if (_dicById.TryGetValue(message.Source.GetId(), out coinKernelVm)) {
                            _dicById.Remove(message.Source.GetId());
                            OnPropertyChanged(nameof(AllCoinKernels));
                            CoinViewModel coinVm;
                            if (AppContext.Instance.CoinVms.TryGetCoinVm((Guid)message.Source.CoinId, out coinVm)) {
                                coinVm.OnPropertyChanged(nameof(CoinViewModel.CoinKernel));
                                coinVm.OnPropertyChanged(nameof(CoinViewModel.CoinKernels));
                                coinVm.OnPropertyChanged(nameof(NTMiner.Vms.CoinViewModel.IsSupported));
                            }
                            var kernelVm = coinKernelVm.Kernel;
                            kernelVm.OnPropertyChanged(nameof(kernelVm.CoinKernels));
                            kernelVm.OnPropertyChanged(nameof(kernelVm.CoinVms));
                            kernelVm.OnPropertyChanged(nameof(kernelVm.SupportedCoinVms));
                            kernelVm.OnPropertyChanged(nameof(kernelVm.SupportedCoins));
                        }
                    });
                Init();
#if DEBUG
                Write.DevTimeSpan($"耗时{Write.Stopwatch.ElapsedMilliseconds}毫秒 {this.GetType().Name}.ctor");
#endif
            }

            private void Init() {
                foreach (var item in NTMinerRoot.Instance.CoinKernelSet) {
                    _dicById.Add(item.GetId(), new CoinKernelViewModel(item));
                }
            }

            public bool TryGetCoinKernelVm(Guid id, out CoinKernelViewModel vm) {
                return _dicById.TryGetValue(id, out vm);
            }

            public List<CoinKernelViewModel> AllCoinKernels {
                get {
                    return _dicById.Values.ToList();
                }
            }
        }
    }
}
