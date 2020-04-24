﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using NTMiner.Windows;
using System;
using System.Management;

namespace NTMiner {
    [TestClass]
    public class CpuTests {
        [TestMethod]
        public void CpuTest1() {
            for (int i = 0; i < 100; i++) {
                Console.WriteLine($"温度：{Cpu.Instance.GetTemperature().ToString("f1")} ℃");
                Console.WriteLine($"PerformanceCounter CpuUsage {Cpu.Instance.GetCurrentCpuUsage().ToString("f1")} %");
                System.Threading.Thread.Sleep(10);
            }
        }

        [TestMethod]
        public void CpuTest2() {
            for (int i = 0; i < 10000; i++) {
                Cpu.Instance.GetCurrentCpuUsage();
            }
        }

        [TestMethod]
        public void Test3() {
            // 第一次请求非常耗时，约需要600毫秒
            Cpu.Instance.GetTemperature();
        }

        [TestMethod]
        public void Test4() {
            for (int i = 0; i < 100; i++) {
                using (var mos = new ManagementObjectSearcher(@"root\WMI", "Select CurrentTemperature From MSAcpi_ThermalZoneTemperature")) {
                    foreach (ManagementObject mo in mos.Get()) {
                        Console.WriteLine(Convert.ToDouble(Convert.ToDouble(mo.GetPropertyValue("CurrentTemperature").ToString()) - 2732) / 10);
                    }
                }
            }
        }
    }
}
