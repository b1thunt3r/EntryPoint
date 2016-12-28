﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EntryPoint;

namespace EntryPointTests.AppOptionModels {
    public class OperandDumpModel : BaseApplicationOptions {
        [OptionParameter(
            LongName = "opt-param-1")]
        public int OptParam1 { get; set; }

        [OptionParameter(
            LongName = "opt-param-2")]
        public int OptParam2 { get; set; }

        [Option(
            LongName = "opt-1")]
        public bool Opt1 { get; set; }
    }
}