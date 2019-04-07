﻿using NTMiner.Core;
using NTMiner.Vms;
using System;
using System.Linq;
using System.Windows.Controls;

namespace NTMiner.Views.Ucs {
    public partial class KernelSelect : UserControl {
        public static void ShowDialog(CoinViewModel coin) {
            KernelSelect uc = null;
            ContainerWindow.ShowWindow(new ContainerWindowViewModel {
                Title = "内核选择",
                IconName = "Icon_Kernel",
                IsDialogWindow = true,
                CloseVisible = System.Windows.Visibility.Visible
            }, ucFactory: (window) => {
                var vm = new KernelSelectViewModel(coin, isExceptedCoin: true, selectedKernel: null);
                uc = new KernelSelect(vm);
                return uc;
            }, fixedSize: true);
            if (uc != null) {
                if (uc.Vm.SelectedResult != null) {
                    int sortNumber = coin.CoinKernels.Count == 0 ? 1 : coin.CoinKernels.Max(a => a.SortNumber) + 1;
                    VirtualRoot.Execute(new AddCoinKernelCommand(new CoinKernelViewModel(Guid.NewGuid()) {
                        Args = string.Empty,
                        CoinId = coin.Id,
                        Description = string.Empty,
                        KernelId = uc.Vm.SelectedResult.Id,
                        SortNumber = sortNumber
                    }));
                }
            }
        }

        public static ContainerWindow ShowWindow(CoinViewModel coin, KernelViewModel kernel, Action<KernelViewModel> callback) {
            KernelSelect uc = null;
            var containerWindow = ContainerWindow.ShowWindow(new ContainerWindowViewModel {
                Title = "内核选择",
                IconName = "Icon_Kernel",
                IsTopMost = true,
                HasOwner = true,
                HeaderVisible = System.Windows.Visibility.Collapsed
            }, ucFactory: (window) => {
                var vm = new KernelSelectViewModel(coin, isExceptedCoin: false, selectedKernel: kernel);
                uc = new KernelSelect(vm);
                window.Deactivated += (sender, e) => {
                    callback?.Invoke(vm.SelectedResult);
                    window.Hide();
                    window.Close();
                };
                return uc;
            }, fixedSize: true);
            return containerWindow;
        }

        public KernelSelectViewModel Vm {
            get {
                return (KernelSelectViewModel)this.DataContext;
            }
        }

        public KernelSelect(KernelSelectViewModel vm) {
            this.DataContext = vm;
            InitializeComponent();
        }
    }
}
