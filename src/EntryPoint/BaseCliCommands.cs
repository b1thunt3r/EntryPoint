﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EntryPoint.Commands;
using EntryPoint.Interfaces;
using EntryPoint.Helpers;

namespace EntryPoint {

    /// <summary>
    /// The base class which must be derived from for a CliCommands implement
    /// </summary>
    public abstract class BaseCliCommands : ICliHelpable {
        
        /// <summary>
        /// Invoked when the user invokes -h/--help with no explicit command
        /// </summary>
        /// <param name="args">Any remaining arguments after --help</param>
        [HelpInvoker]
        public abstract void OnHelpInvoked(string helpText);

    }

}