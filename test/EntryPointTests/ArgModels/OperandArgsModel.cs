﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EntryPoint;

namespace EntryPointTests.ArgModels {
    public class OperandArgsModel : BaseApplicationOptions {
        [Option(
            DoubleDashName = "opt-1")]
        public bool Opt1 { get; set; }

        [Option(
            DoubleDashName = "opt-2")]
        public bool Opt2 { get; set; }

        [Operand(1,
            DefaultValueBehaviour = DefaultValueBehaviourEnum.CustomValue,
            CustomDefaultValue = "NoName")]
        public string Name { get; set; }

        [Operand(2,
            DefaultValueBehaviour = DefaultValueBehaviourEnum.CustomValue,
            CustomDefaultValue = "Undefined")]
        public string Gender { get; set; }
    }
}
