﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EntryPoint;
using Xunit;
using EntryPoint.Exceptions;
using EntryPointTests.Commands.BaseCommandsHelpers;
using EntryPointTests.Commands.Helpers;

namespace EntryPointTests.Commands {
    public class CommandTests {

        // ** Basic Command Handling **

        // If no command passed in, there is no default set, we will get routed to the Help method
        [Fact]
        public void Commands_ExecuteNothing_NoDefaults() {
            string[] args = { };

            Assert.Throws<CommandExecutedException>(
                "HELP",
                () => Cli.Execute<CommandModel_NoDefaults>(args));
        }

        // If no command passed in, we will default to the [DefaultCommand] attribute
        [Fact]
        public void Commands_ExecuteNothing_WithDefaults() {
            string[] args = { };

            Assert.Throws<CommandExecutedException>(
                "C2",
                () => Cli.Execute<CommandModel_Defaults>(args));
        }

        // Pass in command, should be executed with a default set
        [Fact]
        public void Commands_ExecuteCommand_WithDefaults() {
            string[] args = { "C1" };

            Assert.Throws<CommandExecutedException>(
                "C1",
                () => Cli.Execute<CommandModel_Defaults>(args));
        }

        // Pass in command, should be executed wih no default set
        [Fact]
        public void Commands_ExecuteCommand_NoDefaults() {
            string[] args = { "C1" };

            Assert.Throws<CommandExecutedException>(
                "C1",
                () => Cli.Execute<CommandModel_NoDefaults>(args));
        }


        // ** Deep Command Handling **

        // Pass in command with options, should be executed and pass on unused arguments
        [Fact]
        public void Commands_ExecuteKnown_Options() {
            string[] args = { "C1", "--option", "value", "operand1" };
            string[] expected = { "--option", "value", "operand1" };

            Assert.Throws<CommandExecutedException>(
                "C1 " + string.Join(" ", expected),
                () => Cli.Execute<CommandModel_Executable>(args));
        }

        // Pass in no command with options, should be executed and pass on all arguments
        [Fact]
        public void Commands_ExecuteUnknown_Options() {
            string[] args = { "--option", "value", "operand1" };
            string[] expected = { "--option", "value", "operand1" };

            Assert.Throws<CommandExecutedException>(
                "C2 " + string.Join(" ", expected),
                () => Cli.Execute<CommandModel_Executable>(args));
        }


        // ** Help Handling **

        // Call --help, expect to be sent straight to the help method
        [Fact]
        public void Commands_ExecuteHelp() {
            string[] args = { "--help" };

            Assert.Throws<CommandExecutedException>(
                "HELP",
                () => Cli.Execute<CommandModel_Help>(args));
        }

        // Call COMMAND --help, expect to be sent to the command
        [Fact]
        public void Commands_ExecuteHelp_CommandFirst() {
            string[] args = { "C1", "--help" };

            Assert.Throws<CommandExecutedException>(
                "C1",
                () => Cli.Execute<CommandModel_Help>(args));
        }

        // Call --help with other args, expect to be sent to the help method and discard the args
        [Fact]
        public void Commands_ExecuteHelp_OtherArgs() {
            string[] args = { "--help", "some", "other", "arguments" };

            Assert.Throws<CommandExecutedException>(
                "HELP",
                () => Cli.Execute<CommandModel_Help>(args));
        }


        // ** Validation Handling **

        // Pass model with duplicate command names (case-insensitive)
        [Fact]
        public void Commands_Model_DuplicateNames() {
            string[] args = new string[] { };

            Assert.Throws<InvalidModelException>(
                () => Cli.Execute<CommandModel_DuplicateNames>(args));
        }

        // Pass model with 2 defaults
        [Fact]
        public void Commands_Model_MultipleDefaults() {
            string[] args = new string[] { };

            Assert.Throws<InvalidModelException>(
                () => Cli.Execute<CommandModel_TwoDefaults>(args));
        }

        // Check validation of Method
        [Fact]
        public void Commands_Model_InvalidMethodSignature_NoArgs() {
            string[] args = { "C1" };

            Assert.Throws<InvalidModelException>(
                () => Cli.Execute<CommandModel_MethodSig_NoArgs>(args));
        }

        // Check validation of Method
        [Fact]
        public void Commands_Model_InvalidMethodSignature_ManyArgs() {
            string[] args = { "C1" };

            Assert.Throws<InvalidModelException>(
                () => Cli.Execute<CommandModel_MethodSig_ManyArgs>(args));
        }

        // Check validation of Method
        [Fact]
        public void Commands_Model_ValidMethodSignature_CorrectArgs() {
            string[] args = { "C1" };

            Assert.Throws<CommandExecutedException>(
                "C1",
                () => Cli.Execute<CommandModel_NoDefaults>(args));
        }
    }
}
