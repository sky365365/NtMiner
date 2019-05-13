﻿using NTMiner.Bus;
using NTMiner.Core;
using NTMiner.Vms;

namespace NTMiner {
    [MessageType(description: "打开环境变量编辑界面")]
    public class EnvironmentVariableEditCommand : Cmd {
        public EnvironmentVariableEditCommand(CoinKernelViewModel coinKernelVm, EnvironmentVariable environmentVariable) {
            this.CoinKernelVm = coinKernelVm;
            this.EnvironmentVariable = environmentVariable;
        }

        public CoinKernelViewModel CoinKernelVm { get; private set; }
        public EnvironmentVariable EnvironmentVariable { get; private set; }
    }

    [MessageType(description: "打开内核输入片段编辑界面")]
    public class InputSegmentEditCommand : Cmd {
        public InputSegmentEditCommand(CoinKernelViewModel coinKernelVm, InputSegment segment) {
            this.CoinKernelVm = coinKernelVm;
            this.Segment = segment;
        }

        public CoinKernelViewModel CoinKernelVm { get; private set; }
        public InputSegment Segment { get; private set; }
    }

    [MessageType(description: "打开币种级内核编辑界面")]
    public class CoinKernelEditCommand : Cmd {
        public CoinKernelEditCommand(FormType formType, CoinKernelViewModel source) {
            this.FormType = formType;
            this.Source = source;
        }

        public FormType FormType { get; private set; }
        public CoinKernelViewModel Source { get; private set; }
    }

    [MessageType(description: "打开币种编辑界面")]
    public class CoinEditCommand : Cmd {
        public CoinEditCommand(FormType formType, CoinViewModel source) {
            this.FormType = formType;
            this.Source = source;
        }

        public FormType FormType { get; private set; }
        public CoinViewModel Source { get; private set; }
    }
}
